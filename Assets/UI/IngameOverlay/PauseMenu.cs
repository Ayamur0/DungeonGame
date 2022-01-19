using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    public void PauseGame() {
        Time.timeScale = 0;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
    }

    public void ExitGame() {
        SceneManager.LoadScene("MainMenu");
    }
}
