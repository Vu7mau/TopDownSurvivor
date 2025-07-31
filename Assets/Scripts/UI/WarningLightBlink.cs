using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningLightBlink : VuMonoBehaviour
{
    private Light warningLight;

    [Header("Cài đặt nhấp nháy")]
    [SerializeField] private float blinkSpeed = 2f;
    [SerializeField] private float blinkDuration = 3f;
    [SerializeField] private Color onColor = Color.red;
    [SerializeField] private Color offColor = Color.black;

    private Coroutine blinkCoroutine;

    protected override void Start()
    {
       

    }
    //private void Start()
    //{
    //  
    //    StartBlinking(); // auto start nếu muốn
    //}

    protected override void OnEnable()
    {
       warningLight = GetComponent<Light>();
        if (warningLight == null)
        {
            Debug.LogWarning("Không tìm thấy Light component!");
            //   enabled = false;
        }
        else { warningLight.enabled = false; }
    }

    public void StartBlinking(float duration = -1f)
    {
        warningLight.enabled = true;
        if (duration > 0) blinkDuration = duration;

        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        float timer = 0f;
       // warningLight.enabled = true;
        while (timer < blinkDuration)
        {
            float t = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            warningLight.color = Color.Lerp(offColor, onColor, t);
            timer += Time.deltaTime;
            yield return null; 
        }

        // Kết thúc: chuyển sang trắng
        warningLight.color = Color.white;
        blinkCoroutine = null;
    }
}
