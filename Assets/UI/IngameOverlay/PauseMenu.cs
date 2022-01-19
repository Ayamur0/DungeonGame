using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    private GameObject Container;

    void Start() {
        Container = transform.GetChild(0).gameObject;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (Container.activeSelf)
                ResumeGame();
            else
                PauseGame();
    }

    public void PauseGame() {
        Time.timeScale = 0;
        Container.SetActive(true);
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        Container.SetActive(false);
    }

    public void ExitGame() {
        SceneManager.LoadScene("MainMenu");
    }
}
