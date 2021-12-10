using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public enum EnemyType
    {
        SkeletonBasic,
        Archer,
        BigSkeleton, //Hammer
        Magician,
        Wiking,
        Witch
    }

    public enum EnemyDifficulty
    {
        Easy,
        Medium,
        Hard,
        Boss
    }

    private EnemyType enemies;

    private float amountEnemy;
    private float healthFactor;
    private float damagePointsFactor;
    private float shootIntervalFactor;

    [Header("Prefabs")]

    public GameObject skeleton_basic_prefab;
    public GameObject archer_prefab;
    public GameObject bigSkeleton_prefab;
    public GameObject witch_prefab;
    public GameObject wiking_prefab;
    public GameObject magician_prefab;

    [Header("Value Matrix")]
    public List<EnemyEntry> EnemyList = new List<EnemyEntry>();

    [System.Serializable]
    public struct EnemyEntry
    {
        public EnemyType Type;
        public List<GameObject> Difficulty;
    }

    public struct EnemyValues
    {
        private float healthFactor;
        private float damagePoint;
        private float searchRange;
        private float attackRange; 
        private float patrolRange;
        private float shootInterval;
    }

    public List<GameObject> GenerateEnemies(float mode, List<Transform> spawnPoints)
    {
        List<GameObject> returnList = new List<GameObject>();
        EnemyDifficulty mode_difficulty = (EnemyDifficulty)mode;

        //TODO create loop for multiple spawns 

        switch (mode_difficulty)
        {
            case EnemyDifficulty.Easy:
                amountEnemy = Random.Range(0.2f, 0.4f) * 10;
                healthFactor = damagePointsFactor = shootIntervalFactor = 1;
                enemies = (EnemyType)Random.Range(0, 2);
                for (float f = 1.0f; f <= amountEnemy; f++)
                {
                    switch (enemies)
                    {
                        case EnemyType.SkeletonBasic:
                            GameObject skeleton_object = Instantiate(skeleton_basic_prefab, new Vector3(0 + Random.Range(0.0f, 0.5f) * 10, 0, 0 * Random.Range(0.0f, 0.5f)), Quaternion.identity);
                            skeleton_object.GetComponent<EnemyController>().Init();
                            returnList.Add(skeleton_object);
                            break;
                        case EnemyType.Archer:
                            GameObject archer = Instantiate(archer_prefab, new Vector3(0 + Random.Range(0.0f, 0.8f) * 10, 0, 0 * Random.Range(0.0f, 0.8f)), Quaternion.identity);
                            archer.GetComponent<EnemyController>().Init();
                            returnList.Add(archer);
                            break;
                    }
                }
                break;
            case EnemyDifficulty.Medium:
                amountEnemy = Random.Range(0.3f, 0.4f) * 10;
                healthFactor = damagePointsFactor = 1.25f;
                shootIntervalFactor = 0.7f;
                enemies = (EnemyType)Random.Range(0, 5);
                for (float f = 1.0f; f <= amountEnemy; f++)
                {
                    switch (enemies)
                    {
                        case EnemyType.SkeletonBasic:
                            GameObject skeleton_basic_object = Instantiate(skeleton_basic_prefab, new Vector3(0 + Random.Range(0.0f, 0.5f) * 10, 0, 0 * Random.Range(0.0f, 0.5f)), Quaternion.identity);
                            skeleton_basic_object.GetComponent<EnemyController>().Init();
                            returnList.Add(skeleton_basic_object);
                            break;
                        case EnemyType.Archer:
                            GameObject archer = Instantiate(archer_prefab, new Vector3(0 + Random.Range(0.0f, 0.8f) * 10, 0, 0 * Random.Range(0.0f, 0.8f)), Quaternion.identity);
                            archer.GetComponent<EnemyController>().Init();
                            returnList.Add(archer);
                            break;
                        case EnemyType.BigSkeleton:
                            GameObject bigSkeleton_object = Instantiate(bigSkeleton_prefab, new Vector3(0 + Random.Range(0.0f, 0.5f) * 10, 0, 0 * Random.Range(0.0f, 0.5f)), Quaternion.identity);
                            bigSkeleton_object.GetComponent<EnemyController>().Init();
                            returnList.Add(bigSkeleton_object);
                            break;
                        case EnemyType.Magician:
                            GameObject mage_object = Instantiate(witch_prefab, new Vector3(0 + Random.Range(0.0f, 0.8f) * 10, 0, 0 * Random.Range(0.0f, 0.8f)), Quaternion.identity);
                            mage_object.GetComponent<EnemyController>().Init();
                            returnList.Add(mage_object);
                            break;
                        case EnemyType.Wiking:
                            GameObject wiking_object = Instantiate(wiking_prefab, new Vector3(0 + Random.Range(0.0f, 0.5f) * 10, 0, 0 * Random.Range(0.0f, 0.5f)), Quaternion.identity);
                            wiking_object.GetComponent<EnemyController>().Init();
                            returnList.Add(wiking_object);
                            break;
                        case EnemyType.Witch:
                            GameObject witch_object = Instantiate(wiking_prefab, new Vector3(0 + Random.Range(0.0f, 0.5f) * 10, 0, 0 * Random.Range(0.0f, 0.5f)), Quaternion.identity);
                            witch_object.GetComponent<EnemyController>().Init();
                            returnList.Add(witch_object);
                            break;
                    }
                }
                break;
            case EnemyDifficulty.Hard:
                amountEnemy = Random.Range(0.2f, 0.5f) * 10;
                break;
            case EnemyDifficulty.Boss:
                amountEnemy = 1;
                break;
        }

        return returnList;
    }

}
