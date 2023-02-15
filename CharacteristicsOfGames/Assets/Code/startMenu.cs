using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startMenu : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene("Act1");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
