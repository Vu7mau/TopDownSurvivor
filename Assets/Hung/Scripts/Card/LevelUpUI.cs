using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public GameObject levelUpPanel;
    public Button[] skillButtons;
    public Image[] skillIcons;
    public TMP_Text[] skillNames;
    public TMP_Text[] skillDescriptions;

    private SkillCardData[] currentSkills = new SkillCardData[3];

    private void Start()
    {
        levelUpPanel.SetActive(false);
    }

    public void ShowSkillChoices()
    {
        levelUpPanel.SetActive(true);
        List<SkillCardData> skillChoices = SkillCardManager.instance.GetRandomSkillChoices();

        for (int i = 0; i < 3; i++)
        {
            currentSkills[i] = skillChoices[i];
            skillIcons[i].sprite = skillChoices[i].skillIcon;
            skillNames[i].text = skillChoices[i].skillName;

            if (skillChoices[i] == SkillCardManager.instance.backupSkill)
            {
                skillDescriptions[i].text = "Một kỹ năng đặc biệt giúp bạn mạnh hơn!";
            }
            else
            {
                skillDescriptions[i].text = skillChoices[i].levelDescriptions[
                    CharacterStats.instance.GetSkillLevel(skillChoices[i].skillName)
                ];
            }

            int index = i;
            skillButtons[i].gameObject.SetActive(true);
            skillButtons[i].onClick.RemoveAllListeners();
            skillButtons[i].onClick.AddListener(() => SelectSkill(currentSkills[index]));
        }
    }

    public void SelectSkill(SkillCardData skill)
    {
        if (skill == SkillCardManager.instance.backupSkill)
        {
            CharacterStats.instance.ApplyBackupSkill();
        }
        else
        {
            CharacterStats.instance.ApplySkill(skill);
        }

        CharacterStats.instance.UpdateCharacterStats();

        levelUpPanel.SetActive(false);
    }
}
