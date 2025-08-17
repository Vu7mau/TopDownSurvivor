using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PanelDie : Singleton<VuMonoBehaviour>
{
    public GameObject pnlDie;
    public TextMeshProUGUI score;

    //private void Start()
    //{
    //    panelDie.SetActive(false);
    //}
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if (pnlDie.activeSelf)
            {
                pnlDie.SetActive(false);
                Time.timeScale = 1f;
                CrownEffect.Instance?.Close();
            }
            else
            {
                pnlDie.SetActive(true);
                CrownEffect.Instance?.Close();
                Time.timeScale = 0f;
            }
        }
    }
    protected override void Start()
    {
        pnlDie.SetActive(false);
    }
    protected override void OnEnable()
    {
        PanelDieEffect.Instance?.Show();
    }
    protected override void OnDisable()
    {
        PanelDieEffect.Instance?.Close();
    }
    //private void OnEnable()
    //{
    //    Time.timeScale = 0f;
    //}
    public async void Restart()
    {
        Time.timeScale = 1f;
        pnlDie.SetActive(false);
        await LevelManager.Instance.LoadLevelAsync(SceneManager.GetActiveScene().buildIndex);
    }
    public async void MainMenu()
    {
        Time.timeScale = 1f;
        pnlDie.SetActive(false);
        await LevelManager.Instance.LoadLevelAsync(0);
    }
    public void LastScore()
    {
        int targetScore = PlayerScoreManager.Instance.totalScore;

        int currentScore = 0;

        DOTween.Kill("ScoreTween");

        DOTween.To(() => currentScore, x => {
            currentScore = x;
            score.text = currentScore.ToString();
        }, targetScore, 1.5f).SetEase(Ease.OutCubic).SetUpdate(true).SetId("ScoreTween");
    }
}
