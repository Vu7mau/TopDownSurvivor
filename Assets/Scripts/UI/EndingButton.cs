using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if TMP_PRESENT
using TMPro;
#endif

public class EndingButton : MonoBehaviour
{
    public enum ActionType { QuitGame, LoadSceneByIndex }

    [Header("Action")]
    public ActionType action = ActionType.QuitGame;
    [Tooltip("Chỉ dùng khi Action = LoadSceneByIndex")]
    public int sceneIndex = 0;

    [Header("Optional: UI Label (không bắt buộc)")]
    [Tooltip("Nếu có, script sẽ tự set text cho label theo Action")]
    public Text legacyText;
#if TMP_PRESENT
    public TMP_Text tmpText;
#endif

    [Header("Input Shortcut (tuỳ chọn)")]
    public bool triggerOnEnterOrSpace = false; // nhấn Enter/Space cũng kích hoạt
    public bool triggerOnEscape = false;       // nhấn Esc để Quit (chỉ khi Action=QuitGame)

    [Header("Safety")]
    [Tooltip("Bật để log cảnh báo nếu sceneIndex không hợp lệ.")]
    public bool logWarnings = true;

    private Button _btn;

    void Reset()
    {
        _btn = GetComponent<Button>();
        if (_btn == null) _btn = gameObject.AddComponent<Button>();
    }

    void Awake()
    {
        _btn = GetComponent<Button>();
        if (_btn != null)
        {
            _btn.onClick.RemoveAllListeners();
            _btn.onClick.AddListener(Trigger);
        }
        UpdateLabel();
    }

    void Update()
    {
        if (triggerOnEnterOrSpace &&
            (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
        {
            Trigger();
        }

        if (triggerOnEscape && action == ActionType.QuitGame && Input.GetKeyDown(KeyCode.Escape))
        {
            Trigger();
        }
    }

    /// <summary>Gọi hàm này khi muốn kích hoạt hành động (OnClick của Button cũng trỏ vào đây).</summary>
    public void Trigger()
    {
        switch (action)
        {
            case ActionType.QuitGame:
                QuitGame();
                break;
            case ActionType.LoadSceneByIndex:
                LoadSceneByIndex();
                break;
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        // Trong Editor: dừng Play Mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void LoadSceneByIndex()
    {
        // Kiểm tra hợp lệ
        if (sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            if (logWarnings)
                Debug.LogWarning($"[EndingButton] sceneIndex {sceneIndex} không hợp lệ. " +
                                 $"BuildSettings hiện có {SceneManager.sceneCountInBuildSettings} scene.");
            return;
        }

        SceneManager.LoadScene(sceneIndex);
    }

    private void UpdateLabel()
    {
        string label = action == ActionType.QuitGame ? "Quit" : $"Load Scene {sceneIndex}";
        if (legacyText) legacyText.text = label;
#if TMP_PRESENT
        if (tmpText) tmpText.text = label;
#endif
    }

    // Cho phép đổi action/scene lúc runtime (nếu muốn)
    public void SetActionQuit() { action = ActionType.QuitGame; UpdateLabel(); }
    public void SetActionLoadIndex(int index) { action = ActionType.LoadSceneByIndex; sceneIndex = index; UpdateLabel(); }
}
