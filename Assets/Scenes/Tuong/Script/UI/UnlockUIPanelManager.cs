using UnityEngine;
using UnityEngine.UI;
public class UnlockUIPanelManager : MonoBehaviour
{
    public static UnlockUIPanelManager Instance;

    [Header("UI References")]
    public GameObject unlockPanel;
    public Button unlockButton;
    public Button closeButton;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        HideUnlockPanel();
    }

    private void Start()
    {
        if (unlockButton != null)
            unlockButton.onClick.AddListener(OnUnlockClicked);
        if (closeButton != null)
            closeButton.onClick.AddListener(HideUnlockPanel);
    }
    public void ShowUnlockPanel()
    {
        if (unlockPanel != null)
            unlockPanel.SetActive(true);
    }
    public void HideUnlockPanel()
    {
        if (unlockPanel != null)
            unlockPanel.SetActive(false);
    }
    private void OnUnlockClicked()
    {
        PlayerPrefs.SetInt(ModePanel.SURVIVE_UNLOCK_KEY, 1);
        PlayerPrefs.Save();
        HideUnlockPanel();

        ModePanel.Instance?.RefreshUIInstant();
    }
}
