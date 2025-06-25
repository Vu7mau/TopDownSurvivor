using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : VuMonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TMP_Text textValue;
    [SerializeField] private float displayedFillAmount;
    [SerializeField] private float displayedHealth;
    [SerializeField] private float smoothDuration = 0.75f;

    [Space]
    [Header("Color")]
    [SerializeField] private Color health100_Percent;
    [SerializeField] private Color health50_Percent;
    [SerializeField] private Color health25_Percent;

    private Coroutine smoothRoutine;
    protected override void LoadComponents()
    {
        this.Load_HealthBarFill();
        this.Load_TextValue();
    }
    private void Load_HealthBarFill()
    {
        if (healthBarFill != null) return;

        healthBarFill = this.transform.Find("Round bar 1 - fill").GetComponent<Image>();
    }
    private void Load_TextValue()
    {
        if (textValue != null) return;

        textValue = GetComponentInChildren<TMP_Text>();
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        //float currentHealth = Mathf.Clamp(health, 0, maxHealth);
        // healthBarFill.fillAmount = currentHealth / maxHealth;

        if (smoothRoutine != null)
            StopCoroutine(smoothRoutine);

        smoothRoutine = StartCoroutine(SmoothHealth(currentHealth, maxHealth));
    }
    private IEnumerator SmoothHealth(float targetHp, float maxHp)
    {
   
        float elapsed = 0f;

        float startFill = healthBarFill.fillAmount;
        float targetFill = Mathf.Clamp01(targetHp / maxHp);

        float startValue = GetCurrentDisplayValue(); // Lấy số hiện tại đang hiển thị
        float targetValue = targetHp;

        while (elapsed < smoothDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / smoothDuration);

            float newFill = Mathf.Lerp(startFill, targetFill, t);
            healthBarFill.fillAmount = newFill;
            this.UpdateHealthColor(targetHp, maxHp);
            float displayVal = Mathf.Lerp(startValue, targetValue, t);
            textValue.text = Mathf.CeilToInt(displayVal).ToString();

            yield return null;
        }

        // Đảm bảo chính xác 100%
        healthBarFill.fillAmount = targetFill;
        textValue.text = Mathf.RoundToInt(targetHp).ToString();

    }
    private void UpdateHealthColor(float currentHp, float maxHp)
    {
        float healthPercent = currentHp / maxHp;

    

        if (healthPercent > 0.5f)
        {
            // 50% → 100%: từ vàng đến xanh
            float t = (healthPercent - 0.5f) / 0.5f;
            healthBarFill.color = Color.Lerp(health50_Percent, health100_Percent, t);
        }
        else
        {
            // 0% → 50%: từ đỏ đến vàng
            float t = healthPercent / 0.5f;
            healthBarFill.color = Color.Lerp(health25_Percent, health50_Percent, t);
        }
    }

    private float GetCurrentDisplayValue()
    {
        int currentVal;
        if (int.TryParse(textValue.text, out currentVal))
            return currentVal;
        else
            return 0f;
    }

}
