using UnityEngine;
using UnityEngine.UI;
public class OptionButtonManager : MonoBehaviour
{
    [SerializeField] GameObject[] imageHighlights;
    private int currentActiveIndex;
    private void Start()
    {
        for (int i = 0; i < imageHighlights.Length; i++)
        {
            imageHighlights[i].SetActive(i == 0); 
        }
        currentActiveIndex = 0;
    }
    public void OnOptionButtonClicked(int index)
    {
        if (currentActiveIndex == index) return; 
        imageHighlights[currentActiveIndex].SetActive(false);
        imageHighlights[index].SetActive(true);
        currentActiveIndex = index;
    }
}
