using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public SlidingDoor slidingDoor;
    public GameEventManager eventManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && eventManager.IsPowerOn())
        {
            slidingDoor.OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && eventManager.IsPowerOn())
        {
            slidingDoor.CloseDoor();
        }
    }
}
