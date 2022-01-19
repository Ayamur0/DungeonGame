using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour {
    private GameObject Container;

    void Start() {
        Container = transform.GetChild(0).gameObject;
    }

    public void Open() {
        Time.timeScale = 0;
        Container.SetActive(true);
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
