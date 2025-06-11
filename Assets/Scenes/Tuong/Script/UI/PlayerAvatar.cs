using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAvatar : MonoBehaviour
{
    [Header("Display UI")]
    [SerializeField] private Image avatarDisplay;
    [SerializeField] private Image avatarFrameDisplay;
    [Header("Avtar sprites")]
    [SerializeField] private Image[] avatarSprites;
    [SerializeField] private Image[] avatarFrameSprites;
    [Header("Panels")]
    [SerializeField] private GameObject avatarPanel;
    [SerializeField] private GameObject avatarFramePanel;
    [SerializeField] private GameObject changeAvatarPanel;
    [Header("Buttons")]
    [SerializeField] private Button changeAvatarButton;
    [SerializeField] private Button changeAvatarFrameButton;
    private int currentAvatarIndex;
    private int currentFrameIndex;
    private int previewAvatarIndex;
    private int previewFrameIndex;
    [Header("BorderSelection")]
    [SerializeField] GameObject[] imageHighlights;
    [SerializeField] private RectTransform selectionHighlight;
    private int currentActiveIndex;
    [Header("Temporary Avtar")]
    [SerializeField] private Image temporaryAvtar;
    [SerializeField] private Image temporaryAvtarFrame;
    private void Start()
    {
        currentAvatarIndex = PlayerPrefs.GetInt("SeletedAvtar", 0);
        currentFrameIndex = PlayerPrefs.GetInt("SelectedAvatarFrame", 0);
        temporaryAvtar.sprite = avatarSprites[currentAvatarIndex].sprite;
        temporaryAvtarFrame.sprite = avatarFrameSprites[currentFrameIndex].sprite;
        previewAvatarIndex = currentAvatarIndex;
        previewFrameIndex = currentFrameIndex;

        avatarDisplay.sprite = avatarSprites[currentAvatarIndex].sprite;
        avatarFrameDisplay.sprite = avatarFrameSprites[currentFrameIndex].sprite;

        UpdateChangeButton(changeAvatarButton, previewAvatarIndex == currentAvatarIndex);
        UpdateChangeButton(changeAvatarFrameButton, previewFrameIndex == currentFrameIndex);
        for (int i = 0; i < imageHighlights.Length; i++)
        {
            imageHighlights[i].SetActive(i == 0);
        }
        currentActiveIndex = 0;
        avatarPanel.SetActive(true);
    }
    private void OnEnable()
    {
        ResertToAvatarPanel();
    }
    private void ResertToAvatarPanel()
    {
        for (int i = 0; i < imageHighlights.Length; i++)
        {
            imageHighlights[i].SetActive(i == 0);
        }
        currentActiveIndex = 0;
        avatarPanel.SetActive(true);
        avatarFramePanel.SetActive(false);
    }
    public void OnOptionButtonClicked(int index)
    {
        if (currentActiveIndex == index) return;
        if(currentActiveIndex == 0) ResetPriviewAvatar();
        else if(currentActiveIndex == 1) ResetPriviewAvatarFrame();
        imageHighlights[currentActiveIndex].SetActive(false);
        imageHighlights[index].SetActive(true);
        currentActiveIndex = index;
        avatarPanel.SetActive(index == 0);
        avatarFramePanel.SetActive(index == 1);
    }
    private void ResetPriviewAvatar()
    {
        previewAvatarIndex = currentAvatarIndex;
        temporaryAvtar.sprite = avatarSprites[previewAvatarIndex].sprite;
        UpdateChangeButton(changeAvatarButton, true);
    }
    private void ResetPriviewAvatarFrame()
    {
        previewFrameIndex = currentFrameIndex;
        temporaryAvtarFrame.sprite = avatarFrameSprites[previewFrameIndex].sprite;
        UpdateChangeButton(changeAvatarButton, true);
    }
    public void PreviewAvatar(int index)
    {
        if (index >= 0 && index < avatarSprites.Length)
        {
            previewAvatarIndex = index;
            UpdateChangeButton(changeAvatarButton, previewAvatarIndex == currentAvatarIndex);
            selectionHighlight.SetParent(avatarSprites[index].transform, false);
            selectionHighlight.DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.OutQuad);
            temporaryAvtar.sprite = avatarSprites[previewAvatarIndex].sprite;
        }
    }
    public void PreviewAvatarFrame(int index)
    {
        if (index >= 0 && index < avatarFrameSprites.Length)
        {
            previewFrameIndex = index;
            UpdateChangeButton(changeAvatarFrameButton, previewFrameIndex == currentFrameIndex);
            selectionHighlight.SetParent(avatarFrameSprites[index].transform, false);
            selectionHighlight.DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.OutQuad);
            temporaryAvtarFrame.sprite = avatarFrameSprites[previewFrameIndex].sprite;
        }
    }
    public void ChangeAvatar()
    {
        if (previewAvatarIndex != currentAvatarIndex)
        {
            currentAvatarIndex = previewAvatarIndex;
            avatarDisplay.sprite = avatarSprites[currentAvatarIndex].sprite;
            PlayerPrefs.SetInt("SeletedAvtar", currentAvatarIndex);
            UpdateChangeButton(changeAvatarButton, true);
        }
    }
    public void ChangeAvatarFrame()
    {
        if (previewFrameIndex != currentFrameIndex)
        {
            currentFrameIndex = previewFrameIndex;
            avatarFrameDisplay.sprite = avatarFrameSprites[currentFrameIndex].sprite;
            PlayerPrefs.SetInt("SelectedAvatarFrame", currentFrameIndex);
            UpdateChangeButton(changeAvatarFrameButton, true);
        }
    }
    private void UpdateChangeButton(Button button, bool isDisabled)
    {
        button.interactable = !isDisabled;
        var colors = button.colors;
        colors.normalColor = isDisabled ? Color.gray : Color.white;
        button.colors = colors;
    }
    public void OpenChangeAvatarPanel()
    {
        changeAvatarPanel.SetActive(true);
    }
    public void CloseChangeAvatarPanel()
    {
        changeAvatarPanel.SetActive(false);
    }
}
