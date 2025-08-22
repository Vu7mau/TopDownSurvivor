using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : Singleton<UIManager>
{
    [Header("Localization Reference")]
    [SerializeField] private Localization localization;
    [SerializeField] private Timer timer;

    [Header("Panel on the top Gameplay Screen!")]
    [SerializeField] private TextMeshProUGUI titleEnemiesLeft;
    [SerializeField] private TextMeshProUGUI txtEnemiesLeft;
    [SerializeField] private TextMeshProUGUI titleEnemiesWave;
    [SerializeField] private TextMeshProUGUI txtEnemiesWave;
    [SerializeField] private TextMeshProUGUI titleTimeToNextWave;
    [SerializeField] private TextMeshProUGUI txtTimeToNextWave;

    [Space]
    [Space]
    [Header("Panel on the top Gameplay Screen!")]
    [SerializeField] private TextMeshProUGUI title_ATK;
    [SerializeField] private TextMeshProUGUI txt_ATK;
    [SerializeField] private TextMeshProUGUI title_Defense;
    [SerializeField] private TextMeshProUGUI txt_Defense;
    [SerializeField] private TextMeshProUGUI title_CritRate;
    [SerializeField] private TextMeshProUGUI txt_CritRate;
    [SerializeField] private TextMeshProUGUI title_CritDamage;
    [SerializeField] private TextMeshProUGUI txt_CritDamage;
    [SerializeField] private TextMeshProUGUI title_BonusDamage;
    [SerializeField] private TextMeshProUGUI txt_BonusDamage;

    [Space]
    [Space]
    [Header("Panel when playerPosition kill boss!")]
    [SerializeField] private TextMeshProUGUI dlgPlayerKillBoss;

    
    [SerializeField] private GameObject panel1;

    protected override void OnEnable()
    {
    }
    protected override void OnDisable()
    {
    }
    protected override void Start()
    {
        base.Start();
        HideGeneralGameObject(panel1);
    }
    public void UpdateWaveUI(int _wave,int _amountEnemiesLeft)
    {
        titleEnemiesLeft.text = localization.TITLE_ENEMIES_LEFT;
        txtEnemiesLeft.text = _amountEnemiesLeft.ToString();
        titleEnemiesWave.text = localization.TITLE_ENEMY_WAVES;
        txtEnemiesWave.text = _wave.ToString();
    }
    public void UpdateTimeToNextWave(float _time)
    {
        titleTimeToNextWave.text = localization.TITLE_TIME_TO_NEXT_WAVE;
        timer.StartCountDown(true, true, _time);
    }

    public void UpdateCharacterStatsUI(float _atk, float _defense, float _critRate, float _critDamage/*, float _bonusDamage*/)
    {
        if(this.txt_ATK != null)
        {
            this.title_ATK.text = localization.TITLE_ATK;
            this.txt_ATK.text = _atk.ToString();
        }
        if (this.txt_Defense != null)
        {
            this.title_Defense.text = localization.TITLE_DEFENSE;
            this.txt_Defense.text = _defense.ToString();
        }
        if (this.txt_CritRate != null)
        {
            this.title_CritRate.text = localization.TITLE_CRITRATE;
            this.txt_CritRate.text = _critRate.ToString();
        }
        if (this.txt_CritDamage != null)
        {
            this.title_CritDamage.text = localization.TITLE_CRITDAMAGE;
            this.txt_CritDamage.text = _critDamage.ToString();
        }
        //if (this.txt_BonusDamage != null)
        //{
        //    this.title_BonusDamage.text = localization.TITLE_BONUSDAMAGE;
        //    this.txt_BonusDamage.text = (_bonusDamage + _atk).ToString();
        //}
    }

    public void UpdateATKUI(float _atk)
    {
        if (this.txt_ATK != null)
        {
            this.title_ATK.text = localization.TITLE_ATK;
            this.txt_ATK.text = _atk.ToString();
        }
    }


    public void DisplayPanelWhenPlayerKillBoss()
    {
        panel1.gameObject.SetActive(true);
        dlgPlayerKillBoss.text = localization.DLG_WHEN_KILL_BOSS;
    }

    public void HideGeneralGameObject(GameObject obj)
    {
        obj.SetActive(false);
    }




    

}
