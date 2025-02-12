using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager2 : MonoBehaviour
{
    public static SkillManager2 instance;
    public Skill2[] skills;//class Skill2 
    public SkillButton[] skillButtons;

    public Skill2 activeSkill;//class skill2
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance !=this )
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

}
