using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Onlock : MonoBehaviour
{
    public List<Button> buttons;
    [SerializeField] private int amountCoinBuySkin = 1000;
    [SerializeField] private GameObject _onlock;   
    private int currentButton;
    private string savekey;
    private HightScoreTable hightscore;
    private void Start()    
    {        
        hightscore = FindObjectOfType<HightScoreTable>();
        string Name = PlayerPrefs.GetString("Name");
        savekey = "Name " + Name;
        string jsonString = PlayerPrefs.GetString(savekey, "{}");
        Skin sk = JsonUtility.FromJson<Skin>(jsonString);       
        for (int i = 0; i < buttons.Count; i++)
        {           
            if (sk.entries.Count < buttons.Count)
            {
                addButton(i);
            }
            int index = i;            
            buttons[i].onClick.AddListener(() => Panel_onclick(index));            
            SKinEntry entry = sk.entries.FirstOrDefault(e => e.buttons == index);
            if (entry != null && entry.isbutton == true)
            {
                buttons[index].gameObject.SetActive(false); 
            }
        }
    }    
    private void Panel_onclick(int i)
    {        
        currentButton = i;        
        _onlock.SetActive(true);           
    }
    public void OnLockSkin()
    {
        int currentcoin = PlayerPrefs.GetInt("SaveCoin")-amountCoinBuySkin;          
        string jsonString = PlayerPrefs.GetString( savekey, "{}");
        Skin sk = JsonUtility.FromJson<Skin>(jsonString);
        if (sk.entries == null)
        {
            sk.entries = new List<SKinEntry>();
        }
        if (currentcoin >=0) {
            PlayerPrefs.SetInt("LastGameCoin", -amountCoinBuySkin);
            PlayerPrefs.Save();            
            hightscore.updateHightScore();                     
            _onlock.SetActive(false);            
            for (int i = 0; i < buttons.Count; i++)
            {
                if (sk.entries[i].buttons == currentButton)
                {
                    buttons[currentButton].gameObject.SetActive(false);
                    sk.entries[i].isbutton = true;
                    break;
                }
            }            
        }
        else
        {
            Notification.noti_instance.Shownotification("Không đủ coin để mở khóa skin");
        }
        string json = JsonUtility.ToJson(sk);
        PlayerPrefs.SetString(savekey, json);
        PlayerPrefs.Save();
    }
    private void addButton(int button)
    {
        string jsonString = PlayerPrefs.GetString(savekey, "{}");
        Skin sk = JsonUtility.FromJson<Skin>(jsonString);               
        SKinEntry entry = new SKinEntry { isbutton = false,buttons = button };                    
        sk.entries.Add(entry);
        string json = JsonUtility.ToJson(sk);
        PlayerPrefs.SetString(savekey, json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString(savekey));
    }

    private class Skin
    {        
        public List<SKinEntry> entries = new List<SKinEntry>();
    }
    [Serializable]
    private class SKinEntry
    {        
        public bool isbutton;
        public int buttons;
    }
    private void reset()
    {
        Skin skin =  new Skin { entries = new List<SKinEntry>() };
        string json = JsonUtility.ToJson(skin);
        PlayerPrefs.SetString(savekey, json);
        PlayerPrefs.Save();
    }
}
