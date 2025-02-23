using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject pn_Setting;    
    public GameObject pn_die;
    public GameObject pn_win;  
    public void Win()
    {
        Time.timeScale = 0;
        pn_win.SetActive(true);
    }
    public void die()
    {
        Time.timeScale = 0;
        pn_die.SetActive(true);
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pn_Setting.SetActive(true);
            Time.timeScale = 0f;
        }
        return;
    }                  
    public void vemenu()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }    
    public void tieptuc()
    {
        Time.timeScale = 1.0f;
        pn_Setting.SetActive(false);
    }
    public void choilai()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
