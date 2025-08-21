using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelExitGame : MonoBehaviour
{
    public GameObject panelExit;
    public GameObject background;
    private void Start()
    {
        panelExit.SetActive(false);
        background.SetActive(false);
    }
    public void Exit()
    {
        PausePanelTwo.Instance?.ClosePause();
        Debug.Log("Gọi thành công");
        background.SetActive(true);
        panelExit.SetActive(true);
        panelExit.transform.localScale = Vector3.zero;

        panelExit.transform.DOScale(1f, 0.7f).SetEase(Ease.OutBack).SetUpdate(true);
    }
    public void Close()
    {
        panelExit.transform.DOScale(0f, 0.6f).
            SetEase(Ease.InBack).
            SetUpdate(true).
            OnComplete(() =>
            {
                Time.timeScale = 1f;
                background.SetActive(false);
                panelExit.SetActive(false);
            });

    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        if (SceneManager.GetActiveScene().buildIndex != 0 &&
            SceneManager.GetActiveScene().buildIndex != 1)
        {
            PlayerScoreManager.Instance?.SendFinalScore();
        } 
        UnityEditor.EditorApplication.isPlaying = false;
#else
        if (SceneManager.GetActiveScene().buildIndex != 0 &&
            SceneManager.GetActiveScene().buildIndex != 1)
            {
                PlayerScoreManager.Instance?.SendFinalScore();
            } 
        Application.Quit();
#endif
    }
}
