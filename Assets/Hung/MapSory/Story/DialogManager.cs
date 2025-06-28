using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    private CanvasGroup canvasGroup;

    public float fadeDuration = 1f;

    private void Start()
    {
        canvasGroup = dialogPanel.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        dialogPanel.SetActive(false);
    }

    public void ShowDialog(string message)
    {
        StopAllCoroutines(); // Dừng các hiệu ứng cũ nếu đang chạy
        dialogPanel.SetActive(true);
        canvasGroup.alpha = 1f;
        dialogText.text = message;

        // Tự tắt sau 3 giây
        CancelInvoke(nameof(StartFadeOut));
        Invoke(nameof(StartFadeOut), 3f);
    }

    private void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        dialogPanel.SetActive(false);
    }
}
