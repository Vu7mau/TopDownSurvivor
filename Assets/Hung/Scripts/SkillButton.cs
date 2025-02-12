using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButton : MonoBehaviour
{
    public Image skillImage;
    public TMP_Text skillNameText;
    public TMP_Text skillDesText;

    public int skillButtonId;

    public void PressSkillButton()
    {
        SkillManager2.instance.activeSkill = transform.GetComponent<Skill2>();


        skillImage.sprite = SkillManager2.instance.skills[skillButtonId].skillSprite;
        skillNameText.text = SkillManager2.instance.skills[skillButtonId].skillName;
        skillDesText.text = SkillManager2.instance.skills[skillButtonId].skillDes;
    }

}
