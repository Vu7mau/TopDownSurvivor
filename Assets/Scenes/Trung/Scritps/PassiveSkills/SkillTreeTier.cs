using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewSkillTreeTier", menuName = "Skill/Skill Tree Tier")]
public class SkillTreeTier : ScriptableObject
{
    public PassiveSkillData[] availableSkills; // Các skill có thể xuất hiện trong tier này
    public bool randomizeSkills = false; // Có random khi xuất hiện không
}
