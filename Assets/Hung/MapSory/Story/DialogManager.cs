using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;

    private void Start()
    {
        dialogPanel.SetActive(false);
    }

    public void ShowDialog(string message)
    {
        dialogPanel.SetActive(true);
        dialogText.text = message;

        // Tự tắt sau 3 giây
        CancelInvoke(nameof(HideDialog));
        Invoke(nameof(HideDialog), 3f);
    }

    public void HideDialog()
    {
        dialogPanel.SetActive(false);
    }
}
