using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class ChangeScore : MonoBehaviour
{
    [SerializeField] private LocalizedString localStringScore;
    [SerializeField] private TextMeshProUGUI textComp;
    private int Score;
    private void OnEnable()
    {
        localStringScore.Arguments = new object[] { Score };
        localStringScore.StringChanged += UpdateText;
    }
    private void OnDisable()
    {
        localStringScore.StringChanged -= UpdateText;
    }
    private void UpdateText(string value)
    {
        textComp.text = value;
    }
    public void IncreaseScore()
    {
        Score++;
        localStringScore.Arguments = new object[] { Score };
        localStringScore.RefreshString();
    }
}
