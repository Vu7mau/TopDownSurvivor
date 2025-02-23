using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextImformation : MonoBehaviour
{
    public TextMeshProUGUI nameCharacter;
    public TextMeshProUGUI idCharacter;
    private void Awake()
    {              
        nameCharacter.text ="NAME : "+PlayerPrefs.GetString("Name");
        idCharacter.text = "ID : "+PlayerPrefs.GetString("Id");
    }    
}
    