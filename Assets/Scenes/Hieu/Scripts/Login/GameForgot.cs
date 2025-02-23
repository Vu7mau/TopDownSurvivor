using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using static ForgotPassword;

public class GameForgot : MonoBehaviour
{
    public TMP_InputField inputemail;   
    private string email;
    public GameObject pn_OTP;
    public GameObject pn_Login;
    public void OnclickForgot()
    {
        StartCoroutine(Forgotpassword());
    }
    IEnumerator Forgotpassword()
    {        
        ForgotRequestData request = new ForgotRequestData(
            ipemail()
        );
        if (inputemail.text =="")
        {
            Notification.noti_instance.Shownotification("Vui lòng nhập email");
            yield break;
        }
        string body = JsonConvert.SerializeObject(request);                
        using (UnityWebRequest www = new UnityWebRequest("https://localhost:7096/api/APIGame/Forgotpassword", "POST"))
        {
            byte[] bodyraw = System.Text.Encoding.UTF8.GetBytes( body );
            www.uploadHandler = new UploadHandlerRaw( bodyraw );            
            www.downloadHandler = new DownloadHandlerBuffer();            
            www.SetRequestHeader( "Content-Type", "application/json" );
            yield return www.SendWebRequest();
            if (www.error != null)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string json = www.downloadHandler.text;
                ResponseForgot response = JsonConvert.DeserializeObject<ResponseForgot>( json );
                if (response.isSuccess)
                {
                    Debug.Log(response.isSuccess);
                    Notification.noti_instance.Shownotification(response.notification);
                    pn_OTP.SetActive(true );
                    gameObject.SetActive(false);
                }
                else
                {
                    Notification.noti_instance.Shownotification(response.notification);
                }
            }
        }
    }   
    public string ipemail()
    {
        string em = inputemail.text;
        email = em;
        return email;
    }
}
