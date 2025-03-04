using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class HightScoreTable : MonoBehaviour
{      
    [SerializeField] private GameObject entryContainer;
    [SerializeField] private GameObject entryTemple; 
    [SerializeField] private GameObject CurrentRank;
    
    private List<Transform> HighscoreEntryTransformList;     
    private void Awake()
    {
    }
    private void Start()
    {
        updateHightScore();
        
    }
    public void updateHightScore ()
    {
        AddHighscoreEntry(PlayerPrefs.GetInt("LastGameCoin"), PlayerPrefs.GetString("Name"));
         PlayerPrefs.DeleteKey("LastGameCoin");
         PlayerPrefs.Save();    
         Singleton<TextImformation>.Instance.Text();
        //entryContainer = GameObject.Find("HighsScoreEntryContainer");        
        //entryTemple = GameObject.Find("HighsScoreEntryTemplate");
        entryTemple.gameObject.SetActive(false);
        string jsonstring = PlayerPrefs.GetString("highscoreTable", "{}");
        Debug.Log(jsonstring);
        Highscore highscoreData = JsonUtility.FromJson<Highscore>(jsonstring);
        if (highscoreData == null || highscoreData.hightScoreEntryList == null)
        {
            highscoreData = new Highscore { hightScoreEntryList = new List<HightScoreEntry>() };
        }
        highscoreData.hightScoreEntryList.Sort((a, b) => b.score.CompareTo(a.score));
        List<HightScoreEntry> topEntries = highscoreData.hightScoreEntryList.GetRange(0, Mathf.Min(5, highscoreData.hightScoreEntryList.Count));        
        HighscoreEntryTransformList = new List<Transform>();
        foreach (HightScoreEntry t in topEntries)
        {
            CreateHighscoreEntryTransform(t, entryContainer.transform, HighscoreEntryTransformList);
        }
        int playerrank = highscoreData.hightScoreEntryList.FindIndex(x => x.name == PlayerPrefs.GetString("Name"))+1;
        if (playerrank > 0)
        {
            HightScoreEntry hightScoreEntry = highscoreData.hightScoreEntryList[playerrank-1];      
            CurrentRank.transform.Find("Postext").GetComponent<TextMeshProUGUI>().text = playerrank.ToString();
            CurrentRank.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = hightScoreEntry.score.ToString();
            CurrentRank.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = hightScoreEntry.name;
            switch (playerrank)
            {
                default: CurrentRank.transform.Find("trophy").gameObject.SetActive(false); break;
                case 1: CurrentRank.transform.Find("trophy").gameObject.SetActive(true); break;
            }
        }
    }
    private void CreateHighscoreEntryTransform(HightScoreEntry hightScoreEntry,Transform container,List<Transform> transformsList)
    {
        float templateHeight = 110;
        Transform entryTransform = Instantiate(entryTemple.transform, container.transform);
        RectTransform rectTransform = entryTransform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformsList.Count);        
        entryTransform.gameObject.SetActive(true);
        int rank = transformsList.Count + 1;
        string rankString;
        switch (rank)
        {
            default: rankString = rank + "TH"; break;
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ST"; break;
            case 3: rankString = "3ST"; break;
        }
        entryTransform.Find("Postext").GetComponent<TextMeshProUGUI>().text = rankString;
        int score = hightScoreEntry.score;
        entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = score.ToString(); ;
        string name = hightScoreEntry.name;
        entryTransform.Find("NameText").GetComponent<TextMeshProUGUI>().text = name;
        //sentryTransform.gameObject.SetActive(rank % 2 == 1);
        if (rank == 1)
        {
            entryTransform.Find("Postext").GetComponent<TextMeshProUGUI>().color = Color.yellow;
            entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().color = Color.yellow;
            entryTransform.Find("NameText").GetComponent<TextMeshProUGUI>().color = Color.yellow;
        }        
        Transform entry = entryTransform.Find("trophy");
        switch (rank)
        {
            default: entryTransform.Find("trophy").gameObject.SetActive(false); break;
            case 1: entryTransform.Find("trophy").gameObject.SetActive(true); break;            
        }
        transformsList.Add(entryTransform);
    }
    public void AddHighscoreEntry(int score,string name)
    {
        //createHighscoreEntry        
        //LoadSave Highscore
        string jsonstring = PlayerPrefs.GetString("highscoreTable","{}");
        Highscore highscores = JsonUtility.FromJson<Highscore>(jsonstring);                
        HightScoreEntry existingEntry = highscores.hightScoreEntryList.FirstOrDefault(entry =>entry.name == name);
        if (existingEntry != null)
        {
            existingEntry.score += score;
            PlayerPrefs.SetInt("SaveCoin", existingEntry.score);
        }
        else
        {
            HightScoreEntry hightScoreEntry = new HightScoreEntry { score = score, name = name };
            highscores.hightScoreEntryList.Add(hightScoreEntry);
            PlayerPrefs.SetInt("SaveCoin", 0);
        }
        //add new entry to highscore              
        // save upload
        string json = JsonUtility.ToJson(highscores);        
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }
    private class Highscore
    {
        public List<HightScoreEntry> hightScoreEntryList;
    }
    [Serializable]
    public  class HightScoreEntry
    {
        public int score;
        public string name;
    }
    public void save()
    {
        if (PlayerPrefs.HasKey("Name") && PlayerPrefs.HasKey("Coin"))
        {
            AddHighscoreEntry(PlayerPrefs.GetInt("Coin"), PlayerPrefs.GetString("Name"));
        }
    }
    public void ClearHighscoreTable()
    {
        Highscore emptyHighscore = new Highscore { hightScoreEntryList = new List<HightScoreEntry>() };
        string json = JsonUtility.ToJson(emptyHighscore);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();        
    }
}