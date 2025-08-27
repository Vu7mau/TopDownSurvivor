using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillSave : MonoBehaviour
{
    [System.Serializable]
    public class PassiveSkillSaveData
    {
        public string name;
        public int level;
        public bool isUnlocked;
    }

    [System.Serializable]
    public class PassiveSkillSaveWrapper
    {
        public List<PassiveSkillSaveData> skills = new List<PassiveSkillSaveData>();
    }
}
