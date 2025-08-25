using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalCtrl : VuMonoBehaviour
{
    [Space]
    [Header("Ready To Fight!")]

    [SerializeField] protected int timeToReadyFight = 10;
    [SerializeField] protected Transform panelReady;
    [SerializeField] protected Transform panelTopLeft;
    [SerializeField] protected WaveSpawner waveSpawner;
    protected bool isSpawning = false;


    [SerializeField] protected RectTransform panelWave;
    [SerializeField] protected RectTransform panelStats;
    [SerializeField] protected float moveOffsetX = 500f; // khoảng cách PosX
    [SerializeField] protected float moveDuration = 0.5f; // thời gian tween

    protected bool isPanelWave = true;
    protected Vector2 waveDefaultPos;
    protected Vector2 statsDefaultPos;

    protected override void OnEnable()
    {
        this.panelWave.transform.gameObject.SetActive(true);
        this.panelStats.transform.gameObject.SetActive(true);

        // Lưu vị trí mặc định
        waveDefaultPos = panelWave.anchoredPosition;
        statsDefaultPos = panelStats.anchoredPosition; // Panel Stats sẽ vào đúng chỗ Panel Wave

        // Đặt panelStats ở bên trái ngoài viewport
        panelStats.anchoredPosition = statsDefaultPos + Vector2.left * moveOffsetX;
    }

    protected override void Start()
    {
        base.Start();
        this.ReadyToFight();
    }

    protected virtual void Update()
    {
        CtrlPanelSurvival();
    }

    protected virtual void CtrlPanelSurvival()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isPanelWave)
            {
                // Wave trượt sang trái
                panelWave.DOAnchorPos(waveDefaultPos + Vector2.left * moveOffsetX, moveDuration).SetEase(Ease.OutQuad);
                // Stats trượt vào vị trí Wave
                panelStats.DOAnchorPos(statsDefaultPos, moveDuration).SetEase(Ease.OutQuad);
            }
            else
            {
                // Wave trở lại vị trí ban đầu
                panelWave.DOAnchorPos(waveDefaultPos, moveDuration).SetEase(Ease.OutQuad);
                // Stats trượt về bên trái
                panelStats.DOAnchorPos(statsDefaultPos + Vector2.left * moveOffsetX, moveDuration).SetEase(Ease.OutQuad);
            }

            isPanelWave = !isPanelWave;
        }
    }

    protected virtual void ReadyToFight()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        int currentTime = timeToReadyFight;

        if(this.panelTopLeft != null) this.panelTopLeft.gameObject.SetActive(false);
        if (this.panelReady != null) this.panelReady.transform.gameObject.SetActive(true);
        TMP_Text countdownText = this.panelReady.transform.Find("Ready_Time").GetComponent<TMP_Text>();


        while (currentTime >= 0)
        {
            Debug.Log("Time: " + currentTime); // In ra console
            if (countdownText != null)
             countdownText.text = currentTime.ToString(); // Cập nhật UI Text

            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        if(SoundFXManager.Instance.bg_Survival != null) SoundEnemyManager.Instance.PlayBGMusic(SoundFXManager.Instance.bg_Survival, this.transform);
        if (this.panelTopLeft != null) this.panelTopLeft.gameObject.SetActive(true);
        if (this.panelReady != null) this.panelReady.transform.gameObject.SetActive(false);
        if (!isSpawning)
            StartCoroutine(this.waveSpawner.HandleWaves());

        //Debug.Log("Time's up!");
        //if (countdownText != null)
        //    countdownText.text = "0";
    }
}
