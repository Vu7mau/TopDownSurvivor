using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ScrollRect))]
public class SmoothMouseScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [Header("Cấu hình")]
    public float scrollSpeed = 50f;
    public float smoothTime = 0.08f;

    [Tooltip("Gán manager chung quản lý quyền cuộn")]
    public ScrollLockManager lockManager;

    private ScrollRect scrollRect;
    private float targetPosition;
    private float velocity;
    private RectTransform viewport;

    private bool initialized = false;
    private bool userScrolled = false;
    private bool isDragging = false;

    private Camera uiCamera;

    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        viewport = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.GetComponent<RectTransform>();

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
            uiCamera = canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null;
    }

    void OnEnable()
    {
        initialized = false;
        userScrolled = false;
        isDragging = false;
        StartCoroutine(DelayScrollInit());
    }

    private IEnumerator DelayScrollInit()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        targetPosition = scrollRect.verticalNormalizedPosition;
        initialized = true;
    }

    void Update()
    {
        if (!initialized) return;

        float currentPos = scrollRect.verticalNormalizedPosition;
        float scrollDelta = Input.mouseScrollDelta.y;

        if (!isDragging && IsMouseOverViewport() && Mathf.Abs(scrollDelta) > 0.01f)
        {
            if (lockManager != null)
            {
                bool canScroll = lockManager.RequestLock(this);
                if (canScroll)
                {
                    float deltaNormalized = (scrollDelta * scrollSpeed) / scrollRect.content.rect.height;
                    targetPosition += deltaNormalized;
                    targetPosition = Mathf.Clamp01(targetPosition);
                    userScrolled = true;

                    lockManager.RefreshLock(this);
                }
            }
            else
            {
                // Nếu không có manager thì cuộn luôn
                float deltaNormalized = (scrollDelta * scrollSpeed) / scrollRect.content.rect.height;
                targetPosition += deltaNormalized;
                targetPosition = Mathf.Clamp01(targetPosition);
                userScrolled = true;
            }
        }

        if (!isDragging && userScrolled)
        {
            scrollRect.verticalNormalizedPosition = Mathf.SmoothDamp(
                currentPos,
                targetPosition,
                ref velocity,
                smoothTime
            );

            // Nếu đã gần tới đích thì báo nhả quyền
            if (lockManager != null && Mathf.Abs(scrollRect.verticalNormalizedPosition - targetPosition) < 0.0001f)
            {
                lockManager.ReleaseLock(this);
                userScrolled = false;
            }
        }
        else if (!isDragging)
        {
            targetPosition = currentPos;
        }
    }

    bool IsMouseOverViewport()
    {
        if (EventSystem.current == null) return false;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject != null && result.gameObject.transform.IsChildOf(viewport))
                return true;
        }
        return false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        userScrolled = false;
        if (lockManager != null)
            lockManager.RequestLock(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        targetPosition = scrollRect.verticalNormalizedPosition;
        if (lockManager != null)
            lockManager.ReleaseLock(this);
    }
}
