using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill System/Skill")]
public class SkillCardData : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    public string[] levelDescriptions = new string[4];
    public int[] effectValues = new int[4];
}
