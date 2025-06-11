using UnityEngine;

public class OptionsPanel : MonoBehaviour
{
    OptionButtonManager optionButtonManager;
    [Header("Panels")]
    [SerializeField] private GameObject[] panels;
    private void Start()
    {
        optionButtonManager = FindObjectOfType<OptionButtonManager>();
    }
    public void OpenPanel(int index)
    {
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }
    }
}
