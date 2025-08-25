using DG.Tweening;
using PlayFab;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PanelDie : Singleton<VuMonoBehaviour>
{
    public GameObject pnlDie;
    public TextMeshProUGUI score;
    public TextMeshProUGUI time;
    public async void OpenPanelDie()
    {
        Debug.Log("Open Panel Die");
        await Task.Delay(3000);
        Time.timeScale = 0f;
        pnlDie.SetActive(true);
        //if (PlayFabClientAPI.IsClientLoggedIn())
        //{
        //    PlayerScoreManager.Instance.SendFinalScore();
        //}
        //else
        //{
        //    Debug.Log("Chưa đăng nhập PlayFab, không gửi điểm lên hệ thống.");
        //}
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
        pnlDie?.SetActive(false);
        PlayerScoreManager.Instance?.ResetScore();
        CountDownTimer.Instance?.ResetTimer();
        await LevelManager.Instance?.LoadLevelAsync(index);
    }
    public void LastScore()
    {
        //if (PlayerScoreManager.Instance != null)
        //{

        //}


        int targetScore = PlayerPrefs.GetInt("currentScores", 0);
        Debug.Log("Last Score");

        score.text = targetScore.ToString();
        int currentScore = 0;

        //DOTween.To(() => currentScore, x =>
        //{
        //    currentScore = x;
        //    score.text = currentScore.ToString();
        //}, targetScore, 1.5f).SetEase(Ease.OutCubic).SetUpdate(true).SetLink(gameObject);

    }
    public void PlayTime()
    {
        string totalSeconds = CountDownTimer.Instance?.GetFormattedTime();

        time.text = totalSeconds;
    }
}
