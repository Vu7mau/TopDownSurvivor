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

     

        switch (triggerType)
        {
            case TriggerType.DarkZone:
                eventManager.OnEnterDarkZone();
                triggered = true;
                break;
            case TriggerType.DoorZone:
                eventManager.OnEnterDoorZone();
                break;
            case TriggerType.PowerSwitch:
                eventManager.OnActivatePowerSwitch();
                triggered = true;
                break;
            case TriggerType.StartMap2:
                eventManager.StartMap2();
                triggered = true;
                break;
            case TriggerType.Map21:
                eventManager.Map21();
                triggered = true;
                break;
            case TriggerType.Map22:
                eventManager.Map22();
                triggered = true;
                break;
            case TriggerType.Map23:
                eventManager.Map23();
                triggered = true;
                break;
        }
    }
}
