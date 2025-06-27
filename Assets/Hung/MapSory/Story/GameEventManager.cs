using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public GameObject lightObject;
    public DialogManager dialogManager;
    public SlidingDoor slidingDoor;

    private bool powerIsOn = false;

    public bool IsPowerOn() => powerIsOn;

    public void OnEnterDarkZone()
    {
        if (!powerIsOn)
        {
            lightObject.SetActive(false);
            dialogManager.ShowDialog("Khu vực này đã bị mất điện, cần phải khởi động lại nguồn điện!");
        }
        else
        {
            lightObject.SetActive(true);
        }
    }

    public void OnEnterDoorZone()
    {
        if (powerIsOn)
        {
            slidingDoor.OpenDoor();
        }
        else
        {
            dialogManager.ShowDialog("Cần nguồn điện để mở cửa!");
        }
    }

    public void OnActivatePowerSwitch()
    {
        if (!powerIsOn)
        {
            powerIsOn = true;
            lightObject.SetActive(true);
            dialogManager.ShowDialog("Nguồn điện đã được khởi động lại!");
            //Debug.Log("Điện đã bật");
        }
    }
}
