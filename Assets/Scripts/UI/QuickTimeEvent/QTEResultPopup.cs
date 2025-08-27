using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QTEResultPopup : VuMonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TMP_Text popupMessage;
    [SerializeField] private TMP_Text title;
    [SerializeField] private Button popupCloseButton;

    [Header("Show Animation")]
    [SerializeField] private float showScaleDuration = 0.4f;
    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private Vector3 showStartScale = Vector3.zero;

    [Header("Auto Pin (thu nhỏ & đưa vào góc)")]
    [Tooltip("Tự thu nhỏ và ghim vào góc sau bấy nhiêu giây (<=0 để tắt)")]
    [SerializeField] private float autoPinDelay = 2.0f;
    [SerializeField] private float pinMoveDuration = 0.35f;
    [SerializeField] private Ease pinEase = Ease.InOutQuad;
    [SerializeField] private Vector3 pinnedScale = new Vector3(0.65f, 0.65f, 1f);

    public enum PinPosition { TopRight, TopLeft, BottomRight, BottomLeft, Custom }
    [Header("Vị trí Ghim")]
    [SerializeField] private PinPosition pinPosition = PinPosition.TopRight;
    [Tooltip("Chỉ dùng khi PinPosition = Custom (tọa độ anchored, tính theo RectTransform của chính panel).")]
    [SerializeField] private Vector2 customAnchoredPos = new Vector2(-20, -20);

    [Header("Lề khi ghim (theo góc)")]
    [SerializeField] private Vector2 margin = new Vector2(20, 20);

    [Header("Text Options")]
    [SerializeField] private float messageCharacterSpacing = 10f;

    private RectTransform panelRect;
    private Canvas rootCanvas;
    private Coroutine pinRoutine;
    private Sequence currentSeq;

    protected override void Awake()
    {
        base.Awake();

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
            panelRect = popupPanel.GetComponent<RectTransform>();
            rootCanvas = popupPanel.GetComponentInParent<Canvas>();
        }

        if (popupCloseButton != null)
            popupCloseButton.onClick.AddListener(HidePopup);
    }

    /// <summary>
    /// Hiển thị popup: phóng to vào giữa. Sau autoPinDelay sẽ thu nhỏ & ghim vào góc (nếu autoPinDelay > 0).
    /// </summary>
    public void Show(string titleText, string message)
    {
        Debug.Log($"Title: {titleText} | Message: {message}");

        if (popupPanel == null || panelRect == null) return;

        // hủy tweens / coroutine cũ (nếu còn)
        if (currentSeq != null && currentSeq.IsActive()) currentSeq.Kill();
        if (pinRoutine != null) { StopCoroutine(pinRoutine); pinRoutine = null; }

        // set nội dung
        if (title != null) title.text = titleText;
        if (popupMessage != null)
        {
            popupMessage.characterSpacing = messageCharacterSpacing;
            popupMessage.text = message;
        }

        // bật panel & reset transform
        popupPanel.SetActive(true);

        // Đặt giữa màn hình & scale về showStartScale
        CenterInScreen();
        panelRect.localScale = showStartScale;

        // Animate phóng to
        currentSeq = DOTween.Sequence()
            .Append(panelRect.DOScale(Vector3.one, showScaleDuration).SetEase(showEase));

        // Sau X giây thì tự ghim (nếu bật)
        if (autoPinDelay > 0f)
        {
            pinRoutine = StartCoroutine(Co_AutoPinAfterDelay(autoPinDelay));
        }
    }

    /// <summary>
    /// Ẩn popup (kể cả đang ghim).
    /// </summary>
    public void HidePopup()
    {
        if (popupPanel == null || panelRect == null) return;

        if (currentSeq != null && currentSeq.IsActive()) currentSeq.Kill();
        if (pinRoutine != null) { StopCoroutine(pinRoutine); pinRoutine = null; }

        // Animate thu nhỏ rồi tắt
        panelRect.DOScale(Vector3.zero, 0.25f)
                 .SetEase(Ease.InBack)
                 .OnComplete(() => popupPanel.SetActive(false));
    }

    /// <summary>
    /// Gọi lại để bỏ ghim và hiện lớn giữa màn hình (ví dụ khi có thông điệp mới).
    /// </summary>
    public void UnpinAndExpand(string newTitle = null, string newMessage = null)
    {
        if (newTitle != null && title != null) title.text = newTitle;
        if (newMessage != null && popupMessage != null) popupMessage.text = newMessage;

        Show(title != null ? title.text : "", popupMessage != null ? popupMessage.text : "");
    }

    // ========== Core helpers ==========

    private IEnumerator Co_AutoPinAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PinToCorner();
        pinRoutine = null;
    }

    private void PinToCorner()
    {
        if (panelRect == null) return;

        if (currentSeq != null && currentSeq.IsActive()) currentSeq.Kill();

        // Tính anchor, pivot và vị trí theo PinPosition
        Vector2 targetAnchorMin, targetAnchorMax, targetPivot, targetPos;
        GetPinLayout(out targetAnchorMin, out targetAnchorMax, out targetPivot, out targetPos);

        // Chuyển anchor/pivot mượt mà: DOTween không tween trực tiếp anchorMin/Max, nên ta đặt ngay anchor/pivot,
        // còn di chuyển bằng DOAnchorPos cho cảm giác mượt.
        panelRect.anchorMin = targetAnchorMin;
        panelRect.anchorMax = targetAnchorMax;
        panelRect.pivot = targetPivot;

        // Animate move + scale
        currentSeq = DOTween.Sequence()
            .Append(panelRect.DOScale(pinnedScale, pinMoveDuration).SetEase(pinEase))
            .Join(panelRect.DOAnchorPos(targetPos, pinMoveDuration).SetEase(pinEase));
    }

    private void CenterInScreen()
    {
        if (panelRect == null) return;

        // Đặt anchor trung tâm, pivot trung tâm, pos = 0
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.localScale = Vector3.one;
    }

    private void GetPinLayout(out Vector2 aMin, out Vector2 aMax, out Vector2 pivot, out Vector2 anchoredPos)
    {
        // Mặc định
        aMin = aMax = pivot = new Vector2(0.5f, 0.5f);
        anchoredPos = Vector2.zero;

        switch (pinPosition)
        {
            case PinPosition.TopRight:
                aMin = aMax = new Vector2(1f, 1f);
                pivot = new Vector2(1f, 1f);
                anchoredPos = new Vector2(-margin.x, -margin.y); // dịch vào trong
                break;

            case PinPosition.TopLeft:
                aMin = aMax = new Vector2(0f, 1f);
                pivot = new Vector2(0f, 1f);
                anchoredPos = new Vector2(margin.x, -margin.y);
                break;

            case PinPosition.BottomRight:
                aMin = aMax = new Vector2(1f, 0f);
                pivot = new Vector2(1f, 0f);
                anchoredPos = new Vector2(-margin.x, margin.y);
                break;

            case PinPosition.BottomLeft:
                aMin = aMax = new Vector2(0f, 0f);
                pivot = new Vector2(0f, 0f);
                anchoredPos = new Vector2(margin.x, margin.y);
                break;

            case PinPosition.Custom:
                // Custom: giữ anchor/pivot ở TopRight để dễ hình dung (có thể chỉnh tuỳ ý),
                // và dùng customAnchoredPos trực tiếp.
                aMin = aMax = new Vector2(1f, 1f);
                pivot = new Vector2(1f, 1f);
                anchoredPos = customAnchoredPos;
                break;
        }
    }

    // ===== Optional: API công khai để ghim/bỏ ghim theo lệnh bên ngoài =====

    /// <summary>
    /// Ghim ngay lập tức (bỏ qua delay).
    /// </summary>
    public void PinNow()
    {
        if (pinRoutine != null) { StopCoroutine(pinRoutine); pinRoutine = null; }
        PinToCorner();
    }

    /// <summary>
    /// Bỏ ghim & đưa về giữa (không đổi scale), dùng trước khi animate mở mới.
    /// </summary>
    public void ResetToCenter()
    {
        if (currentSeq != null && currentSeq.IsActive()) currentSeq.Kill();
        CenterInScreen();
    }
}
