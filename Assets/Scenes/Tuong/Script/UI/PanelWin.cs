using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using DG.Tweening;
public class PanelWin : MonoBehaviour
{
    public static PanelWin Instance;
    public GameObject panelWin;
    public TextMeshProUGUI score;
    public TextMeshProUGUI time;
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
    public void OpenPanelWin()
    {
        Time.timeScale = 0f;
        Debug.Log("Mở Panel Win thành công");
        panelWin.SetActive(true);
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            PlayerScoreManager.Instance.SendFinalScore();
        }
        else
        {
            Debug.Log("Chưa đăng nhập PlayFab, không gửi điểm lên hệ thống.");
        }
        LastScore();
        PlayTime();
    }
    public async void Restart()
    {
        Time.timeScale = 1f;
        DOTween.Kill(gameObject);
        panelWin.SetActive(false);
        PlayerScoreManager.Instance?.ResetScore();
        CountDownTimer.Instance.ResetTimer();
        await LevelManager.Instance.LoadLevelAsync(SceneManager.GetActiveScene().buildIndex);
    }
    public async void BackToMainMenu(int index)
    {
        Time.timeScale = 1f;
        DOTween.Kill(gameObject);
        panelWin.SetActive(false);
        PlayerScoreManager.Instance?.ResetScore();
        CountDownTimer.Instance?.ResetTimer();
        await LevelManager.Instance?.LoadLevelAsync(index);
    }
    public void LastScore()
    {
        Debug.Log("Cập nhật điểm cuối cùng");
        int targetScore = PlayerPrefs.GetInt("currentScores", 0);
        score.text = targetScore.ToString();
        Debug.Log("Last Score");
        //if (PlayerScoreManager.Instance != null)
        //{
        //    //int targetScore = PlayerScoreManager.Instance.totalScore;
        //    //int currentScore = 0;

           

        //    //DOTween.To(() => currentScore, x =>
        //    //{
        //    //    currentScore = x;
        //    //    score.text = currentScore.ToString();
        //    //}, targetScore, 1.5f).SetEase(Ease.OutCubic).SetUpdate(true).SetLink(gameObject);
        //}
    }
    public void PlayTime()
    {
        Debug.Log("Cập nhật thời gian chơi");
        string totalSeconds = CountDownTimer.Instance.GetFormattedTime();

        time.text = totalSeconds;
    }
}
