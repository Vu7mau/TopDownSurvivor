using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointerController : VuMonoBehaviour
{

    [Header("Pointer Settings")]
    [SerializeField] public TMP_Text logPass;
    public Transform pointA;
    public Transform pointB;

    [Header("UI Refs")]
    public RectTransform safeZone;     // vùng safe (UI) sẽ di chuyển/đổi size
    public RectTransform background;   // giới hạn không được vượt ra ngoài

    public float moveSpeed = 100f;

    private RectTransform pointerTransform;
    private Vector3 targetPosition;
    private bool isActive = false;

    public System.Action<bool> OnQTEResult;

    private Vector2 defaultSafeZoneSize;
    private Vector2 defaultSafeZonePos;
    private float baseMoveSpeed;
    private float speedMultiplier = 1f;

    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        targetPosition = pointB.position;
        baseMoveSpeed = moveSpeed;

        if (safeZone == null || background == null)
        {
            Debug.LogWarning("[PointerController] Vui lòng gán safeZone và background!");
            return;
        }

        // Khuyến nghị pivot center để clamp chuẩn
        if (safeZone.pivot != new Vector2(0.5f, 0.5f))
            Debug.LogWarning("[PointerController] Nên đặt pivot của safeZone = (0.5, 0.5) để clamp chính xác.");
        // Anchors của safeZone nên là center (hoặc fixed) để anchoredPosition hoạt động ổn định
        // (Không bắt buộc, vì mình dùng bounds theo parent)

        defaultSafeZoneSize = safeZone.sizeDelta;
        defaultSafeZonePos = safeZone.anchoredPosition;

        // Clamp ngay lúc start cho chắc
        safeZone.anchoredPosition = ClampInsideBackground(
            desiredPos: safeZone.anchoredPosition,
            targetSize: safeZone.sizeDelta
        );
    }

    void Update()
    {
        if (!isActive) return;

        pointerTransform.position = Vector3.MoveTowards(
            pointerTransform.position,
            targetPosition,
            baseMoveSpeed * speedMultiplier * Time.unscaledDeltaTime
        );

        if (Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f)
            targetPosition = pointB.position;
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1f)
            targetPosition = pointA.position;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool success = RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null);
            OnQTEResult?.Invoke(success);
        }
    }

    public void IncreaseSpeed(float amount = 1.5f)
    {
        speedMultiplier *= amount;
    }

    public void ResetState()
    {
        speedMultiplier = 1f;

        // Reset SafeZone (kèm clamp theo background)
        safeZone.sizeDelta = defaultSafeZoneSize;
        safeZone.anchoredPosition = ClampInsideBackground(defaultSafeZonePos, safeZone.sizeDelta);

        // Reset con trỏ
        pointerTransform.position = pointA.position;
        targetPosition = pointB.position;
    }

    public void StartQTE()
    {
        isActive = true;
        pointerTransform.position = pointA.position;
        targetPosition = pointB.position;
    }

    public void StopQTE()
    {
        isActive = false;
    }

    /// <summary>
    /// Thu nhỏ CHIỀU RỘNG và di chuyển theo trục X, luôn clamp để KHÔNG vượt ra ngoài 'background'.
    /// </summary>
    public void ShrinkSafeZoneWidthOnly(float shrinkFactor, float moveRangeX)
    {
        if (safeZone == null || background == null) return;

        // 1) Width mới
        Vector2 curSize = safeZone.sizeDelta;
        float newWidth = Mathf.Max(1f, curSize.x * shrinkFactor);
        Vector2 targetSize = new Vector2(newWidth, curSize.y);

        // 2) Vị trí mong muốn + clamp theo background
        float offsetX = Random.Range(-moveRangeX, moveRangeX);
        Vector2 desiredPos = safeZone.anchoredPosition + new Vector2(offsetX, 0f);
        Vector2 clampedPos = ClampInsideBackground(desiredPos, targetSize);

        // 3) Tween đồng bộ size + pos (unscaled)
        DOTween.Kill(safeZone);
        var seq = DOTween.Sequence().SetUpdate(true);
        seq.Join(safeZone.DOSizeDelta(targetSize, 0.2f).SetEase(Ease.OutBack));
        seq.Join(safeZone.DOAnchorPos(clampedPos, 0.2f).SetEase(Ease.OutBack));
    }

    /// <summary>
    /// Clamp anchoredPosition của safeZone để KHÔNG vượt ra ngoài 'background'.
    /// Tính trên không gian local của parent của safeZone (dùng RelativeBounds).
    /// </summary>
    private Vector2 ClampInsideBackground(Vector2 desiredPos, Vector2 targetSize)
    {
        var parentRT = safeZone.parent as RectTransform;
        if (parentRT == null || background == null)
            return desiredPos;

        // Bounds của background trong KHÔNG GIAN parentRT (local)
        Bounds bgBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(parentRT, background);
        // Với safeZone pivot center
        float halfW = targetSize.x * 0.5f;
        float halfH = targetSize.y * 0.5f;

        float minX = bgBounds.min.x + halfW;
        float maxX = bgBounds.max.x - halfW;
        float minY = bgBounds.min.y + halfH;
        float maxY = bgBounds.max.y - halfH;

        return new Vector2(
            Mathf.Clamp(desiredPos.x, minX, maxX),
            Mathf.Clamp(desiredPos.y, minY, maxY)
        );
    }

    // Hiệu ứng
    public void PlaySuccessEffect(AudioSource source, AudioClip clip)
    {
        Image image = safeZone.GetComponent<Image>();
        if (source && clip) source.PlayOneShot(clip);

        image.color = Color.green;
        image.DOFade(0.5f, 0.1f).SetLoops(2, LoopType.Yoyo).OnComplete(() => image.color = Color.white);
    }

    public void PlayFailEffect(AudioSource source, AudioClip clip)
    {
        Image image = safeZone.GetComponent<Image>();
        if (source && clip) source.PlayOneShot(clip);

        image.color = Color.red;
        image.DOFade(0.3f, 0.1f).SetLoops(2, LoopType.Yoyo).OnComplete(() => image.color = Color.white);
        safeZone.DOShakeAnchorPos(0.2f, 10f, 10, 90).SetEase(Ease.InOutSine).SetUpdate(true);
    }
}
