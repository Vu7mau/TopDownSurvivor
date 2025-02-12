using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static SkillTree;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public int id;

    public TMP_Text TilteText;
    public TMP_Text DescriptionText;

    public int[] ConnectedSkills;

    public int[] ConnectedUpgrade;

    public void UpdateUI()
    {
        TilteText.text = $"{skillTree.SkillLevels[id]} / {skillTree.SkillCaps[id]} \n{skillTree.SkillNames[id]}";
        DescriptionText.text = $"{skillTree.SkillDescriptions[id]}\nCost{skillTree.Skillpoint}/1 SP";

        GetComponent<Image>().color = skillTree.SkillLevels[id] >= skillTree.SkillCaps[id] ? Color.yellow : skillTree.Skillpoint > 0 ? Color.green : Color.white ;

        foreach (var connectedSkill in ConnectedSkills)
        {
            skillTree.SkillList[connectedSkill].gameObject.SetActive(skillTree.SkillLevels[id] > 0);
            skillTree.ConnectionList[connectedSkill].SetActive(skillTree.SkillLevels[id] > 0 );
        }
    }

    public void Buy()
    {
        if(skillTree.Skillpoint<1 || skillTree.SkillLevels[id] >= skillTree.SkillCaps[id])
        {
            return;
        }
        skillTree.Skillpoint -= 1;
        skillTree.SkillLevels[id]++;
        skillTree.UpdateAllSkillUI();
    }
}
