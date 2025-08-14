using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorConfig : VuMonoBehaviour
{
    [SerializeField] protected CursorType cursorType = CursorType.Invisible;

    protected static Texture2D currentCursor;
    public static Texture2D CurrentCursor { get => currentCursor; set => currentCursor = value;  }

    protected static Texture2D startCursor;
    public static Texture2D StartCursor { get => startCursor; }
    protected override void OnEnable()
    {
        this.LoadDefaultCursor();
    }

    protected virtual void LoadDefaultCursor()
    {
        if(this.cursorType == CursorType.Invisible)
        {
            Cursor.visible = false;
            currentCursor = null;
            //Cursor.lockState = CursorLockMode.Locked;
        }
        else currentCursor = GetStartCursor(this.cursorType);
        startCursor = currentCursor;
        CursorManager.Instance.SetCurrentCursor(currentCursor);
    }


    public static Texture2D GetStartCursor(CursorType cursorType)
    {
        return cursorType switch
        { 
            CursorType.MainMenu1 => CursorManager.Instance.CursorIconsCtrl.CursorIcons.cursor_MainMenu1,


            CursorType.StoryFight => CursorManager.Instance.CursorIconsCtrl.CursorIcons.cursor_StoryFightIcon,


            CursorType.SurvivalFight => CursorManager.Instance.CursorIconsCtrl.CursorIcons.cursor_SurvivalFightIcon,


            CursorType.SkillSelect1 => CursorManager.Instance.CursorIconsCtrl.CursorIcons.cursor_SkillSelect1,

            _ => null
        };
    }
}

[System.Serializable]
public enum CursorType
{
    Invisible,


    MainMenu1,



    StoryFight,
    SurvivalFight,


    SkillSelect1
}
