using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public class Score
    {
        public int KilledEnemies = 0;
        public int CurrentStage = 1;
        public int CollectedCoins = 0;
    }

    public Score CurrentScore = new Score();

    public Action OnPlayerDied;

    public void Start()
    {
        this.CurrentScore = new Score();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            FindObjectOfType<PlayerStats>().TakeDamage(999);
        }
    }
}
