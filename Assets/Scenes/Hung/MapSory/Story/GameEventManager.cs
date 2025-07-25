using PlayFab.EconomyModels;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public GameObject lightObject;
    public DialogManager dialogManager;
    public SlidingDoor slidingDoor;
    public PowerStartupSequence powerStartupSequence;

    [SerializeField] private AudioClip notificationAudi;

    private bool powerIsOn = false;

    public bool IsPowerOn() => powerIsOn;

    public void OnEnterDarkZone()
    {
        if (!powerIsOn)
        {
            lightObject.SetActive(false);
            string content = "Khu vực này đã bị mất điện, cần phải khởi động lại nguồn điện!";
            ChatDialogueManager.Instance.chatDialogue.ShowDialogue(content, 7, notificationAudi);
            BackgroundMusicManager.Instance.PlayMusic(BackgroundMusicManager.Instance.musicClip_2);
            // dialogManager.ShowDialog("Khu vực này đã bị mất điện, cần phải khởi động lại nguồn điện!");
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
            string content = "Cần khởi động lại nguồn điện";
            //dialogManager.ShowDialog("Cần khởi động lại nguồn điện");
            ChatDialogueManager.Instance.chatDialogue.ShowDialogue(content, 7, notificationAudi);

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
        //dialogManager.ShowDialog("Có nguyên một khu vực bí mật ở dưới lòng đất. Tìm hiểu xem có gì dưới đây");
        string content = "Có nguyên một khu vực bí mật ở dưới lòng đất. Tìm hiểu xem có gì dưới đây";

        ChatDialogueManager.Instance.chatDialogue.ShowDialogue(content, 7, notificationAudi);

    }

    public void Map21()
    {
        //dialogManager.ShowDialog("Máy tính cần mật khẩu, hãy tìm kiếm xung quanh");
        string content = "Máy tính cần mật khẩu, hãy tìm kiếm xung quanh";

        ChatDialogueManager.Instance.chatDialogue.ShowDialogue(content, 7, notificationAudi);
    }

    public void Map22()
    {
        //dialogManager.ShowDialog("Cổng không gian? Họ đang nghiên cứu cái gì đây vậy?");

        string content = "Cổng không gian? Họ đang nghiên cứu cái gì đây vậy?";
        ChatDialogueManager.Instance.chatDialogue.ShowDialogue(content, 7, notificationAudi);
    }

    public void Map23()
    {
        //dialogManager.ShowDialog("Một con robot nằm chắn lối ra");
        string content = "Một con robot nằm chắn lối ra";
        ChatDialogueManager.Instance.chatDialogue.ShowDialogue(content, 7, notificationAudi);
    }
}
