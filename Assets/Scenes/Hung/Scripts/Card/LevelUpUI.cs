using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class LevelUpUI : MonoBehaviour
{
    [Header("Animation When Display")]
    //[SerializeField] protected float duration = 0.5f;
    [SerializeField] protected Ease animEaseOn = Ease.OutBack;
    [SerializeField] protected Ease animEaseOff = Ease.OutBack;
    [SerializeField] protected bool isEaseOn = true;
    [Space]
    [Header("SoundFX")]
    [SerializeField] protected List<AudioClip> snd_click;

    [Space]
    public GameObject levelUpPanel;
    public Button[] skillButtons;
    public Image[] skillIcons;
    public TMP_Text[] skillNames;
    public TMP_Text[] skillDescriptions;

    private SkillCardData[] currentSkills = new SkillCardData[3];

    Vector3 defaultBtnScale = Vector3.one;

    [Space]
    [Space]
    [Header("Color Cards Skill Header")]
    [SerializeField] protected List<Image> headerImageSkill;
    [SerializeField] protected List<Image> bodyImageSkill;
    //[SerializeField] protected RectTransform notice;

    [Space]
    [Header("Cursors!")]
    [SerializeField] protected CursorType cursorTypeWhenSelectSkills;

    private void Start()
    {
        levelUpPanel.SetActive(false);
        if(skillButtons.Length > 0) this.defaultBtnScale = skillButtons[0].transform.localScale;
    }

    public void ShowSkillChoices()
    {
        levelUpPanel.SetActive(true);
        CursorManager.Instance.SetCurrentCursor(CursorConfig.GetStartCursor(cursorTypeWhenSelectSkills));
        List<SkillCardData> skillChoices = SkillCardManager.instance.GetRandomSkillChoices();



        if(this.skillButtons.Length > 0)
        {
            this.SetAllSkillButtonState(this.skillButtons,true);
        }


        for (int i = 0; i < 3; i++)
        {
            currentSkills[i] = skillChoices[i];
            skillIcons[i].sprite = skillChoices[i].skillIcon;
            skillNames[i].text = skillChoices[i].skillName;
            headerImageSkill[i].color = skillChoices[i].headerColor;
            bodyImageSkill[i].color=skillChoices[i].bodyColor;
            if (skillChoices[i] == SkillCardManager.instance.backupSkill)
            {
                skillDescriptions[i].text = "Một kỹ năng đặc biệt giúp bạn mạnh hơn!";
            }
            else
            {
                skillDescriptions[i].text = skillChoices[i].levelDescriptions[
                    CharacterStats.Instance.GetSkillLevel(skillChoices[i].skillName)
                ];
            }

            int index = i;
            Button _btn = skillButtons[i];
            _btn.transform.localScale = defaultBtnScale;
            skillButtons[i].gameObject.SetActive(true);
            skillButtons[i].onClick.RemoveAllListeners();
            skillButtons[i].onClick.AddListener(() => SelectSkill(currentSkills[index], _btn));
        }
    }

    public void SelectSkill(SkillCardData skill, Button btn)
    {
        if (skill == SkillCardManager.instance.backupSkill)
        {
            CharacterStats.Instance.ApplyBackupSkill();
        }
        else
        {
            CharacterStats.Instance.ApplySkill(skill);
        }


        CharacterStats.Instance.UpdateCharacterStats();
        CharacterEvents.OnCharacterPropertiesChanged?.Invoke();

        Transform noti = GameObject.Find("NotificationSystemPanelCanvas").transform;
        RectTransform notiRect = noti.GetChild(0).GetComponent<RectTransform>();
        if(notiRect != null) this.Notification(notiRect);

        if (this.skillButtons.Length > 0)
        {
            this.SetAllSkillButtonState(this.skillButtons, false);
        }

        if (!this.isEaseOn)
        {
            levelUpPanel.SetActive(false);
            Time.timeScale = 1f;
            return;
        }
        if(this.snd_click.Count > 0)
        {
            int random = Random.Range(0,this.snd_click.Count);
            if (this.snd_click[random] != null) SoundEnemyManager.Instance.PlayEnemySoundFXClip(this.snd_click[random], this.transform);
        }
        Vector3 btnDefaultScale = btn.transform.localScale;
        RectTransform rectObj = this.levelUpPanel.GetComponent<RectTransform>();
        RectTransform rectBtn = btn.transform.gameObject.GetComponent<RectTransform>();
        rectBtn.localScale = btnDefaultScale;
        Vector3 newVector = btnDefaultScale + Vector3.one;
        rectBtn.DOScale(newVector, 0.3f).SetEase(this.animEaseOn).SetUpdate(true).OnComplete(() =>
        {
            rectBtn.DOScale(Vector3.zero, 0.3f).SetEase(this.animEaseOn).SetUpdate(true).OnComplete(() =>
            {
                rectObj.DOScale(Vector3.zero, 0.1f).SetEase(this.animEaseOff).SetUpdate(true).OnComplete(() =>
                {
                    levelUpPanel.SetActive(false);
                    Time.timeScale = 1f;
                    CursorManager.Instance.SetCurrentCursor(CursorConfig.StartCursor);
                });
            });
        });
        //Time.timeScale = 1; 
    }

    protected virtual void SetAllSkillButtonState(Button[] btns, bool isClick)
    {
        foreach(var button in btns)
        {
            button.GetComponent<Button>().enabled = isClick;
            button.GetComponent<HoverScale3D>().CanHover = isClick;
        }
    }

    protected virtual void Notification(RectTransform notice)
    {
        // Kill tất cả tween đang chạy trên notice
        notice.DOKill();

        // Đảm bảo bắt đầu từ vị trí ẩn
        notice.anchoredPosition = new Vector2(notice.anchoredPosition.x, 700);

        // Tween mới
        notice.DOAnchorPosY(-327, 0.5f).SetEase(Ease.OutQuad).SetUpdate(true)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    notice.DOAnchorPosY(700, 0.5f).SetEase(Ease.InQuad).SetUpdate(true);
                });
            });
    }
}
