using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PausePanel : MonoBehaviour
{
    public GameObject settingsPanel;
    public EffectSignIn effectPausePanel;
    private void Start()
    {
        settingsPanel.SetActive(false);
    }
    public void CloseSettings()
    {
        MainMenuTwo.Instance.CloseSetingPanel();
    }
}
