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

    [Header("Prefabs")]

    public GameObject skeleton_basic_prefab;
    public GameObject archer_prefab;
    public GameObject bigSkeleton_prefab;
    public GameObject witch_prefab;
    public GameObject wiking_prefab;
    public GameObject magician_prefab;


    public List<GameObject> GenerateEnemies(float mode, List<Vector3> spawnPoints)
    {
        List<GameObject> returnList = new List<GameObject>();
        EnemyDifficulty mode_difficulty = (EnemyDifficulty)mode;
        enemies = (EnemyType)Random.Range(0, 5);


        //TODO create loop for multiple spawns 

        switch (mode_difficulty)
        {
            case EnemyDifficulty.Easy:
                amountEnemy = Random.Range(0.2f, 0.4f) * 10;
                break;
            case EnemyDifficulty.Medium:
                amountEnemy = Random.Range(0.3f, 0.4f) * 10;
                break;
            case EnemyDifficulty.Hard:
                amountEnemy = Random.Range(0.2f, 0.5f) * 10;
                break;
                //case EnemyDifficulty.Boss:
                //    amountEnemy = 1;
                //    //enemies = 6; //not implemented 
                //    break;
        }

        Vector3 spawnpoint = new Vector3(0 + Random.Range(0.0f, 0.5f) * 10, 0, 0 * Random.Range(0.0f, 0.5f));


        for (float f = 1.0f; f <= amountEnemy; f++)
        {
            switch (enemies)
            {
                case EnemyType.SkeletonBasic:
                    GameObject skeleton_basic_object = Instantiate(skeleton_basic_prefab, spawnpoint, Quaternion.identity);
                    skeleton_basic_object.GetComponent<EnemyController>().Init(mode_difficulty, enemies);
                    returnList.Add(skeleton_basic_object);
                    break;
                case EnemyType.Archer:
                    GameObject archer = Instantiate(archer_prefab, new Vector3(0 + Random.Range(0.0f, 0.8f) * 10, 0, 0 * Random.Range(0.0f, 0.8f)), Quaternion.identity);
                    archer.GetComponent<EnemyController>().Init(mode_difficulty, enemies);
                    returnList.Add(archer);
                    break;
                case EnemyType.BigSkeleton:
                    GameObject bigSkeleton_object = Instantiate(bigSkeleton_prefab, new Vector3(0 + Random.Range(0.0f, 0.5f) * 10, 0, 0 * Random.Range(0.0f, 0.5f)), Quaternion.identity);
                    bigSkeleton_object.GetComponent<EnemyController>().Init(mode_difficulty, enemies);
                    returnList.Add(bigSkeleton_object);
                    break;
                case EnemyType.Magician:
                    GameObject mage_object = Instantiate(witch_prefab, new Vector3(0 + Random.Range(0.0f, 0.8f) * 10, 0, 0 * Random.Range(0.0f, 0.8f)), Quaternion.identity);
                    mage_object.GetComponent<EnemyController>().Init(mode_difficulty, enemies);
                    returnList.Add(mage_object);
                    break;
                case EnemyType.Wiking:
                    GameObject wiking_object = Instantiate(wiking_prefab, new Vector3(0 + Random.Range(0.0f, 0.5f) * 10, 0, 0 * Random.Range(0.0f, 0.5f)), Quaternion.identity);
                    wiking_object.GetComponent<EnemyController>().Init(mode_difficulty, enemies);
                    returnList.Add(wiking_object);
                    break;
                case EnemyType.Witch:
                    GameObject witch_object = Instantiate(wiking_prefab, new Vector3(0 + Random.Range(0.0f, 0.5f) * 10, 0, 0 * Random.Range(0.0f, 0.5f)), Quaternion.identity);
                    witch_object.GetComponent<EnemyController>().Init(mode_difficulty, enemies);
                    returnList.Add(witch_object);
                    break;
            }
        }

        return returnList;
    }

}
