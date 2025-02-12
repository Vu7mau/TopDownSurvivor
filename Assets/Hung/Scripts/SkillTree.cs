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

    public List<Skill> SkillList;
    public GameObject SkillHolder;

    public List<GameObject> ConnectionList;
    public GameObject ConnectorHolder;

    public int Skillpoint;
    private void Start()
    {
        Skillpoint = 20;

        SkillLevels = new int[7];
        SkillCaps = new[] { 1, 5, 5, 5, 10, 10,1, };

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
        foreach(var skill in SkillHolder.GetComponentsInChildren<Skill>())
        {
            SkillList.Add(skill);
        }

        foreach (var connector in ConnectorHolder.GetComponentsInChildren<RectTransform>())
        {
            ConnectionList.Add(connector.gameObject);
        }

        for (var i=0; i<SkillList.Count; i++)
        {
            SkillList[i].id = i;
        }

        SkillList[0].ConnectedSkills = new[] { 1, 2, 3, };
        SkillList[2].ConnectedSkills = new[] { 4, 5, 6, };

        UpdateAllSkillUI();
    }
    public void UpdateAllSkillUI()
    {
        foreach(var skill in SkillList)
        {
            skill.UpdateUI();
        }
    }

}
