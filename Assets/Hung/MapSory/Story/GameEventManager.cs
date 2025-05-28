using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public GameObject lightObject;
    public DialogManager dialogManager;

    public void OnEnterDarkZone()
    {
        lightObject.SetActive(false);
        dialogManager.ShowDialog("Khu vực này đã bị mất điện, cần phải khởi động lại nguồn điện!");
    }

    public void OnEnterDoorZone()
    {
        dialogManager.ShowDialog("Cần nguồn điện để mở cửa!");
    }

    public void OnActivatePowerSwitch()
    {
        lightObject.SetActive(true);
        dialogManager.ShowDialog("Nguồn điện đã được khởi động lại!");
    }
}
