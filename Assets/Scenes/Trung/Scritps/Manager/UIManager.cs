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

    [Header("Panel when player kill boss!")]
    [SerializeField] private TextMeshProUGUI dlgPlayerKillBoss;

    [Header("List of Text Aniamtion")]
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private GameObject coinTextPrefab;
    [SerializeField] private GameObject ExpTextPrefab;

    [SerializeField] private Canvas _myCanvas;
    [SerializeField] private Canvas _DamageTextCanvas;
    [SerializeField] private GameObject panel1;

    protected override void OnEnable()
    {
        base.OnEnable();
        CharacterEvents.characterDamaged += CharacterTookDamage;
        //CharacterEvents.characterTookItem += CharacterTookItem;
        //CharacterEvents.characterTookExp += CharacterTookExp;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        CharacterEvents.characterDamaged -= CharacterTookDamage;
        //CharacterEvents.characterTookItem -= CharacterTookItem;
        //CharacterEvents.characterTookExp -= CharacterTookExp;
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




    public void CharacterTookDamage(GameObject character, float damageReceived)
    {
        Vector3 spamPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmp_text = Instantiate(damageTextPrefab, spamPosition, Quaternion.identity, _DamageTextCanvas.transform)
            .GetComponent<TMP_Text>();
        tmp_text.text = damageReceived.ToString();
        Debug.Log("Text damage đã hiện!");
    }

}
