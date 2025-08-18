using DG.Tweening;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PanelDie : Singleton<VuMonoBehaviour>
{
    public GameObject pnlDie;
    public TextMeshProUGUI score;
    public TextMeshProUGUI time;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if (pnlDie.activeSelf)
            {
                pnlDie.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                OpenPanelDie(); 
                Time.timeScale = 0f; 
            }
        }
    }
    public void OpenPanelDie()
    {
        pnlDie.SetActive(true);
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            PlayerScoreManager.Instance.SendFinalScore();
        }
        else
        {
            Debug.Log("Chưa đăng nhập PlayFab, không gửi điểm lên hệ thống.");
        }
        CountDownTimer.Instance.GetSessionDurationInSeconds();
        LastScore();
        PlayTime();
    }
    public async void Restart()
    {
        Time.timeScale = 1f;
        DOTween.Kill(gameObject);
        pnlDie.SetActive(false);
        PlayerScoreManager.Instance.ResetScore();
        CountDownTimer.Instance.ResetTimer();
        await LevelManager.Instance.LoadLevelAsync(SceneManager.GetActiveScene().buildIndex);
    }
    protected override void Start()
    {
        pnlDie.SetActive(false);
    }
    public async void BackToMainMenu(int index)
    {
        Time.timeScale = 1f;
        DOTween.Kill(gameObject);
        pnlDie.SetActive(false);
        PlayerScoreManager.Instance?.ResetScore();
        CountDownTimer.Instance?.ResetTimer();
        await LevelManager.Instance?.LoadLevelAsync(index);
    }
    public void LastScore()
    {
        int targetScore = PlayerScoreManager.Instance.totalScore;

        int currentScore = 0;

        DOTween.To(() => currentScore, x =>
        {
            currentScore = x;
            score.text = currentScore.ToString();
        }, targetScore, 1.5f).SetEase(Ease.OutCubic).SetUpdate(true).SetLink(gameObject);
    }
    public void PlayTime()
    {
        int totalSeconds = CountDownTimer.Instance.GetSessionDurationInSeconds();
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        time.text = $"{minutes:D2}:{seconds:D2}";
    }
}
