using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameOverButtons : MonoBehaviour
{
    public void PlayAgainButton()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void QuitButton()
    {
        Application.Quit(); // quitting in built version
        EditorApplication.isPlaying = false; // quitting when testing in editor version
    }
}
