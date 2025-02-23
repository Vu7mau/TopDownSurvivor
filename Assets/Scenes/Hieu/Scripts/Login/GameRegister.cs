using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using static RegisterData;
using UnityEngine.Networking;
using System;
using System.Security.Cryptography;
public class GameRegister : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField nameInput;
    public TMP_InputField passwordInput;    
    public GameObject pn_Login;
    public void OnclickButton()
    {
        StartCoroutine(Register());
    }
    private IEnumerator Register()
    {
        RegisterRequestData request = new RegisterRequestData(
            emailInput.text,
            passwordInput.text,
            nameInput.text,
            ""
            );              
        if (emailInput.text ==""||passwordInput.text==""||nameInput.text=="")
        {
            Notification.noti_instance.Shownotification("Nhập đầy đủ thông tin");
            yield break;
        }
        string body = JsonUtility.ToJson(request);
        using (UnityWebRequest www = new UnityWebRequest("https://localhost:7096/api/APIGame/Register", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            if (www.error!=null)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string json = www.downloadHandler.text;           
                ResponseUserSuccess response = JsonConvert.DeserializeObject<ResponseUserSuccess>(json);
                Debug.Log(response.isSuccess);
                var data = response.data;
                if (response.isSuccess==true)
                {
                    Notification.noti_instance.Shownotification(response.notification);
                    pn_Login.SetActive(true);
                    gameObject.SetActive(false);
                }
                else
                {
                    Notification.noti_instance.Shownotification("Đăng kí thất bại");
                }
            }
        }        
    }
}
