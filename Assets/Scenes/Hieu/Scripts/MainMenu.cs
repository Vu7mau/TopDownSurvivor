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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void Logout()
    {
        SceneManager.LoadScene(0);
    }
}
