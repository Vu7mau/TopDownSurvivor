using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerH : MonoBehaviour
{
    public void LoadSencePlay(int scenenumber)
    {
        SceneManager.LoadScene(scenenumber);
    }
}
