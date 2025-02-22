using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public static CharacterStats instance;
    public LevelUpUI levelUpUI;



    [Header("Base Stats")]
    public int baseHP = 100;
    public int baseAtk = 10;
    public int baseDef = 5;
    public float baseCritRate = 10f;
    public float baseCritDamage = 50f;

    [Header("Current Stats")]
    public int currentHP;
    public int currentAtk;
    public int currentDef;
    public float currentCritRate;
    public float currentCritDamage;

    [Header("Bonus Stats from card")]
    [SerializeField] int bonusFromSkillsHP = 0;
    [SerializeField] private int bonusFromSkillsAtk = 0;
    [SerializeField] private int bonusFromSkillsDef = 0;
    [SerializeField] private float bonusFromSkillsCritRate = 0;

    private Dictionary<string, int> skillLevels = new Dictionary<string, int>(); // Lưu level của từng skill

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LoadPassiveSkillBonuses();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) // Nhấn L để lên level
        {
            LevelUpTest();
            levelUpUI.ShowSkillChoices();
        }
    }

    public void LoadPassiveSkillBonuses()
    {
        currentHP = baseHP + PlayerPrefs.GetInt("BonusHP", 0);
        currentAtk = baseAtk + PlayerPrefs.GetInt("BonusAtk", 0);
        currentDef = baseDef + PlayerPrefs.GetInt("BonusDef", 0);
        currentCritRate = baseCritRate + PlayerPrefs.GetFloat("BonusCritRate", 0);
        currentCritDamage = baseCritDamage + PlayerPrefs.GetFloat("BonusCritDamage", 0);
    }

    public int GetSkillLevel(string skillName)
    {
        return skillLevels.ContainsKey(skillName) ? skillLevels[skillName] : 0;
    }

    public void ApplySkill(SkillCardData skill)
    {
        if (!skillLevels.ContainsKey(skill.skillName))
        {
            skillLevels[skill.skillName] = 1;
        }
        else
        {
            skillLevels[skill.skillName]++;
        }

        int skillLevel = skillLevels[skill.skillName];

        if (skillLevel > 4)
        {
            Debug.Log($"{skill.skillName} đã đạt max level!");
            return;
        }

        int effectValue = skill.effectValues[skillLevel - 1];

        switch (skill.skillName)
        {
            case "BonusDefend":
                bonusFromSkillsDef += effectValue;
                break;
            case "BonusDamage":
                bonusFromSkillsAtk += effectValue;
                break;
            case "BonusHp":
                bonusFromSkillsHP += effectValue;
                break;
            case "BonusCrit":
                bonusFromSkillsCritRate += effectValue;
                break;
        }

        // Cập nhật chỉ số dựa trên cả Passive Skill + Skill từ thẻ
        UpdateCharacterStats();

        Debug.Log($"Skill {skill.skillName} Lv.{skillLevel} → New Stats: Def: {currentDef}, Atk: {currentAtk}, HP: {currentHP}, CritRate: {currentCritRate}%");
    }
    public void UpdateCharacterStats()
    {
        int bonusHP = PlayerPrefs.GetInt("BonusHP", 0);
        int bonusAtk = PlayerPrefs.GetInt("BonusAtk", 0);
        int bonusDef = PlayerPrefs.GetInt("BonusDef", 0);
        float bonusCritRate = PlayerPrefs.GetFloat("BonusCritRate", 0);
        float bonusCritDamage = PlayerPrefs.GetFloat("BonusCritDamage", 0);

        // Cộng chỉ số từ Passive Skill + Skill thẻ
        currentHP = baseHP + bonusHP + bonusFromSkillsHP;
        currentAtk = baseAtk + bonusAtk + bonusFromSkillsAtk;
        currentDef = baseDef + bonusDef + bonusFromSkillsDef;
        currentCritRate = baseCritRate + bonusCritRate + bonusFromSkillsCritRate;
        currentCritDamage = baseCritDamage + bonusCritDamage;

        Debug.Log($"🔹 Cập nhật chỉ số: HP: {currentHP}, Atk: {currentAtk}, Def: {currentDef}, CritRate: {currentCritRate}%");
    }

    public void ApplyBackupSkill()
    {
        Debug.Log("Backup Skill Activated: Tăng5% atk");
        currentHP = Mathf.RoundToInt(currentAtk * 1.05f);
    }

    public void LevelUpTest()
    {
        Debug.Log("Level Up! Choosing a skill...");
        levelUpUI.ShowSkillChoices();
    }

    public void AttackEnemy()
    {
        int damage = DamageCalculator.CalculateDamage(currentAtk, currentCritRate, currentCritDamage);
        Debug.Log($"Gây {damage} sát thương lên quái!");
    }
}
