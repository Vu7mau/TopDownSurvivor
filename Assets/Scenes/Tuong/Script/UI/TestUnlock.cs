using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TestUnlock : MonoBehaviour
{
    public static TestUnlock Instance;
    public GameObject panelUnlock;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && panelUnlock != null)
        {
            panelUnlock.SetActive(!panelUnlock.activeSelf);
        }
    }
    public void OpenMode()
    {
        ModeUnlockManager.UnlockSurviveMode();
        ModePanel.Instance?.RefreshUIInstant();
        Debug.Log("Survive mode unlocked!");
    }
    public void CloseMode()
    {
        ModeUnlockManager.ResetUnlocks();
        ModePanel.Instance?.RefreshUIInstant();
        Debug.Log("Survive locked!");
    }
}
