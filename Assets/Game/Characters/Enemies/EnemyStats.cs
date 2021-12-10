using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="AI/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public float health = 3f;
    public float minDamagePoints;

    public float searchRange = 10f;
    public float attackRange = 5f;

    public float patrolRange = 8f;

    public float shootInterval;
    
}
