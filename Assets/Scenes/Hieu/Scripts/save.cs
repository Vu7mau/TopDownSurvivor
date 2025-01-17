using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class save : MonoBehaviour
{
    private Material _material;
    private Color Color;
    private string currentColor;
    public GameObject pn_home;
    private void Start()
    {
        _material = GetComponentInChildren<Renderer>().material;
        if (PlayerPrefs.HasKey("currentColor"))
        {
            currentColor = PlayerPrefs.GetString("currentColor");
            if (currentColor == "red")
            {
                Color = _material.color;
                Color.b = PlayerPrefs.GetFloat("red");
                _material.color = Color;
            }
            if (currentColor == "yellow")
            {
                Color = _material.color;
                Color.a = PlayerPrefs.GetFloat("yellow");
                _material.color = Color;
            }
        }
    }
    public void vemenu()
    {
        SceneManager.LoadScene(0);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void red()
    {        
        _material.color = Color.red;
        currentColor = "red";
        Debug.Log("red"+Color.red);
        
    }
    public void yellow()
    {
        _material.color = Color.yellow;
        currentColor = "yellow";
        Debug.Log("yellow" + Color.yellow);
    }
    public void saveColor()
    {
        PlayerPrefs.SetString("currentColor",currentColor);
        if (currentColor == "red")
        {
            PlayerPrefs.SetFloat("red",_material.color.b);            
        }
        if (currentColor == "yellow")
        {
            PlayerPrefs.SetFloat("yellow",_material.color.b);           
        }
    }
}
