using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill System/Skill")]
public class SkillCardData : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    public string[] levelDescriptions = new string[4];
    public int[] effectValues = new int[4];

    [Space]
    [Header("Color for cards skill!")]

    [Space]
    [Space]
    public Color headerColor;
    public Color bodyColor;
}
