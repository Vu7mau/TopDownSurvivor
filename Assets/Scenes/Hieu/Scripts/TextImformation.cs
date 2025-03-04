using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextImformation : Singleton<TextImformation>
{
    public TextMeshProUGUI nameCharacter;
    //public TextMeshProUGUI idCharacter;
    public TextMeshProUGUI coinText;
    public void Text()
    {              
        nameCharacter.text ="NAME : "+PlayerPrefs.GetString("Name");
        //idCharacter.text = "ID : "+PlayerPrefs.GetString("Id");
        coinText.text = "x " +PlayerPrefs.GetInt("SaveCoin").ToString();
    }    
}
    