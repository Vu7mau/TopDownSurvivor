using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public GameObject lightObject;
    public DialogManager dialogManager;
    public SlidingDoor slidingDoor;
    public PowerStartupSequence powerStartupSequence;

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
            dialogManager.ShowDialog("Cần khởi động lại nguồn điện");
        }
    }

    public void OnActivatePowerSwitch()
    {
        if (!powerIsOn && powerStartupSequence != null)
        {
            powerStartupSequence.StartSequence();
        }
    }

    public void FinishPowerActivation()
    {
        powerIsOn = true;
        lightObject.SetActive(true);
    }

    public void StartMap2()
    {
        dialogManager.ShowDialog("Có nguyên một khu vực bí mật ở dưới lòng đất. Tìm hiểu xem có gì dưới đây");
    }

    public void Map21()
    {
        dialogManager.ShowDialog("Máy tính cần mật khẩu, hãy tìm kiếm xung quanh");
    }

    public void Map22()
    {
        dialogManager.ShowDialog("Cổng không gian? Họ đang nghiên cứu cái gì đây vậy?");
    }

    public void Map23()
    {
        dialogManager.ShowDialog("Một con robot nằm chắn lối ra");
    }
}
