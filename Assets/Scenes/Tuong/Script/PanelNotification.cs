using UnityEngine;
using System.Collections;
public class PanelNotification : MonoBehaviour
{
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private GameObject loadingPanel;
    public void OnRetryClicked()
    {
        notificationPanel.SetActive(false);
        loadingPanel.SetActive(true);

        StartCoroutine(RetryConnection());
    }
    private IEnumerator RetryConnection()
    {
        float timer = 0f;
        float maxWaitTime = 30f; 

        while (timer < maxWaitTime)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                Debug.Log("Có mạng trở lại!");
                loadingPanel.SetActive(false);

                MainMenuTwo.Instance.PlayMenu.SetActive(true);
                MainMenuTwo.Instance.IconLeaderBoard.SetActive(true);
                MainMenuTwo.Instance.IconGame.SetActive(true);

                yield break; 
            }

            timer += Time.deltaTime;
            yield return null; 
        }

        Debug.LogWarning("Không có mạng sau khi thử 30s.");
        loadingPanel.SetActive(false);
        notificationPanel.SetActive(true);
    }
    public void ExitPanel()
    {
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
            MainMenuTwo.Instance.PlayMenu.SetActive(true);
            MainMenuTwo.Instance.IconGame.SetActive(true);
            MainMenuTwo.Instance.IconLeaderBoard.SetActive(true);
        }
    }
}
