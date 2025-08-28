using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveSkill", menuName = "Skill/PassiveSkill")]
public class PassiveSkillSO : ScriptableObject
{
    public string skillName;
    [TextArea] public string description;


    [System.Serializable]
    public class SkillLevel
    {
        public int cost;      // coin để mở cấp này
        public float value;   // giá trị buff (VD: +10 Damage, +5% Crit...)
    }

    public SkillLevel[] levels;

    [Header("Unlock Next Skill")]
    public PassiveSkillSO nextSkill;   // skill tiếp theo sẽ mở
    public int unlockAtLevel = 1;      // cấp bao nhiêu thì mở (mặc định 1)

    [Header("UI")]
    public Sprite icon; // icon riêng cho skill
}
