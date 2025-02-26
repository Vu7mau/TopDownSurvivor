using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.UI;

public class save : MonoBehaviour
{
    private SkinnedMeshRenderer CurrentMaterial;
    public Material[] skinPlayer;        
    //private GameObject Player;
    public Button[] buttons;
    private void Awake()
    {
        CurrentMaterial = GameObject.Find("Character_Bandit_Male_011").GetComponent<SkinnedMeshRenderer>();               
    }
    private void Start()
    {
        if (CurrentMaterial != null)
        {            
            if (PlayerPrefs.HasKey("sk"))
            {                
                CurrentMaterial.material = skinPlayer[PlayerPrefs.GetInt("sk")];              
            }
        }        
        for (int i = 0; i < skinPlayer.Length; i++)
        {
            if (buttons.Length>0)
            {
                int index = i;
                buttons[i].onClick.AddListener(() => changSkin(index));
            }
        }
    }    
    private void changSkin(int indext)
    {
        CurrentMaterial.material = skinPlayer[indext];        
        SaveSkin(indext);
    }
    private void SaveSkin(int index)
    {
        PlayerPrefs.SetInt("sk",index);
        PlayerPrefs.Save();
    }
}
