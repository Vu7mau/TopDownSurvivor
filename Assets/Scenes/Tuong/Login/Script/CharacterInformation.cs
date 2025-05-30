using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
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
        ShowCharacters();
    }
    public void ShowCharacters()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            string displayName = result.AccountInfo.TitleInfo.DisplayName;
            characterName.text = "Name: " + displayName;
            string userID = result.AccountInfo.PlayFabId;
            characterId.text = "ID: " + userID;

        }, error =>
        {
            characterName.text = "Lỗi khi lấy tên người dùng: " + error.GenerateErrorReport();
        });
    }
}
