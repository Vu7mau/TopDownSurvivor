using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UserData;

public class RegisterPlayerPrefs : MonoBehaviour
{
    public TMP_InputField NameInput;
    public TMP_InputField EmailInput;
    public TMP_InputField PassWordInput;    
    public void InputRegister()
    {        
        if (NameInput.text == ""||EmailInput.text=="")
        {
            Notification.noti_instance.Shownotification("Vui lòng nhập đầy đủ thông tin");
            return;
        }
        if(PassWordInput.text.Length < 5)
        {
            Notification.noti_instance.Shownotification("Mật khẩu trên 5 kí tự");
            return;
        }
        if (!EmailInput.text.EndsWith("@gmail.com"))
        {
            Notification.noti_instance.Shownotification("Email không đúng định dạng");
            return;
        }
        string jsonstring = PlayerPrefs.GetString("Register");
        data datas = JsonUtility.FromJson<data>(jsonstring);
        dataEntry dataEntrys = datas.listdata.FirstOrDefault(x => x.name == NameInput.text || x.email == EmailInput.text);
        if (dataEntrys != null)
        {
            Notification.noti_instance.Shownotification("email hoặc tên tài khoản đã tồn tại");
            return;
        }
        addInput(NameInput.text,EmailInput.text,PassWordInput.text);
    }        
    public void addInput(string name,string email,string password)
    {        
        string jsonstring = PlayerPrefs.GetString("Register", "{}"); 
        data datas = JsonUtility.FromJson<data>(jsonstring);        
        if (datas == null)
        {
            datas = new data();
        }                        
        dataEntry entry = new dataEntry { name = name, email = email, password = password };                
        datas.listdata.Add(entry);        
        string json = JsonUtility.ToJson(datas);
        PlayerPrefs.SetString("Register", json);
        PlayerPrefs.Save();        
        Debug.Log("Dữ liệu đã lưu: " + PlayerPrefs.GetString("Register"));
        Notification.noti_instance.Shownotification("Đăng kí thành công");
    }  
}
