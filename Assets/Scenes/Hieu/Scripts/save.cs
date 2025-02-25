using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.UI;

public class save : MonoBehaviour
{
    private SkinnedMeshRenderer CurrentMaterial;
    public Material[] skinPlayer;
    private string currentSkin;
    private static save Instance;
    private GameObject Player;
    public Button[] buttons;
    private void Awake()
    {
        Player = GameObject.Find("Character_Bandit_Male_011");
    }
    private void Start()
    {
        if (Player != null)
        {
            CurrentMaterial = Player.GetComponent<SkinnedMeshRenderer>();
            if (PlayerPrefs.HasKey("sk"))
            {
                string savedMaterial = PlayerPrefs.GetString("sk").Replace("(Instance)", "").Trim();                
                for (int i = 0; i <skinPlayer.Length; i++)
                {
                    string MaterialName = skinPlayer[i].name.Trim();                    
                    if (MaterialName == savedMaterial)
                    {
                        Debug.Log("MaterialCurrent");
                        CurrentMaterial.material = skinPlayer[i];
                        break;
                    }
                }
            }
        }
        for (int i = 0; i < skinPlayer.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => changSkin(index));
        }
    }    
    private void changSkin(int indext)
    {
        CurrentMaterial.material = skinPlayer[indext];
        SaveSkin(CurrentMaterial.material.name);
    }
    private void SaveSkin(string name)
    {
        PlayerPrefs.SetString("sk",name);
        PlayerPrefs.Save();
    }
}
