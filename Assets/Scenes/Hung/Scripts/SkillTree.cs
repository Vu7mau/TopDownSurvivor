using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public static SkillTree skillTree;
    private void Awake()
    {
        skillTree = this;
    }

    public int[] SkillLevels;
    public int[] SkillCaps;
    public string[] SkillNames;
    public string[] SkillDescriptions;

    public List<SkillCard> SkillList;
    public GameObject SkillHolder;

    public List<GameObject> ConnectionList;
    public GameObject ConnectorHolder;

    public int Skillpoint;
    private void Start()
    {
        // Load lại Skillpoint đã lưu neu chua luu thi mac dinh la 20
        Skillpoint = PlayerPrefs.GetInt("Skillpoint", 20);

        SkillLevels = new int[7];
        SkillCaps = new[] { 1, 5, 5, 5, 10, 10, 1, };

        SkillNames = new[] { "Upgrade 1", "Upgrade 2", "Upgrade 3", "Upgrade 4", "Upgrade 5", "Upgrade 6", "Upgrade 7" };
        SkillDescriptions = new[]
        {
            "All attribute +5",
            "HP +5/lv",
            "Atk +5/lv",
            "Defend +5/lv",
            "Crit rate +5/lv",
            "Crit damage +20%/lv",
            "Atk +1000",
        };
        foreach (var skill in SkillHolder.GetComponentsInChildren<SkillCard>())
        {
            SkillList.Add(skill);
        }

        foreach (var connector in ConnectorHolder.GetComponentsInChildren<RectTransform>())
        {
            ConnectionList.Add(connector.gameObject);
        }

        for (var i = 0; i < SkillList.Count; i++)
        {
            SkillList[i].id = i;
            // Load cấp độ kỹ năng từ PlayerPrefs
            SkillLevels[i] = PlayerPrefs.GetInt($"SkillLevel_{i}", 0);
        }

        SkillList[0].ConnectedSkills = new[] { 1, 2, 3, };
        SkillList[2].ConnectedSkills = new[] { 4, 5, 6, };

        UpdateAllSkillUI();
    }
    public void UpdateAllSkillUI()
    {
        foreach (var skill in SkillList)
        {
            skill.UpdateUI();
        }
    }

    public void ResetSkillTree()
    {
        int skillCount = SkillCaps.Length; // Lấy số lượng kỹ năng từ SkillCaps

        // Xóa tất cả kỹ năng đã lưu
        for (int i = 0; i < skillCount; i++)
        {
            PlayerPrefs.DeleteKey($"SkillLevel_{i}");
        }
        PlayerPrefs.DeleteKey("Skillpoint");
        PlayerPrefs.Save();

        // Đặt lại Skillpoint và SkillLevels
        Skillpoint = 20;
        SkillLevels = new int[skillCount]; // Reset tất cả kỹ năng về 0

        // Cập nhật UI sau khi reset
        UpdateAllSkillUI();
        PlayerPrefs.SetInt("BonusHP", skillTree.GetBonusHP());
        PlayerPrefs.SetInt("BonusAtk", skillTree.GetBonusAtk());
        PlayerPrefs.SetInt("BonusDef", skillTree.GetBonusDef());
        PlayerPrefs.SetFloat("BonusCritRate", skillTree.GetBonusCritRate());
        PlayerPrefs.SetFloat("BonusCritDamage", skillTree.GetBonusCritDamage());


    }

    public int GetBonusHP()
    {
        return SkillLevels[1] * 5; // Upgrade 2: HP +5 mỗi cấp
    }

    public int GetBonusAtk()
    {
        return SkillLevels[2] * 5 + (SkillLevels[6] > 0 ? 1000 : 0); // Upgrade 3: Atk +5/lv, Upgrade 7: Atk +1000
    }

    public int GetBonusDef()
    {
        return SkillLevels[3] * 5; // Upgrade 4: Def +5/lv
    }

    public float GetBonusCritRate()
    {
        return SkillLevels[4] * 5; // Upgrade 5: Crit rate +5% mỗi cấp
    }

    public float GetBonusCritDamage()
    {
        return SkillLevels[5] * 20; // Upgrade 6: Crit damage +20% mỗi cấp
    }
}
