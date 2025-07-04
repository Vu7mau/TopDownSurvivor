using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public void OpenMode()
    {
        ModeUnlockManager.UnlockSurviveMode();
        ModePanel.Instance.RefreshUIInstant();
        Debug.Log("Survive mode unlocked!");
    }
    public void CloseMode()
    {
        ModeUnlockManager.ResetUnlocks();
        ModePanel.Instance.RefreshUIInstant();
        Debug.Log("Survive locked!");
    }
}
