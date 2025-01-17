using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
//    public GameObject ProfilePlayer;    
    private Material _Material;
    private Color color;
    public GameObject player;
    private string currentColor;
    private void Start()
    {
         //Debug.Log("color : " + Color.red);
        _Material = player.GetComponent<Renderer>().material;
        if (PlayerPrefs.HasKey("currentColor"))
        {
            currentColor =PlayerPrefs.GetString("currentColor");
            if (currentColor=="red")
            {
                color = _Material.color;
                color.b = PlayerPrefs.GetFloat("red");
                _Material.color = color;                
            }
            if (currentColor == "yellow")
            {
                color = _Material.color;    
                color.a = PlayerPrefs.GetFloat("yellow");
                _Material.color = color;                
            }
        }
    }
    public void ButtonStart()
    {
        SceneManager.LoadScene(1);        
    }
    //public void ButtonProfile()
    //{
    //    ProfilePlayer.SetActive(true);
    //}
    //public void ExitProfile()
    //{
    //    ProfilePlayer.SetActive(false);
    //}       
    public void ButtonExit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }

}
