using UnityEngine;
using UnityEngine.UI;

public class TestUnlock : MonoBehaviour
{
    public GameObject panelUnlock;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            panelUnlock.SetActive(!panelUnlock.activeSelf);
        }
    }
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
