using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public enum TriggerType { DarkZone, DoorZone, PowerSwitch }
    public TriggerType triggerType;

    public GameEventManager eventManager;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {

            triggered = true;
            

            switch (triggerType)
            {
                case TriggerType.DarkZone:
                    eventManager.OnEnterDarkZone();
                    break;
                case TriggerType.DoorZone:
                    eventManager.OnEnterDoorZone();
                    break;
                case TriggerType.PowerSwitch:
                    eventManager.OnActivatePowerSwitch();
                    break;
            }
        }
    }
}
