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
        BigSkeleton,
        Mage,
        Wiking,
        Witch,
    }

    public enum EnemyDifficulty
    {
        Easy,
        Medium,
        Hard,
    }

    private EnemyType enemies;

    private int amountEnemy;

    [Header("Prefabs")]
    public GameObject skeleton_basic_prefab;
    public GameObject archer_prefab;
    public GameObject bigSkeleton_prefab;
    public GameObject witch_prefab;
    public GameObject wiking_prefab;
    public GameObject mage_prefab;


    public List<GameObject> GenerateEnemies(float mode, List<Transform> spawnPoints, Room activeRoom)
    {
        List<GameObject> returnList = new List<GameObject>();
        EnemyDifficulty mode_difficulty = (EnemyDifficulty)mode;
        List<EnemyType> spawnType = new List<EnemyType>( (IEnumerable<EnemyType>) System.Enum.GetValues(typeof(EnemyType)));

        int spawnRate = Random.Range(1, 3);

        do
        {
            int TypeRandomIndex = Random.Range(0, spawnType.Count);
            enemies = spawnType[TypeRandomIndex];
            spawnType.RemoveAt(TypeRandomIndex);

            switch (mode_difficulty)
            {
                case EnemyDifficulty.Easy:
                    amountEnemy = Random.Range(Mathf.Min(2, spawnPoints.Count), Mathf.Min(4, spawnPoints.Count));
                    break;
                case EnemyDifficulty.Medium:
                    amountEnemy = Random.Range(Mathf.Min(3, spawnPoints.Count), Mathf.Min(5, spawnPoints.Count));
                    break;
                case EnemyDifficulty.Hard:
                    amountEnemy = Random.Range(Mathf.Min(3, spawnPoints.Count), Mathf.Min(6, spawnPoints.Count));
                    break;
            }


            for (int f = 1; f <= amountEnemy; f++)
            {
                int PointRandomIndex = Random.Range(0, spawnPoints.Count - 1);
                //Debug.Log("("+amountEnemy+") "+(amountEnemy - f)+"<"+spawnPoints.Count);
                Transform spawnpoint = spawnPoints[PointRandomIndex];
                spawnPoints.RemoveAt(PointRandomIndex);
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
                    case EnemyType.Mage:
                        enemyObj = Instantiate(mage_prefab);
                        break;
                    case EnemyType.Wiking:
                        enemyObj = Instantiate(wiking_prefab);
                        break;
                    case EnemyType.Witch:
                        enemyObj = Instantiate(witch_prefab);
                        break;
                }

                if (enemyObj != null)
                {
                    enemyObj.GetComponent<EnemyController>().Init(mode_difficulty, enemies, activeRoom);
                    enemyObj.GetComponent<NavMeshAgent>().Warp(spawnpoint.position);
                    returnList.Add(enemyObj);
                }
                else Destroy(enemyObj);

                spawnRate--;
            }

        } while (spawnPoints.Count > 0 || spawnRate > 0);
        return returnList;
    }

}
