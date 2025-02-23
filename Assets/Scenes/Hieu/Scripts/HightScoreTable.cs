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
    private GameObject entryContainer;
    private GameObject entryTemple;    
    private List<Transform> HighscoreEntryTransformList;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("LastGameCoin"))
        {
            AddHighscoreEntry(PlayerPrefs.GetInt("LastGameCoin"), PlayerPrefs.GetString("Name"));
            PlayerPrefs.DeleteKey("LastGameCoin");
            PlayerPrefs.Save();
        }
        entryContainer = GameObject.Find("HighsScoreEntryContainer");        
        entryTemple = GameObject.Find("HighsScoreEntryTemplate");
        entryTemple.gameObject.SetActive(false);
        string jsonstring = PlayerPrefs.GetString("highscoreTable", "{}");
        Debug.Log(jsonstring);        
        Highscore highscoreData = JsonUtility.FromJson<Highscore>(jsonstring);
        if (highscoreData == null || highscoreData.hightScoreEntryList == null)
        {
            highscoreData = new Highscore { hightScoreEntryList = new List<HightScoreEntry>() };
        }
        highscoreData.hightScoreEntryList.Sort((a, b) => b.score.CompareTo(a.score));
        List<HightScoreEntry> topEntries = highscoreData.hightScoreEntryList.GetRange(0, Mathf.Min(10, highscoreData.hightScoreEntryList.Count));

        HighscoreEntryTransformList = new List<Transform>();
        foreach (HightScoreEntry t in topEntries)
        {
            CreateHighscoreEntryTransform(t, entryContainer.transform, HighscoreEntryTransformList);
        }               
    }
    private void CreateHighscoreEntryTransform(HightScoreEntry hightScoreEntry,Transform container,List<Transform> transformsList)
    {
        float templateHeight = 90;
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
        string jsonstring = PlayerPrefs.GetString("highscoreTable");
        Highscore highscores = JsonUtility.FromJson<Highscore>(jsonstring);                
        HightScoreEntry existingEntry = highscores.hightScoreEntryList.FirstOrDefault(entry =>entry.name == name);
        if (existingEntry != null)
        {
            existingEntry.score += score;
        }
        else
        {
            HightScoreEntry hightScoreEntry = new HightScoreEntry { score = score, name = name };
            highscores.hightScoreEntryList.Add(hightScoreEntry);
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