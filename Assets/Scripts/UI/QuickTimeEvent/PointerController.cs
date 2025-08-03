using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PointerController : VuMonoBehaviour
{

    [Header("Pointer Settings")]
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZone; // Gắn vùng safe
    public float moveSpeed = 100f;

    private RectTransform pointerTransform;
    private Vector3 targetPosition;
    private bool isActive = false;

    public System.Action<bool> OnQTEResult;

    private Vector2 defaultSafeZoneSize;
    private Vector2 defaultSafeZonePos;
    private float baseMoveSpeed;
    private float speedMultiplier = 1f;
    protected override void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        targetPosition = pointB.position;
        baseMoveSpeed = moveSpeed;

        defaultSafeZoneSize = safeZone.sizeDelta;
        defaultSafeZonePos = safeZone.anchoredPosition;
    }
    void Update()
    {
        if (!isActive) return;

        pointerTransform.position = Vector3.MoveTowards(  pointerTransform.position, targetPosition, baseMoveSpeed * speedMultiplier * Time.unscaledDeltaTime);

        if (Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f)
            targetPosition = pointB.position;
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1f)
            targetPosition = pointA.position;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool success = RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null);
            OnQTEResult?.Invoke(success);
            //isActive = false;
        }
    }
    public void IncreaseSpeed(float amount = 0.1f)
    {
        speedMultiplier += amount;
    }
    public void ResetState()
    {
        // Reset tốc độ
        speedMultiplier = 1f;

        // Reset SafeZone
        safeZone.sizeDelta = defaultSafeZoneSize;
        safeZone.anchoredPosition = defaultSafeZonePos;

        // Reset vị trí con trỏ
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

    public void ShrinkSafeZoneWidthOnly(float shrinkFactor, float moveRangeX)
    {
        // Lấy kích thước hiện tại
        Vector2 size = safeZone.sizeDelta;

        // Giảm chiều rộng (x), giữ nguyên chiều cao (y)
        float newWidth = size.x * shrinkFactor;

        safeZone.DOSizeDelta(new Vector2(newWidth, size.y), 0.2f).SetEase(Ease.OutBack);

        // Di chuyển theo chiều ngang
        float offsetX = Random.Range(-moveRangeX, moveRangeX);
        var rect = (safeZone.parent as RectTransform).rect;
        Vector2 newPos = safeZone.anchoredPosition + new Vector2(offsetX, 0);

        float maxX = (rect.width - newWidth) / 2f;
        newPos.x = Mathf.Clamp(newPos.x, -maxX, maxX);

        safeZone.DOAnchorPos(newPos, 0.2f).SetEase(Ease.OutBack);
    }
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
        safeZone.DOShakeAnchorPos(0.2f, 10f, 10, 90).SetEase(Ease.InOutSine);
    }
}
