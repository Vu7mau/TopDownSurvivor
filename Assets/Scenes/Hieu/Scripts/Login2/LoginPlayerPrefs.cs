using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UserData;

public class LoginPlayerPrefs : MonoBehaviour
{
    public TMP_InputField InputEmail;
    public TMP_InputField InputPassword;
    public void LoginLick()
    {
        if (InputEmail.text != "" || InputPassword.text != "")
        {
            string stringJson = PlayerPrefs.GetString("Register");
            data datas = JsonUtility.FromJson<data>(stringJson);
            Debug.Log(datas);
            dataEntry dataEntry = datas.listdata.FirstOrDefault(x => x.email == InputEmail.text && x.password == InputPassword.text);
            if (dataEntry != null)
            {
                Notification.noti_instance.Shownotification("Đăng nhập thành công");
                PlayerPrefs.SetString("Name", dataEntry.name);
                PlayerPrefs.SetInt("LastGameCoin", 0);
                StartCoroutine(cd());
            }
            else
            {
                Notification.noti_instance.Shownotification("Tài khoản hoặc mật khẩu không chính xác");
            }
        }
        else
        {
            Notification.noti_instance.Shownotification("Điền đủ thông tin");
        }
    }
    IEnumerator cd()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }
}
