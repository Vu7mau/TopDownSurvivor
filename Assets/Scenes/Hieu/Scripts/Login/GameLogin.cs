using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static Login;
public class GameLogin : MonoBehaviour
{
    public TMP_InputField loginInput;
    public TMP_InputField passwordInput;    
    public void OnclickLogin()
    {
        StartCoroutine(_Login());
    }
    private IEnumerator _Login()
    {        
        RequestLoginData loginData = new RequestLoginData(
            loginInput.text,
            passwordInput.text
            );
        if ( loginInput.text==""||passwordInput.text=="")
        {
            Notification.noti_instance.Shownotification("Nhập đầy đủ thông tin");
            yield break;
        }
        string body = JsonConvert.SerializeObject(loginData);
        using (UnityWebRequest www = new UnityWebRequest("https://localhost:7096/api/APIGame/Login", "POST"))
        {
            byte[] bodyraw = System.Text.Encoding.UTF8.GetBytes(body);
            www.uploadHandler = new UploadHandlerRaw(bodyraw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            if (www.error != null)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string json = www.downloadHandler.text;
                ResponseLogin response = JsonConvert.DeserializeObject<ResponseLogin>(json);
                var data = response.data;
                if (response.isSuccess)
                {
                    PlayerPrefs.SetString("Name",data.name);
                    PlayerPrefs.SetString("Id",data.id);
                    PlayerPrefs.SetInt("LastGameCoin", 0);
                    Notification.noti_instance.Shownotification("Đăng nhập thành công");
                    StartCoroutine(cd());
                }
                else
                {
                    Notification.noti_instance.Shownotification("Email hoặc mật khẩu không chính xác");
                }
            }
        }
    }
    IEnumerator cd()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }
}
