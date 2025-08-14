using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance;

    public List<SkillTreeTier> tiers; // Danh sách các tầng
    public int playerLevel;
    public int playerCoins;

    // Skill đã sở hữu + level stack
    private Dictionary<PassiveSkillData, int> ownedSkills = new Dictionary<PassiveSkillData, int>();

    // Tier hiện tại đã unlock tới đâu
    private int unlockedTierIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Mặc định skill đầu tiên
        if (tiers.Count > 0 && tiers[0].availableSkills.Length > 0)
        {
            var firstSkill = tiers[0].availableSkills[0];
            AddSkill(firstSkill);
        }
    }

    public bool CanUnlockTier(int tierIndex)
    {
        if (tierIndex < 0 || tierIndex >= tiers.Count) return false;
        return playerLevel >= GetLowestRequiredLevel(tiers[tierIndex]);
    }

    private int GetLowestRequiredLevel(SkillTreeTier tier)
    {
        int minLevel = int.MaxValue;
        foreach (var skill in tier.availableSkills)
        {
            if (skill.requiredLevel < minLevel)
                minLevel = skill.requiredLevel;
        }
        return minLevel;
    }

    public bool BuySkill(PassiveSkillData skill)
    {
        if (playerCoins < skill.price) return false;
        //if (playerLevel < skill.requiredLevel) return false;

        playerCoins -= skill.price;
        AddSkill(skill);
        return true;
    }

    private void AddSkill(PassiveSkillData skill)
    {
        if (ownedSkills.ContainsKey(skill))
        {
            if (skill.isStackable)
                ownedSkills[skill]++;
        }
        else
        {
            ownedSkills.Add(skill, 1);
        }
    }

    public int GetSkillStack(PassiveSkillData skill)
    {
        return ownedSkills.TryGetValue(skill, out int stack) ? stack : 0;
    }
}
