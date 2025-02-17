using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    [SerializeField] private float timeDeleted;
    private Vector3 moveSpeed = new Vector3(0, 100, 0);
    [SerializeField] private float timeToFade;
    private float timeElapsed = 0f;
    RectTransform textHealth;
    TextMeshProUGUI textDamage;
    public Color startColor;
    private void Awake()
    {
        textHealth = GetComponent<RectTransform>();
        textDamage= GetComponent<TextMeshProUGUI>();
        textDamage.color = startColor;
    }
    private void Update()
    {
        textHealth.position += moveSpeed * Time.deltaTime;
        timeElapsed+=Time.deltaTime;
        if (timeElapsed < timeToFade)
        {
            float fadeAlpha=startColor.a*(1- (timeElapsed/timeToFade));
            textDamage.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }
        else
        {
            Destroy(gameObject, timeDeleted);
        }
    }
}
