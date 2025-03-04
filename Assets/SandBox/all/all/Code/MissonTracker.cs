using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class MissonTracker : Singleton<MissonTracker>
{
    public MissonGame missionGame;
    public TextMeshProUGUI progressText;
    public Button claimRewardButton;
    public GameObject coinPrefab;
    public Transform rewardSpawnPoint;
    private bool missonCompleted = false;
    private int bosskillCount = 0;
    private int targetBossCount = 1;
    public int coinReward = 100;
    //Ẩn trap
    public GameObject trap;
    protected override void Start()
    {
        missionGame = FindObjectOfType<MissonGame>();
        claimRewardButton.onClick.RemoveAllListeners();
        claimRewardButton.onClick.AddListener(ClaimReward);
        claimRewardButton.interactable = false;
        //progressText.gameObject.SetActive(false);
        claimRewardButton.gameObject.SetActive(false);
        if (missionGame.selectedMission != null)
        {
           // UpdateProgressText();
        }
        else
        {
            Debug.Log("");
        }
    }

    public void BossKilled(GameObject boss)
    {
        if (!missonCompleted&& boss!=null)
        {
            bosskillCount++;
            if(bosskillCount >= targetBossCount)
            {
                missonCompleted = true;
               // UpdateProgressText();
                Debug.Log("Nhiệm vụ hoàn thành: Tiêu diệt boss");
                trap?.gameObject.SetActive(false);
                claimRewardButton.gameObject.SetActive(true);
                claimRewardButton.interactable = true;
            }
          //  UpdateProgressText();
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Boss"))
    //    {
    //        BossKilled(other.gameObject);
    //    }
    //}
    //private void UpdateProgressText()
    //{
    //    progressText.text = $"Tiến độ: {bosskillCount} / {targetBossCount} Boss";
    //}
    private void ClaimReward()
    {
        claimRewardButton.interactable = false;
        claimRewardButton.gameObject.SetActive(false);
        progressText.gameObject.SetActive(false);
        int coins = PlayerPrefs.GetInt("Coins", 0);
        PlayerPrefs.SetInt("Coins", coins + coinReward);
        PlayerPrefs.Save();
        if(PlayerData.Instance != null)
        {
            PlayerData.Instance.AddCoin(coinReward);
        }
        else
        {
            Debug.Log("Lỗi");
        }
        
        SpawnRewards(coinReward);
    }
    private void SpawnRewards(int rewardText)
    {
        int coinAmount = rewardText;

        for (int i = 0; i < coinAmount; i++)
        {
            Instantiate(coinPrefab, rewardSpawnPoint.position + Random.insideUnitSphere * 2, Quaternion.identity);
        }
    }
    protected override void OnEnable()
    {
        EnemyManagerMisson.OnBossKilled += BossKilled;
    }

    protected override void OnDisable()
    {
        EnemyManagerMisson.OnBossKilled -= BossKilled;
    }
    public void StartMission()
    {
        Debug.Log("Nhiệm vụ bắt đầu: Tiêu diệt Boss");
        progressText.gameObject.SetActive(true);
       // UpdateProgressText();
    }

}
