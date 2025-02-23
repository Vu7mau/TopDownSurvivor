using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{        
    public void ButtonStart()
    {
        SceneManager.LoadScene(2);
    }
    public void ButtonExit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }
    public void Logout()
    {
        SceneManager.LoadScene(0);
    }
}
