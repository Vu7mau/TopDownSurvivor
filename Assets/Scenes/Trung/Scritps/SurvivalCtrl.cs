using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SurvivalCtrl : VuMonoBehaviour
{
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
}
