using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    public static Notification noti_instance;
    private TextMeshProUGUI notificationText;
    private void Awake()
    {
        notificationText = GetComponent<TextMeshProUGUI>();
        if (noti_instance == null)
        {
            noti_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Shownotification(string message)
    {
        if (notificationText != null)
        {
            notificationText.text = message;
            StartCoroutine(destroyText());
        }
    }
    IEnumerator destroyText()
    {
        yield return new WaitForSeconds(2.5f);
        notificationText.text = string.Empty;
    }
}
