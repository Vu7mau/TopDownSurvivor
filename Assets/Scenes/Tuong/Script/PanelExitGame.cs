using DG.Tweening;
using UnityEngine;

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
        Debug.Log("Gọi thành công");
        background.SetActive(true);
        panelExit.SetActive(true);
        panelExit.transform.localScale = Vector3.zero;

        panelExit.transform.DOScale(1f, 0.7f).SetEase(Ease.OutBack);
    }
    public void Close()
    {
        panelExit.transform.DOScale(0f, 0.6f).SetEase(Ease.InBack);
        Invoke(nameof(DisablePanel), 0.5f);
    }
    private void DisablePanel()
    {
        background.SetActive(false);
        panelExit.SetActive(false);
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
