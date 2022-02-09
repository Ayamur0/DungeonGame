using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    public enum State {
        Patroling,
        Chasing,
        Attack,
    }

    private bool alive = true;

    private EnemyGenerator.EnemyType EnemyType;

    protected State currentState;

    [Header("Scripts")]
    public EnemyHealth HealthStats;
    public DamageManager DamageManager;

    [Header("ScriptableObject")]
    public EnemyStats EnemyStats;

    [Header("Meta Data")]
    public NavMeshAgent Agent;
    public GameObject Player;
    public Animator m_Animator;
    public LayerMask WhatIsGround, WhatIsPlayer;
    public Vector3 NextMovingPoint;
    public GameObject projectile;

    private bool MovePointisValid;
    private float playerDistance;
    private bool waitForTimeout = false;

    private Room activeRoom;

    private int arrowCounter;

    [Header("Effects")]
    public AudioClip[] NearAttackSounds;
    private AudioSource Audiosource;
    public GameObject DeathVFX;
    private bool vfxStarted = false;
    Component[] EffectSkinnedMesh;
    Component[] EffectMesh;

    public class ColorItem
    {
        public int Index;
        public Color Item;

        public ColorItem(int i, Color color)
        {
            Index = i;
            Item = color;
        }
    }

    public List<ColorItem> Colors = new List<ColorItem>();
    private bool HitEffectPlayed;
    public float lightTimer;
    public float lightDuration;
    public float lightningIntensity;

    private PowerupSpawner powerupSpawner;

    public void Init(EnemyGenerator.EnemyDifficulty mode, EnemyGenerator.EnemyType type, Room activeRoom) {
        this.EnemyType = type;
        Player = GameObject.FindGameObjectWithTag("Player");
        Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        currentState = State.Patroling;

        HealthStats = GetComponent<EnemyHealth>();
        DamageManager = GetComponent<DamageManager>();
        HealthStats.Init(this, type);
        DamageManager.Init(mode, type);
        Audiosource = GetComponent<AudioSource>();

        EffectSkinnedMesh = GetComponentsInChildren<SkinnedMeshRenderer>();
        EffectMesh = GetComponentsInChildren<MeshRenderer>();
        SetListOfMeshRenderer();

        lightDuration = 0.5f;
        lightningIntensity = 10.0f;

        if (type == EnemyGenerator.EnemyType.Witch)
        {
            m_Animator.SetFloat("shootingTimeMultiplier", 0.4f);
        }

        this.activeRoom = activeRoom;

    }

    private void SetListOfMeshRenderer()
    {
        int i = 0;
        foreach (SkinnedMeshRenderer mesh in EffectSkinnedMesh)
        {
            foreach (Material mat in mesh.materials)
            {
                
                Colors.Add(new ColorItem(i, mat.color));
                i++;
            }
        }
        foreach (MeshRenderer mesh in EffectMesh)
        {
            foreach (Material mat in mesh.materials)
            {
                Colors.Add(new ColorItem(i, mat.color));
                i++;
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        NextMovingPoint = ChooseNextMovingPoint();
        powerupSpawner = FindObjectOfType<PowerupSpawner>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!alive)
            return;
        switch (currentState) {
            case State.Patroling:
                if (MovePointisValid) //check if player is not alive
                {
                    Agent.SetDestination(NextMovingPoint);
                    m_Animator.SetTrigger("walking");
                    playerDistance = Vector3.Distance(transform.position, NextMovingPoint);
                    if (playerDistance < 1.5f) {
                        MovePointisValid = false;
                        NextMovingPoint = ChooseNextMovingPoint();
                    }
                } else {
                    NextMovingPoint = ChooseNextMovingPoint();
                }
                DetectPlayer();
                break;
            case State.Chasing:
                ChasePlayer();
                break;
            case State.Attack:
                Attack();
                break;
            default:
                break;
        }
    }

    public void showHitEffect(Color color)
    {
        lightTimer = lightDuration;
        StartCoroutine(StartEffect(lightTimer, color));
    }

    private IEnumerator StartEffect(float lightTimer, Color EffectColor)
    {
        if (!HitEffectPlayed)
        {
            HitEffectPlayed = true;
            List<ColorItem> ListColors = new List<ColorItem>(Colors);
            while (lightTimer > 0.0f)
            {
                float lerp = Mathf.Clamp01(lightTimer / lightDuration);
                float intensity = (lerp * lightningIntensity) + 1.0f;
                foreach (SkinnedMeshRenderer mesh in EffectSkinnedMesh)
                {
                    if (lightTimer >= 0.0f)
                    {
                        foreach (Material mat in mesh.materials)
                        {
                            mat.color = EffectColor * intensity;
                        }
                    }
                }
                foreach (MeshRenderer mesh in EffectMesh)
                {
                    if (lightTimer >= 0.0f)
                    {
                        foreach (Material mat in mesh.materials)
                        {
                            mat.color = EffectColor * intensity;
                        }
                    }
                }
                lightTimer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            foreach (SkinnedMeshRenderer mesh in EffectSkinnedMesh)
            {
                foreach (Material mat in mesh.materials)
                {
                    mat.color = ListColors[0].Item;
                    ListColors.Remove(ListColors[0]);
                }
            }
            foreach (MeshRenderer mesh in EffectMesh)
            {
                foreach (Material mat in mesh.materials)
                { 
                    mat.color = ListColors[0].Item;
                    ListColors.Remove(ListColors[0]);
                }
            }
            HitEffectPlayed = false;
            yield break;
        }
    }

    public void Die() {
        this.alive = false;
        Agent.ResetPath();

        GetComponent<CapsuleCollider>().enabled = false;

        m_Animator.ResetTrigger("walking");
        m_Animator.ResetTrigger("attacking");
        m_Animator.SetTrigger("defeated");

        if (DeathVFX && !vfxStarted) {
            var deathvfx = Instantiate(DeathVFX);
            deathvfx.transform.position = gameObject.transform.position;
            deathvfx.transform.localScale = new Vector3(2, 2, 2);
            vfxStarted = true;

            FindObjectOfType<GameManager>().CurrentScore.KilledEnemies++;
            Destroy(gameObject, 1f);
        }

        int r = Random.Range(0, 5);
        if (r == 0) {
            if (powerupSpawner == null)
                powerupSpawner = FindObjectOfType<PowerupSpawner>();
            List<Vector3> v = new List<Vector3>();
            v.Add(this.transform.position);
            powerupSpawner.SpawnCoins(v);
        }
    }


    private Vector3 ChooseNextMovingPoint() {
        Vector3 randomDirRange = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        Vector3 walkingPoint = randomDirRange * Random.Range(EnemyStats.patrolRange, EnemyStats.patrolRange);

        if (this.activeRoom) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + walkingPoint, -transform.up, out hit, 2f, WhatIsGround)) {
                RaycastHit sphereHit;
                if (Physics.SphereCast(transform.position + walkingPoint, 2f, transform.up, out sphereHit, 0.1f)) {
                    if (sphereHit.transform.gameObject.tag == "Decoration") {
                        return transform.position + walkingPoint;
                    }
                }
                var roomID = this.activeRoom.gameObject.GetInstanceID();
                // path: NavMeshPlane/Ground/Colliders/Room
                if (hit.transform.parent.parent.parent.gameObject.GetInstanceID() == roomID) {
                    MovePointisValid = true;
                }
            }
        }

        return transform.position + walkingPoint;
    }



    private void DetectPlayer() {
        if (Vector3.Distance(Player.transform.position, transform.position) < EnemyStats.searchRange) {
            RaycastHit hit;
            Vector3 originPosition = transform.position + new Vector3(0,1,0);
            Vector3 PlayerPosition = Player.transform.position + new Vector3(0,1,0);
            Debug.DrawRay(originPosition, PlayerPosition - originPosition, Color.black);
            if (Physics.Raycast(originPosition, PlayerPosition - originPosition, out hit, EnemyStats.searchRange))
            {
                if (hit.transform.gameObject.tag != "Player")
                {
                    return;
                }
            }
            else return;
            currentState = State.Chasing;
        }
    }

    private void ChasePlayer() {
        Agent.SetDestination(Player.transform.position);
        if (Vector3.Distance(Player.transform.position, transform.position) < EnemyStats.attackRange) {
            RaycastHit sphereHit;
            Vector3 originPosition = transform.position + new Vector3(0, 1, 0);
            Vector3 PlayerPosition = Player.transform.position + new Vector3(0, 1, 0);
            if (Physics.Raycast(originPosition, PlayerPosition - originPosition, out sphereHit, EnemyStats.attackRange))
            {
                if (sphereHit.transform.gameObject.tag != "Player")
                {
                    return;
                }
            }
            currentState = State.Attack;
        }
    }
    public void ActivateChasing()
    {
        currentState = State.Chasing;
    }

    private void Attack() {
        Agent.SetDestination(transform.position);
        transform.LookAt(new Vector3(Player.transform.position.x, 0, Player.transform.position.z));

        if (!waitForTimeout) {
            waitForTimeout = true;
            m_Animator.ResetTrigger("walking");
            m_Animator.SetTrigger("attacking");
            Invoke(nameof(SetShootTimeout), EnemyStats.shootInterval);
        }
        if (Vector3.Distance(transform.position, Player.transform.position) > EnemyStats.attackRange) {
            currentState = State.Chasing;
            m_Animator.SetTrigger("walking");
            m_Animator.ResetTrigger("attacking");
        }

    }

    private void SetShootTimeout() {
        waitForTimeout = false;
    }

    private void SendNearAttackDamage() {
            Player.GetComponent<PlayerAPI>().TakeDamage(DamageManager.GetDamage());
            if (NearAttackSounds != null)
            {
                if (NearAttackSounds.Length > 0)
                {
                    Audiosource.clip = NearAttackSounds[Random.Range(0, NearAttackSounds.Length)];
                    Audiosource.Play();
                }
            }
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnemyStats.attackRange);
        Gizmos.DrawWireSphere(NextMovingPoint, 1);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, EnemyStats.searchRange);
    }

    private void ShootProjectile() {
        Vector3 spawnPosition = transform.position + transform.forward + new Vector3(0, 1.5f, 0);
        GameObject projectileObj;

        switch (EnemyType) {
            case EnemyGenerator.EnemyType.Archer:
                if (arrowCounter > 3)
                {
                    projectileObj = Instantiate(projectile, spawnPosition, gameObject.transform.rotation);
                    projectileObj.GetComponentInChildren<ArrowEffect>().Setup(Player.transform.position, 15f, DamageManager.GetDamage());
                    projectileObj = Instantiate(projectile, spawnPosition, gameObject.transform.rotation * Quaternion.Euler(0, 30, 0));
                    projectileObj.GetComponentInChildren<ArrowEffect>().Setup(Player.transform.position, 15f, DamageManager.GetDamage());
                    projectileObj = Instantiate(projectile, spawnPosition, gameObject.transform.rotation * Quaternion.Euler(0, -30, 0)); 
                    projectileObj.GetComponentInChildren<ArrowEffect>().Setup(Player.transform.position, 15f, DamageManager.GetDamage());
                    arrowCounter = 0;
                }
                else
                {
                    projectileObj = Instantiate(projectile, spawnPosition, gameObject.transform.rotation);
                    projectileObj.GetComponentInChildren<ArrowEffect>().Setup(Player.transform.position, 15f, DamageManager.GetDamage());
                    arrowCounter++;
                }
                break;
            case EnemyGenerator.EnemyType.Mage:
                StartCoroutine(MageShots());
                break;
            case EnemyGenerator.EnemyType.Witch:
                projectileObj = Instantiate(projectile, spawnPosition, Quaternion.identity);
                projectileObj.GetComponent<PotionExplosion>().Setup(Player.transform.position, 8f, DamageManager.GetDamage());
                break;

        }
    }

    private IEnumerator MageShots(){
        if (!waitForTimeout)
        {
            GameObject projectileObj;

            waitForTimeout = true;
            for (int i = 0; i < 3; i++)
            {
                Vector3 spawnPosition = transform.position + transform.forward + new Vector3(0, 1.5f, 0);
                projectileObj = Instantiate(projectile, spawnPosition, gameObject.transform.rotation);
                projectileObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                StartCoroutine(GrowFireball(projectileObj));
                projectileObj.GetComponent<FireballExplosion>().Setup(Player.transform.position, 7f, DamageManager.GetDamage());
                yield return new WaitForSeconds(.7f);
            }
            waitForTimeout = false;
        }
    }

    private IEnumerator GrowFireball(GameObject projectileObj)
    {
        while (projectileObj.transform.localScale.x < 1.0f)
        {
            projectileObj.transform.localScale += new Vector3(1.0f, 1.0f, 1.0f) * Time.deltaTime;
            yield return new WaitForSeconds(0);
        }
    }
}
