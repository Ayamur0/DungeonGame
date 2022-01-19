using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour {
    private GameObject Container;

    public Text ScoreText;

    void Start() {
        Container = transform.GetChild(0).gameObject;
    }

    public void Open() {
        Time.timeScale = 0;
        Container.SetActive(true);

        var score = FindObjectOfType<GameManager>().CurrentScore;
        ScoreText.text = string.Format("Killed Enemies: {0}\nCurrent Stage: {1}\nCollected Coins: {2}\n",
            score.KilledEnemies, score.CurrentStage, score.CollectedCoins);
    }

    public void RestartGame() {
        Time.timeScale = 1;
        Container.SetActive(false);
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame() {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
