using UnityEngine;

public class save : MonoBehaviour
{
    private SkinnedMeshRenderer CurrentMaterial;
    public Material[] skinPlayer;
    private string currentSkin;
    private static save Instance;
    private GameObject Player;
    private void Awake()
    {
        Player = GameObject.FindWithTag("Skin");
    }
    private void Start()
    {
        if (Player != null)
        {
            CurrentMaterial = Player.GetComponent<SkinnedMeshRenderer>();
            if (PlayerPrefs.HasKey("sk"))
            {
                string savedMaterial = PlayerPrefs.GetString("sk").Replace("(Instance)", "").Trim();
                for (int i = 0; i < 2; i++)
                {
                    string MaterialName = skinPlayer[i].name.Trim();
                    Debug.Log($"🔍 Comparing:\n- Saved: '{savedMaterial}'\n- Material: '{skinPlayer[i].name}'");
                    if (MaterialName == savedMaterial)
                    {
                        Debug.Log("MaterialCurrent");
                        CurrentMaterial.material = skinPlayer[i];
                        break;
                    }
                }
            }
        }
    }
    private void SwapMaterial()
    {
        if (CurrentMaterial != null)
        {
            Material Temp = skinPlayer[0];
            skinPlayer[0] = skinPlayer[1];
            skinPlayer[1] = Temp;
        }

        CurrentMaterial.material = skinPlayer[0];
    }
    public void skin1()
    {
        SwapMaterial();
        CurrentSkin(CurrentMaterial.material.name);
        Debug.Log("Current ; " + CurrentMaterial.material.name);
    }
    public void skin2()
    {
        SwapMaterial();
        CurrentSkin(CurrentMaterial.material.name);
    }
    private void CurrentSkin(string Current)
    {
        PlayerPrefs.SetString("sk", Current);
        PlayerPrefs.Save();
    }
}
