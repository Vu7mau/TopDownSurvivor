using System.Collections;
using UnityEngine;

public class WarningLightBlink : VuMonoBehaviour
{
    private Light warningLight;

    [Header("Cài đặt nhấp nháy")]
    [SerializeField] private float blinkSpeed = 2f;
    [SerializeField] private float blinkDuration = 3f;
    [SerializeField] private Color onColor = Color.red;
    [SerializeField] private Color offColor = Color.black;

    [Header("Khi kết thúc")]
    [Tooltip("Màu cuối cùng khi nhấp nháy kết thúc.")]
    [SerializeField] private Color finishColor = Color.white;
    [Tooltip("Tắt Light hẳn sau khi kết thúc? Nếu false thì giữ màu finishColor.")]
    [SerializeField] private bool disableLightOnFinish = false;

    private Coroutine blinkCoroutine;

    protected override void OnEnable()
    {
        warningLight = GetComponent<Light>();
        if (warningLight == null)
        {
            Debug.LogWarning("Không tìm thấy Light component!");
        }
        else
        {
            warningLight.enabled = false;
        }
    }

    /// <summary>
    /// Bắt đầu nhấp nháy. Nếu duration > 0 thì override blinkDuration.
    /// </summary>
    public void StartBlinking(float duration = -1f)
    {
        if (warningLight == null) return;

        warningLight.enabled = true;
        if (duration > 0) blinkDuration = duration;

        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        float timer = 0f;
        while (timer < blinkDuration)
        {
            float t = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            warningLight.color = Color.Lerp(offColor, onColor, t);
            timer += Time.deltaTime;
            yield return null;
        }

        // Kết thúc
        if (disableLightOnFinish)
        {
            warningLight.enabled = false;
        }
        else
        {
            warningLight.color = finishColor;
        }

        blinkCoroutine = null;
    }
}
