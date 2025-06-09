using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
public class CharacterInformation : MonoBehaviour
{
    public static CharacterInformation Instance;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI characterId;
    private void Start()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
        LoadOrFetCharacterInfo();
    }
    private void LoadOrFetCharacterInfo()
    {
        string savedName = PlayerPrefs.GetString("Name", "");
        string savedId = PlayerPrefs.GetString("Id", "");

        if(!string.IsNullOrEmpty(savedName) || !string.IsNullOrEmpty(savedId))
        {
            characterName.text = "Name: " + savedName;
            characterId.text = "ID: " + savedId;
        }
        if(PlayFabClientAPI.IsClientLoggedIn())
        {
            ShowCharacters();
        }
        else if (string.IsNullOrEmpty(savedName) && string.IsNullOrEmpty(savedId))
        {
            characterName.text = "Vui lòng đăng nhập để xem thông tin";
            characterId.text = "";
        }
    }
    public void ShowCharacters()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            string displayName = result.AccountInfo.TitleInfo.DisplayName;
            characterName.text = "Name: " + displayName;
            string userID = result.AccountInfo.PlayFabId;
            characterId.text = "ID: " + userID;
            PlayerPrefs.SetString("Name", displayName);
            PlayerPrefs.SetString("Id", userID);

        }, error =>
        {
            characterName.text = "Lỗi khi lấy tên người dùng: " + error.GenerateErrorReport();
        });
    }
}
