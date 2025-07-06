using UnityEngine;
using UnityEngine.SceneManagement;
public class PanelDie : Singleton<VuMonoBehaviour>
{
    public GameObject pnlDie;

    //private void Start()
    //{
    //    panelDie.SetActive(false);
    //}
    protected override void Start()
    {
        pnlDie.SetActive(false);
    }
    protected override void OnEnable()
    {
        //Time.timeScale = 0f;
    }
    //private void OnEnable()
    //{
    //    Time.timeScale = 0f;
    //}
    public void Restart()
    {
        Time.timeScale = 1f;
        pnlDie.SetActive(false);
        LevelManager.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        pnlDie.SetActive(false);
        LevelManager.Instance.LoadLevel(0);
    }
}
