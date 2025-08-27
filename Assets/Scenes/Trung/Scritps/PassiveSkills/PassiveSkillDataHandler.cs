using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using static PassiveSkillSave;

public class PassiveSkillDataHandler : MonoBehaviour
{
    public PassiveSkillManager manager;

    public void SaveSkillData()
    {
        PassiveSkillSaveWrapper wrapper = new PassiveSkillSaveWrapper();

        foreach (var s in manager.skills)
        {
            wrapper.skills.Add(new PassiveSkillSaveData
            {
                name = s.data.skillName,
                level = s.currentLevel,
                isUnlocked = s.isUnlocked
            });
        }

        string json = JsonUtility.ToJson(wrapper);

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "PassiveSkillData", json }
            }
        };

        PlayFabClientAPI.UpdateUserData(request,
            result => Debug.Log("Lưu PassiveSkill thành công!"),
            error => Debug.LogError("Lỗi lưu PassiveSkill: " + error.GenerateErrorReport()));
    }

    public void LoadSkillData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
            result =>
            {
                if (result.Data != null && result.Data.ContainsKey("PassiveSkillData"))
                {
                    string json = result.Data["PassiveSkillData"].Value;
                    PassiveSkillSaveWrapper wrapper = JsonUtility.FromJson<PassiveSkillSaveWrapper>(json);

                    foreach (var save in wrapper.skills)
                    {
                        PassiveSkill skill = manager.skills.Find(s => s.data.skillName == save.name);
                        if (skill != null)
                        {
                            skill.currentLevel = save.level;
                            skill.isUnlocked = save.isUnlocked;
                        }
                    }

                    Debug.Log("Load tiến trình PassiveSkill thành công!");
                }
                else
                {
                    Debug.Log("Không có dữ liệu skill → Tài khoản mới, tạo mặc định");
                    manager.ResetSkillData();
                }
            },
            error => Debug.LogError("Lỗi load PassiveSkill: " + error.GenerateErrorReport()));
    }
}
