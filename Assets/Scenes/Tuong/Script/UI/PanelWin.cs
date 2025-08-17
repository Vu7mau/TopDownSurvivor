using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using DG.Tweening;
public class PanelWin : MonoBehaviour
{
    public GameObject panelWin;
    public TextMeshProUGUI score;
    public TextMeshProUGUI time;
    private void Start()
    {
        panelWin.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (panelWin.activeSelf)
            {
                panelWin.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                OpenPanelWin();
            }
        }
    }
    public void OpenPanelWin()
    {
        panelWin.SetActive(true);
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
        Time.timeScale = 0f;
    }
    public async void Restart()
    {
        panelWin.SetActive(false);
        PlayerScoreManager.Instance.ResetScore();
        CountDownTimer.Instance.ResetTimer();
        await LevelManager.Instance.LoadLevelAsync(SceneManager.GetActiveScene().buildIndex);
    }
    public async void BackToMainMenu()
    {
        panelWin.SetActive(false);
        PlayerScoreManager.Instance?.ResetScore();
        CountDownTimer.Instance?.ResetTimer();
        await LevelManager.Instance?.LoadLevelAsync(0);
    }
    public void LastScore()
    {
        int targetScore = PlayerScoreManager.Instance.totalScore; 

        int currentScore = 0;

        DOTween.Kill("ScoreTween");

        DOTween.To(() => currentScore, x => {
            currentScore = x;
            score.text = "Số điểm: " + currentScore.ToString();
        }, targetScore, 1.5f).SetEase(Ease.OutCubic).SetUpdate(true).SetId("ScoreTween");
    }

    public void PlayTime()
    {
        int totalSeconds = PlayerPrefs.GetInt("LastPlayTime", 0);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        time.text = $"Thời gian chơi: {minutes:D2}:{seconds:D2}";
    }
}
