using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public void DisplayPanelWhenPlayerKillBoss()
    {
        panel1.gameObject.SetActive(true);
        dlgPlayerKillBoss.text = localization.DLG_WHEN_KILL_BOSS;
        StartCoroutine(VictoryRoutine(panel1.gameObject));
    }
    private IEnumerator VictoryRoutine(GameObject obj)
    {
        yield return new WaitForSeconds(3f);
        HideGeneralGameObject(obj);
    }


    public void HideGeneralGameObject(GameObject obj)
    {
        obj.SetActive(false);
    }




    

}
