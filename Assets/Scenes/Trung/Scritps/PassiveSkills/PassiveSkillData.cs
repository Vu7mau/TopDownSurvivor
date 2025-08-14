using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassiveSkill", menuName = "Skill/PassiveSkill")]
public class PassiveSkillData : ScriptableObject
{
    public string skillName;
    [TextArea] public string description;
    public Sprite icon;

    public int price;        // Giá coin mua skill
    public int requiredLevel; // Level yêu cầu mở skill
    public bool isStackable;  // Có cộng dồn không
}
