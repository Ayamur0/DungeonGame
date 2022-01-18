using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : Projectile {
    public float Lifetime = 2f;
    public float Speed;
    private float direction = 0f;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float arcHeight = 5;
    private float dist;
    private bool targetReached;
    private AudioSource audioSource;

    // hide deprecated collider property
    new private Collider collider;
    private HashSet<Collider> enmiesInRange = new HashSet<Collider>();

    // Start is called before the first frame update
    override protected void Start() {
        this.audioSource = GetComponent<AudioSource>();
        collider = GetComponent<Collider>();
        startPos = transform.position;
        direction = transform.rotation.eulerAngles.y;
        targetPos = transform.position + transform.forward * Speed * Lifetime;
        dist = Vector3.Distance(startPos, targetPos);
        StartCoroutine("Explode");
        StartCoroutine("EmitParticles");
        ParticleSystem.MainModule particles = GetComponent<ParticleSystem>().main;
        Destroy(gameObject, Lifetime + particles.duration);
    }

    // Update is called once per frame
    override protected void Update() {
        if (targetReached)
            return;
        float x0 = startPos.x;
        float z0 = startPos.z;
        float x1 = targetPos.x;
        float z1 = targetPos.z;
        transform.position += transform.forward * Speed * Time.deltaTime;
        float arc = arcHeight * (transform.position.x - x0 + transform.position.z - z0) * (transform.position.x - x1 + transform.position.z - z1) / (-0.25f * dist * dist);
        transform.position = new Vector3(transform.position.x, startPos.y + arc, transform.position.z);
    }

    private void OnTriggerEnter(Collider other) {
        EnemyController enemyController = other.GetComponent<EnemyController>();
        if (enemyController != null)
            enmiesInRange.Add(other);
    }

    private void OnTriggerExit(Collider other) {
        EnemyController enemyController = other.GetComponent<EnemyController>();
        if (enemyController != null)
            enmiesInRange.Remove(other);
    }

    private IEnumerator EmitParticles() {
        yield return new WaitForSeconds(Lifetime);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        transform.rotation = new Quaternion(0, 0, 0, 1);
        ParticleSystem.MainModule particles = GetComponent<ParticleSystem>().main;
        particles.startSizeX = new ParticleSystem.MinMaxCurve(particles.startSizeX.constantMin * transform.localScale.x, particles.startSizeX.constantMax * transform.localScale.x);
        particles.startSizeY = new ParticleSystem.MinMaxCurve(particles.startSizeY.constantMin * transform.localScale.x, particles.startSizeY.constantMax * transform.localScale.x);
        particles.startSizeZ = new ParticleSystem.MinMaxCurve(particles.startSizeZ.constantMin * transform.localScale.x, particles.startSizeZ.constantMax * transform.localScale.x);
        GetComponent<ParticleSystem>().Play();
        targetReached = true;
    }

    private IEnumerator Explode() {
        yield return new WaitForSeconds(Lifetime);

        this.audioSource.pitch = Random.Range(0.5f, 1f);
        this.audioSource.Play();
        transform.GetChild(0).GetComponent<Light>().enabled = true;
        DecreaseLightIntensity();

        foreach (Collider c in enmiesInRange) {
            c.GetComponent<EnemyHealth>().ReceiveDamage(playerStats.GetDamage());

            if (onHitEffect != null)
                onHitEffect(c.gameObject);
        }
    }

    private IEnumerator DecreaseLightIntensity() {
        Light light = transform.GetChild(0).GetComponent<Light>();
        float startIntensity = light.intensity;
        for (float i = 0; i < startIntensity; i += 0.1f) {
            light.intensity -= 0.1f;
            Debug.Log(light.intensity);
            yield return new WaitForSeconds(0.1f / startIntensity);
        }
    }
}
