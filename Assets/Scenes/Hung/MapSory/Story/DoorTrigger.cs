using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public SlidingDoor slidingDoor;
    public GameEventManager eventManager;

    [Header("Cửa này có yêu cầu nguồn điện không?")]
    public bool requirePower = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!requirePower || eventManager.IsPowerOn())
        {
            slidingDoor.OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!requirePower || eventManager.IsPowerOn())
        {
            slidingDoor.CloseDoor();
        }
    }
}
