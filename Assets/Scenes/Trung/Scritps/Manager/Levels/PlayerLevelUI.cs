using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI ProgressText;
    [SerializeField] private Slider expBar;
    [SerializeField] private PlayerLevelSystem playerLevel;

    private void Start()
    {
        playerLevel.OnLevelUp += UpdateLevelText;
        playerLevel.OnExpChanged += UpdateExpBar;
        UpdateLevelText(playerLevel.Level);
        UpdateExpBar(playerLevel.CurrentEXP, playerLevel.EXPToNextLevel);
    }

    private void UpdateLevelText(int level)
    {
        if(this.levelText != null)
        {
            levelText.text = "Level: " + level;
        }
    }


    private void UpdateExpBar(int current, int max)
    {
        if(this.expBar != null)
        {
            this.expBar.maxValue = max;
            this.expBar.value = current;
        }
        if(this.ProgressText != null)
        {
            this.ProgressText.text = current.ToString() + " / " + max.ToString();
        }
    }
}
