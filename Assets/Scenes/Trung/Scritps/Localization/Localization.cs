using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Localization")]
public class Localization:ScriptableObject
{
    [Header("Panel ở phía góc trên bên trái màn hình Gameplay")]
    public string TITLE_ENEMIES_LEFT;
    public string TITLE_ENEMY_WAVES;
    public string TITLE_TIME_TO_NEXT_WAVE;

    [Header("Victory Bosss")]
    public string DLG_WHEN_KILL_BOSS = "BOSS IS DEAD!";
}
