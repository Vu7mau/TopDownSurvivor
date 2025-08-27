using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CreditSimpleScroller : MonoBehaviour
{
    public enum StartPosition { Top, Middle, Bottom }

    [Header("Refs")]
    public TextMeshProUGUI creditText;   // TextMeshProUGUI có sẵn
    public RectTransform viewport;       // Khung nhìn (Panel có Mask)
    public RectTransform textRect;       // RectTransform của creditText

    [Header("Content")]
    [TextArea(10, 30)]
    public string creditContent;

    [Header("Scroll")]
    public float scrollSpeed = 60f;
    public float startDelay = 1f;
    public float endDelay = 1.5f;
    public StartPosition startPosition = StartPosition.Bottom;

    [Header("Loop Options")]
    public bool loop = true;                 // Bật lặp vô hạn
    public float loopGapPixels = 200f;       // Khoảng trống giữa 2 vòng
    public float pauseAtTopSeconds = 0.0f;   // Tạm dừng ngắn khi chạm đỉnh (chỉ khi loop)

    [Header("End Behaviour (khi loop = false)")]
    public string nextSceneName;
    public bool quitGameAfter = false;

    [Header("Controls")]
    public bool allowSkipToEnd = true;       // Nhấn bất kỳ phím/click để nhảy tới cuối (khi không loop)
    public bool allowTogglePause = true;     // Nhấn Space để tạm dừng/tiếp tục

    // Runtime
    private bool isScrolling = false;
    private bool isPaused = false;
    private float startTime;
    private float targetY;
    private float startY;
    private float pauseTimer = 0f;

    void Start()
    {
        if (!creditText || !viewport || !textRect)
        {
            Debug.LogError("[CreditSimpleScroller] Chưa gán đủ tham chiếu (creditText, viewport, textRect)!");
            return;
        }

        // Gán nội dung
        creditText.text = creditContent;

        // Cập nhật kích thước Text ngay để tính toán chính xác
        Canvas.ForceUpdateCanvases();

        // Tính vị trí bắt đầu theo lựa chọn
        switch (startPosition)
        {
            case StartPosition.Top:
                startY = viewport.rect.height / 2f; // xuất hiện ở trên
                break;
            case StartPosition.Middle:
                startY = -(textRect.rect.height / 2f); // giữa viewport
                break;
            default:
                startY = -(viewport.rect.height + 50f); // dưới viewport
                break;
        }
        textRect.anchoredPosition = new Vector2(0, startY);

        // Mốc kết thúc 1 vòng cuộn (khi text đi hết lên trên)
        targetY = textRect.rect.height + viewport.rect.height + loopGapPixels;

        startTime = Time.time + startDelay;
        isScrolling = true;
    }

    void Update()
    {
        if (!isScrolling) return;
        if (Time.time < startTime) return;

        // Toggle pause (Space)
        if (allowTogglePause && Input.GetKeyDown(KeyCode.Space))
            isPaused = !isPaused;

        if (isPaused) return;

        // Skip tới cuối (khi không loop)
        if (!loop && allowSkipToEnd && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
        {
            textRect.anchoredPosition = new Vector2(0, targetY);
        }

        // Cuộn lên
        textRect.anchoredPosition += Vector2.up * (scrollSpeed * Time.deltaTime);

        if (textRect.anchoredPosition.y >= targetY)
        {
            if (loop)
            {
                // Tạm dừng ở đỉnh (nếu cấu hình)
                if (pauseAtTopSeconds > 0f && pauseTimer <= 0f)
                {
                    pauseTimer = pauseAtTopSeconds;
                }

                if (pauseTimer > 0f)
                {
                    pauseTimer -= Time.deltaTime;
                    return; // Đợi hết pause rồi mới reset
                }

                // Reset về vị trí bắt đầu để lặp
                textRect.anchoredPosition = new Vector2(0, startY);
                // (Không tắt isScrolling để chạy vòng kế tiếp)
                // Có thể thêm: randomize startDelay nhẹ nếu muốn
            }
            else
            {
                isScrolling = false;
                Invoke(nameof(OnCreditEnd), endDelay);
            }
        }
    }

    private void OnCreditEnd()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else if (quitGameAfter)
        {
            Application.Quit();
        }
    }

    // Cho phép cập nhật nội dung lúc chạy và lặp tiếp
    public void SetContentAndRestart(string newContent, bool keepLoop = true)
    {
        creditContent = newContent;
        loop = keepLoop;

        creditText.text = creditContent;
        Canvas.ForceUpdateCanvases();

        // Recompute geometry
        switch (startPosition)
        {
            case StartPosition.Top:
                startY = viewport.rect.height / 2f;
                break;
            case StartPosition.Middle:
                startY = -(textRect.rect.height / 2f);
                break;
            default:
                startY = -(viewport.rect.height + 50f);
                break;
        }
        textRect.anchoredPosition = new Vector2(0, startY);
        targetY = textRect.rect.height + viewport.rect.height + loopGapPixels;

        startTime = Time.time + startDelay;
        isScrolling = true;
        isPaused = false;
        pauseTimer = 0f;
    }
}
