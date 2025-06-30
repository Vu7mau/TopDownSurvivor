using UnityEngine;

public class OptionButtonManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonHighlight
    {
        public GameObject[] highlights;
    }
    [SerializeField] private ButtonHighlight[] buttonHighlights;
    private int currentActiveIndex;
    private void Start()
    {
        for (int i = 0; i < buttonHighlights.Length; i++)
            SetGroupActive(i, i == 0);
        currentActiveIndex = 0;
    }
    public void OnOptionButtonClicked(int index)
    {
        if (currentActiveIndex == index) return;
        SetGroupActive(currentActiveIndex, false);
        SetGroupActive(index, true);

        currentActiveIndex = index;
    }
    private void SetGroupActive(int groupIndex, bool isActive)
    {
        foreach (var go in buttonHighlights[groupIndex].highlights)
            go.SetActive(isActive);
    }
}
