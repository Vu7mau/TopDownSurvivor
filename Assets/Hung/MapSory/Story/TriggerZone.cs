using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public enum TriggerType { DarkZone, DoorZone, PowerSwitch, StartMap2, Map21, Map22 , Map23}
    public TriggerType triggerType;

    public GameEventManager eventManager;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;

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
            case TriggerType.StartMap2:
                eventManager.StartMap2();
                break;
            case TriggerType.Map21:
                eventManager.Map21();
                break;
            case TriggerType.Map22:
                eventManager.Map22();
                break;
            case TriggerType.Map23:
                eventManager.Map23();
                break;
        }
    }
}
