using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public void OpenMode()
    {
        ModeUnlockManager.UnlockSurviveMode();
        Debug.Log("Survive mode unlocked!");
    }
    public void CloseMode()
    {
        ModeUnlockManager.ResetUnlocks();
        Debug.Log("Survive locked!");
    }
}
