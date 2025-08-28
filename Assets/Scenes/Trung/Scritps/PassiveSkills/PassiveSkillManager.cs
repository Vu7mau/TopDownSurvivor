using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using PlayFab;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static PassiveSkillSave;
using TMPro;




[System.Serializable]
public class PassiveSkill
{
    public PassiveSkillSO data;
    public int currentLevel = 0; // cấp hiện tại
    public bool isUnlocked = false; // có thể mua hay không

    public bool IsMaxed => currentLevel >= data.levels.Length;
}




public class PassiveSkillManager : VuMonoBehaviour
{
    public static PassiveSkillManager Instance;

    public List<PassiveSkill> skills;   // list skill trong cây
    public int playerCoin = 100;

    // coin của người chơi (tạm demo)

    [SerializeField] protected List<Button> listSkillButton;

    //[SerializeField] private Sprite lockedSprite;


    [SerializeField] private bool useConfirmPopup = true;  // Bật/tắt confirm
    [SerializeField] private GameObject confirmPanel;      // Panel popup confirm
    [SerializeField] private TMP_Text confirmText;             // Text hiển thị info skill

    private PassiveSkill pendingSkill; // skill đang chờ confirm


    protected override void Start()
    {
        this.LoadPanelSkill();
    }
    
    protected virtual void LoadPanelSkill()
    {
        for (int i = 0; i < listSkillButton.Count; i++)
        {
            int index = i; // copy giá trị i ra biến cục bộ
            this.listSkillButton[index].onClick.AddListener(() => OnSkillButtonClick(this.skills[index]));
        }
    }

    protected virtual void FixedUpdate()
    {
        this.UpdateUI();
    }

    // Hàm cập nhật UI theo trạng thái skill
    public void UpdateUI()
    {
        for (int i = 0; i < listSkillButton.Count; i++)
        {
            var skill = skills[i];
            var button = listSkillButton[i];
            var icon = button; // lấy image trực tiếp từ button

            if (skill.isUnlocked)
            {
                Transform IconPassiveSkillObj = button.transform.Find("IconPassiveSkill");
                IconPassiveSkillObj.gameObject.SetActive(true);
                IconPassiveSkillObj.transform.Find("img_skill").GetComponent<Image>().sprite = skill.data.icon; // đổi sang icon skill
                button.transform.Find("LockSkill").transform.gameObject.SetActive(false);
                //button.interactable = !skill.IsMaxed;
            }
            else
            {
                Transform IconPassiveSkillObj = button.transform.Find("IconPassiveSkill");
                IconPassiveSkillObj.gameObject.SetActive(false);
                IconPassiveSkillObj.transform.Find("img_skill").GetComponent<Image>().sprite = skill.data.icon; // đổi sang icon skill
                button.transform.Find("LockSkill").transform.gameObject.SetActive(true);
                //button.GetComponent<Image>().sprite = lockedSprite; // đổi sang icon khóa
                //button.interactable = false;
            }
        }
    }


    private void OnSkillButtonClick(PassiveSkill skill)
    {
        if (!skill.isUnlocked)
        {
            Debug.Log(skill.data.skillName + " chưa được mở!");
            return;
        }

        if (skill.IsMaxed)
        {
            Debug.Log(skill.data.skillName + " đã đạt cấp tối đa!");
            return;
        }
        if (this.confirmPanel == null)
        {
            UnlockSkill(skill); // Mua luôn nếu confirm off
            return;
        }
        else
        {
            if (useConfirmPopup)
            {
                // Hiện panel confirm
                confirmPanel.SetActive(true);
                confirmText.text = $"Bạn có muốn nâng {skill.data.skillName} (Lv.{skill.currentLevel + 1}) với giá {skill.data.levels[skill.currentLevel].cost} coin không?";
                pendingSkill = skill; // lưu lại skill để xử lý sau
            }
            else
            {
                UnlockSkill(skill); // Mua luôn nếu confirm off
            }
        }
    }


    public void OnConfirmYes()
    {
        if (pendingSkill != null)
        {
            UnlockSkill(pendingSkill);
            pendingSkill = null;
        }
        confirmPanel.SetActive(false);
    }

    public void OnConfirmNo()
    {
        pendingSkill = null;
        confirmPanel.SetActive(false);
    }

    public void UnlockSkill(PassiveSkill skill)
    {
        if (!skill.isUnlocked)
        {
            Debug.Log(skill.data.skillName + " chưa được mở!");
            return;
        }

        if (skill.IsMaxed)
        {
            Debug.Log(skill.data.skillName + " đã đạt cấp tối đa!");
            return;
        }

        var nextLevel = skill.data.levels[skill.currentLevel];
        if (playerCoin < nextLevel.cost)
        {
            Debug.Log("Không đủ coin để nâng cấp " + skill.data.skillName);
            return;
        }

        // Trừ coin
        playerCoin -= nextLevel.cost;
        skill.currentLevel++;

        Debug.Log($"Nâng {skill.data.skillName} lên cấp {skill.currentLevel}, +{nextLevel.value}");




        // Áp dụng hiệu ứng
        ApplySkillEffect(skill);


        // Kiểm tra mở skill tiếp theo
        if (skill.data.nextSkill != null && skill.currentLevel >= skill.data.unlockAtLevel)
        {
            PassiveSkill next = skills.Find(s => s.data == skill.data.nextSkill);
            if (next != null && !next.isUnlocked)
            {
                next.isUnlocked = true;
                Debug.Log("Mở khóa skill mới: " + next.data.skillName);
            }
        }
    }

    private void ApplySkillEffect(PassiveSkill skill)
    {
        float bonus = skill.data.levels[skill.currentLevel - 1].value;

        switch (skill.data.skillName)
        {
            case "Attack":
                int bonusAttackTotal = PlayerPrefs.GetInt("BonusAtk", 0) + (int)bonus;
                Debug.Log("Bonus Attack: " +  bonusAttackTotal);
                PlayerPrefs.SetInt("BonusAtk", bonusAttackTotal);
                PlayerPrefs.Save();
                Debug.Log("Bonus Attack Total: " + PlayerPrefs.GetInt("BonusAtk", 0));
                //PlayerStats.Instance.attackDamage += bonus;
                break;
            case "Defense":
                //PlayerStats.Instance.defense += bonus;
                break;
            case "Critical":
                //PlayerStats.Instance.critRate += bonus;
                break;
            case "DamageRate":
                //PlayerStats.Instance.damageRate += bonus;
                break;
        }
    }


    // Reset toàn bộ skill
    public void ResetSkillData()
    {
        PlayerPrefs.DeleteKey("BonusAtk");
        Debug.Log("Bonus Attack: " + PlayerPrefs.GetInt("BonusAtk", 0));


        foreach (var s in skills)
        {
            s.currentLevel = 0;
            s.isUnlocked = false;
        }

        // Mặc định mở skill đầu tiên cấp 1
        if (skills.Count > 0)
        {
            skills[0].isUnlocked = true;
            skills[0].currentLevel = 0;
        }

        //SaveSkillData();

        Debug.Log("🔄 Reset toàn bộ skill thành công!");
    }

    protected virtual void GetBonus(float bonus)
    {

    }
}
