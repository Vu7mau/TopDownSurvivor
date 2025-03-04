using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissonGame : MonoBehaviour
{
    [System.Serializable]
    public class MissonData
    {
        public string title;
        public string description;
    }
    public MissonData mission;

    public Button buttonMission;
    public static MissonGame Instance;
    public MissonData selectedMission;
    private void Start()
    {
        buttonMission.onClick.AddListener(SelectMission);
    }
 
    public void SelectMission()
    {
        selectedMission = mission;
        FindObjectOfType<MissonTracker>()?.StartMission();
        buttonMission.interactable = false;
    }
}
