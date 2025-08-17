using UnityEngine;
using TMPro;
using System.Collections;

public class LoadingDotEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingText;
    private string baseText = "Đang kết nối lại";
    private bool isRunning = true;

    private void OnEnable()
    {
        isRunning = true;
        StartCoroutine(DotAnimation());
    }

    private void OnDisable()
    {
        isRunning = false;
    }

    private IEnumerator DotAnimation()
    {
        int dotCount = 0;
        while (isRunning)
        {
            loadingText.text = baseText + new string('.', dotCount);
            dotCount = (dotCount + 1) % 4; 
            yield return new WaitForSeconds(0.5f);
        }
    }
}
