using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CursorIcons",menuName = "Cursor/ListCursors")]

public class CursorIcons : ScriptableObject
{
    [Header("List of Cursor's Icon")]

    [Space(20)]
    [Header("<----------- Main Menu ----------->")]


    public Texture2D cursor_MainMenu1;




    [Space(20)]
    [Header("<----------- For Survival ---------->")]
    public Texture2D cursor_SurvivalFightIcon;
    public Texture2D cursor_SkillSelect1;



    [Space(20)]
    [Header("<----------- For Story ------------>")]
    public Texture2D cursor_StoryFightIcon;
}
