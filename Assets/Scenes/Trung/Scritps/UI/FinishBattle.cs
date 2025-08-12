using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

[System.Serializable]
public enum StateFinishBattle { Victory, Defeat }
public class FinishBattle : Panel
{
    [SerializeField] protected StateFinishBattle stateFinishBattle;


    [Header("SoundFX")]
    [SerializeField] private AudioClip snd_finish;

    [Space]
    [Header("Text")]
    [SerializeField] protected TMP_Text txt_title;
    [SerializeField] protected TMP_Text txt_level;
    [SerializeField] protected TMP_Text txt_coin;
    [SerializeField] protected TMP_Text txt_kills;
    [SerializeField] protected TMP_Text txt_scores;

    protected override void OnEnable()
    {
        //SoundFXManager.Instance.PlaySoundFXClip(snd_finish,transform);
        base.OnEnable();
        this.ShowBattleResult();
    }

    protected virtual void ShowBattleResult()
    {
        int coins = PlayerPrefs.GetInt("currentCoins", 0);
        int level = PlayerPrefs.GetInt("currentLevel", 0);
        int kills = PlayerPrefs.GetInt("currentKills", 0);
        int scores = PlayerPrefs.GetInt("currentScores", 0);
        bool isWin = stateFinishBattle == StateFinishBattle.Victory? true: false;
        this.DisplayPanelWhenFinishTheBattle(isWin,level, scores,kills, coins);
    }



    public override void AnimationDisplayOnFX()
    {
        Time.timeScale = 0f;
        if (!this.isEaseOn) return;
        RectTransform rectObj = this.GetComponent<RectTransform>();
        rectObj.localScale = Vector3.zero;
        rectObj.DOScale(Vector3.one, this.duration).SetEase(this.animEaseOn).SetUpdate(true);
    }
    public override void AnimationDisplayOffFX()
    {
    }

    public virtual void DisplayPanelWhenFinishTheBattle(bool isVictory, int level, int scores, int kills, int coins)
    {
        if (this.txt_level != null)
        {
            string title = isVictory ? "VICTORY!" : "DEFEAT!";
            this.txt_title.text = title.ToString();
        }
        if (this.txt_level != null) this.txt_level.text = level.ToString();
        if (this.txt_scores != null) this.txt_scores.text = scores.ToString();
        if (this.txt_kills != null) this.txt_kills.text = kills.ToString();
        if (this.txt_coin != null) this.txt_coin.text = coins.ToString();
    }
}
