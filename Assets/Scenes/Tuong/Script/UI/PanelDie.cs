using UnityEngine;
using UnityEngine.SceneManagement;
public class PanelDie : MonoBehaviour
{
    public GameObject panelDie;
    private void Start()
    {
        panelDie.SetActive(false);
    }
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        panelDie.SetActive(false);
        LevelManager.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        panelDie.SetActive(false);
        LevelManager.Instance.LoadLevel(0); 
    }
}
