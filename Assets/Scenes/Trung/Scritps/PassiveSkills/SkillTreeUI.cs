using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUI : MonoBehaviour
{
    public Transform skillTreeContainer;     // Nơi chứa các tree
    public GameObject treePrefab;            // Prefab 1 tree (bên trong có 1-2 skill slot)
    public GameObject skillButtonPrefab;     // Prefab button skill

    public int currentTierIndex = 0;

    private void OnEnable()
    {
        // Xóa toàn bộ tree cũ
        foreach (Transform child in skillTreeContainer)
            Destroy(child.gameObject);
        for (int i = 0; i < currentTierIndex + 1; i++)
        {
            this.DisplayTier(i);
        }
    }

    public void DisplayTier(int tierIndex)
    {

        var tier = SkillTreeManager.Instance.tiers[tierIndex];
        //var skills = tier.randomizeSkills
        //    ? ShuffleSkills(tier.availableSkills)
        //    : tier.availableSkills;
        var skills = tier.availableSkills;
        // Tạo tree mới
        var treeObj = Instantiate(treePrefab, skillTreeContainer);
        // Nhóm skill thành từng "tree" 1 hoặc 2 skill
        for (int i = 0; i < skills.Length; i++)
        {

            // Lấy container bên trong prefab để bỏ skill vào
            Transform skillSlotsParent = treeObj.transform.GetComponentInChildren<SkillSlots>().transform;

            // Skill 1
            CreateSkillButton(skills[i], skillSlotsParent);

            //// Skill 2 (nếu có)
            //if (i + 1 < skills.Length)
            //{
            //    CreateSkillButton(skills[i + 1], skillSlotsParent);
            //}
        }
    }

    private void CreateSkillButton(PassiveSkillData skill, Transform parent)
    {
        var btnObj = Instantiate(skillButtonPrefab, parent);
        var icon = btnObj.transform.Find("Icon").GetComponent<Image>();
        var nameTxt = btnObj.transform.Find("Name").GetComponent<TMP_Text>();
        var priceTxt = btnObj.transform.Find("Price").GetComponent<TMP_Text>();
        var btn = btnObj.GetComponent<Button>();

        icon.sprite = skill.icon;
        nameTxt.text = skill.skillName;
        priceTxt.text = skill.price.ToString() + " coins";

        btn.onClick.AddListener(() =>
        {
            if (SkillTreeManager.Instance.BuySkill(skill))
            {
                Debug.Log("Mua thành công skill: " + skill.skillName);
                currentTierIndex++;
                if (currentTierIndex < SkillTreeManager.Instance.tiers.Count)
                    DisplayTier(currentTierIndex);
            }
            else
            {
                Debug.Log("Không đủ coin hoặc level!");
            }
        });
    }

    private PassiveSkillData[] ShuffleSkills(PassiveSkillData[] array)
    {
        var arr = (PassiveSkillData[])array.Clone();
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            var temp = arr[i];
            arr[i] = arr[rnd];
            arr[rnd] = temp;
        }
        return arr;
    }
}
