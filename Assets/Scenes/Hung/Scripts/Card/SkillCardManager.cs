using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCardManager : MonoBehaviour
{
    public static SkillCardManager instance;

    public List<SkillCardData> skillPool; // Danh sách tất cả các kỹ năng
    public SkillCardData backupSkill; // Skill Card Phụ

    private void Awake()
    {
        instance = this;
    }

    public List<SkillCardData> GetRandomSkillChoices()
    {
        List<SkillCardData> availableSkills = new List<SkillCardData>();

        foreach (var skill in skillPool)
        {
            if (CharacterStats.Instance.GetSkillLevel(skill.skillName) < 4)
            {
                availableSkills.Add(skill);
            }
        }

        List<SkillCardData> chosenSkills = new List<SkillCardData>();

        while (chosenSkills.Count < 3)
        {
            if (availableSkills.Count > 0)
            {
                int randomIndex = Random.Range(0, availableSkills.Count);
                chosenSkills.Add(availableSkills[randomIndex]);
                availableSkills.RemoveAt(randomIndex);
            }
            else
            {
                chosenSkills.Add(backupSkill);
            }
        }

        return chosenSkills;
    }
}
