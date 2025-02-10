using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject pn_home;
    private save savemenu;
    private void Awake()
    {
        savemenu = FindObjectOfType<save>();
    }
    public void ChangeSkin()
    {
        if (savemenu != null)
        {
            savemenu.skin1();
        }
    }
    public void ButtonStart()
    {
        SceneManager.LoadScene(1);        
    }           
    public void ButtonExit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }
    public void vemenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void pnhome()
    {
        pn_home.SetActive(true);
        Time.timeScale = 0;
    }
    public void tieptuc()
    {
        Time.timeScale = 1.0f;
        pn_home.SetActive(false);
    }
    public void choilai()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
