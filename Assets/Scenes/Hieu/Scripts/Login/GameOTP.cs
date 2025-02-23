using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using static OTP;

public class GameOYTP : MonoBehaviour
{
    public GameForgot GameForgot;
    public TMP_InputField inputOTP;
    public TMP_InputField newPassword;        
    public void OnclickOTP()
    {
        StartCoroutine(OTP());
    }
    IEnumerator OTP()
    {
        string email = GameForgot.ipemail();
        int otp = int.Parse(inputOTP.text);
        Debug.Log(email);
        OTPrequest request = new OTPrequest(
            email,
            otp,
            newPassword.text
            );
        if (email=="" || inputOTP.text==""||newPassword.text=="" )
        {
            Notification.noti_instance.Shownotification("Nhập đầy đủ thông tin");
            yield break;
        }
        string body = JsonConvert.SerializeObject(request);
        using (UnityWebRequest www = new UnityWebRequest("https://localhost:7096/api/APIGame/ResetPassword", "POST"))
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
                ResponseOTP response = JsonConvert.DeserializeObject<ResponseOTP>(json);
                if (response.isSuccess)
                {
                    Notification.noti_instance.Shownotification(response.notification);
                }
                else
                {
                    Notification.noti_instance.Shownotification(response.notification);
                }
            }
        }
    }
}
