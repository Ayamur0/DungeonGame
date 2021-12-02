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

        switch (mode_difficulty)
        {
            case EnemyDifficulty.Easy:
                amountEnemy = Random.Range(0.2f, 0.4f) * 10;
                healthFactor = damagePointsFactor = shootIntervalFactor = 1;
                enemies = (EnemyType)Random.Range(0, 2);
                for (float f= 1.0f; f <= amountEnemy; f++)
                {
                    switch (enemies)
                    {
                        case EnemyType.SkeletonBasic:
                            GameObject skeleton_object = Instantiate(skeleton_basic_prefab, new Vector3(0 + Random.Range(0.0f, 0.5f) * 10, 0, 0 * Random.Range(0.0f, 0.5f)), Quaternion.identity);
                            skeleton_object.GetComponent<EnemyController>().Init(Random.Range(0.3f, 0.5f) * 10, 2f, 8f, 2f, 10f, 2f);
                            returnList.Add(skeleton_object);
                            break;
                        case EnemyType.Archer:
                            GameObject archer = Instantiate(archer_prefab, new Vector3(0 + Random.Range(0.0f, 0.8f) * 10, 0, 0 * Random.Range(0.0f, 0.8f)), Quaternion.identity);
                            archer.GetComponent<EnemyController>().Init(Random.Range(0.2f, 0.4f) * 10, 3f, 11f, 8f, 10f, 6f);
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
                            skeleton_basic_object.GetComponent<EnemyController>().Init(Random.Range(0.3f, 0.5f) * healthFactor * 10, 2f * damagePointsFactor, 8f, 2f, 10f, 2f * shootIntervalFactor);
                            returnList.Add(skeleton_basic_object);
                            break;
                        case EnemyType.Archer:
                            GameObject archer = Instantiate(archer_prefab, new Vector3(0 + Random.Range(0.0f, 0.8f) * 10, 0, 0 * Random.Range(0.0f, 0.8f)), Quaternion.identity);
                            archer.GetComponent<EnemyController>().Init(Random.Range(0.2f, 0.4f) * healthFactor * 10, 3f * damagePointsFactor, 11f, 8f, 10f, 6f * shootIntervalFactor);
                            returnList.Add(archer);
                            break;
                        case EnemyType.BigSkeleton:
                            GameObject bigSkeleton_object = Instantiate(bigSkeleton_prefab, new Vector3(0 + Random.Range(0.0f, 0.5f) * 10, 0, 0 * Random.Range(0.0f, 0.5f)), Quaternion.identity);
                            bigSkeleton_object.GetComponent<EnemyController>().Init(Random.Range(0.6f, 0.18f) * 10, 6f, 6f, 2f, 4f, 6f);
                            returnList.Add(bigSkeleton_object);
                            break;
                        case EnemyType.Magician:
                            GameObject witch_object = Instantiate(witch_prefab, new Vector3(0 + Random.Range(0.0f, 0.8f) * 10, 0, 0 * Random.Range(0.0f, 0.8f)), Quaternion.identity);
                            witch_object.GetComponent<EnemyController>().Init(Random.Range(0.2f, 0.4f) * healthFactor * 10, 3f * damagePointsFactor, 11f, 8f, 10f, 6f * shootIntervalFactor);
                            returnList.Add(witch_object);
                            break;
                        case EnemyType.Wiking:
                            GameObject wiking_object = Instantiate(wiking_prefab, new Vector3(0 + Random.Range(0.0f, 0.5f) * 10, 0, 0 * Random.Range(0.0f, 0.5f)), Quaternion.identity);
                            wiking_object.GetComponent<EnemyController>().Init(Random.Range(0.3f, 0.5f) * healthFactor * 10, 2f * damagePointsFactor, 8f, 2f, 10f, 2f * shootIntervalFactor);
                            returnList.Add(wiking_object);
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
