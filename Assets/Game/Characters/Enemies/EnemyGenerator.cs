using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGenerator : MonoBehaviour
{
    public enum EnemyType
    {
        SkeletonBasic,
        Archer,
        BigSkeleton, //Hammer
        Magician,
        Wiking,
        Witch,
        Boss
    }

    public enum EnemyDifficulty
    {
        Easy,
        Medium,
        Hard,
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


    public List<GameObject> GenerateEnemies(float mode, List<Transform> spawnPoints, Room activeRoom)
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
                //    enemies = 6; //not implemented 
                //    break;
        }


        for (float f = 1.0f; f <= amountEnemy; f++)
        {
            Transform spawnpoint = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
            GameObject enemyObj = null;

            switch (enemies)
            {
                case EnemyType.SkeletonBasic:
                    enemyObj = Instantiate(skeleton_basic_prefab);
                    break;
                case EnemyType.Archer:
                    enemyObj = Instantiate(archer_prefab);
                    break;
                case EnemyType.BigSkeleton:
                    enemyObj = Instantiate(bigSkeleton_prefab);
                    break;
                case EnemyType.Magician:
                    enemyObj = Instantiate(witch_prefab);
                    break;
                case EnemyType.Wiking:
                    enemyObj = Instantiate(wiking_prefab);
                    break;
                case EnemyType.Witch:
                    enemyObj = Instantiate(wiking_prefab);
                    break;
            }

            if (enemyObj != null)
            {
                enemyObj.GetComponent<EnemyController>().Init(mode_difficulty, enemies, activeRoom);
                enemyObj.GetComponent<NavMeshAgent>().Warp(spawnpoint.position);
                returnList.Add(enemyObj);
            }
        }

        return returnList;
    }

}
