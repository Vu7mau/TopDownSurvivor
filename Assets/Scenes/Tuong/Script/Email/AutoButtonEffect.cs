using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AutoButtonEffect : TuongMonobehaviour
{
    [SerializeField] private GameObject buttonParent;
    private string getButtonParent = "LoginGame";
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip buttonHoverClip;

    [SerializeField] private float cooldown = 0.1f;
    private float lastPlayTime;
    void Start()
    {
        Auto();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGameObject();
    }
    protected virtual void LoadGameObject()
    {
        if(buttonParent == null) buttonParent = LoadGameObject(buttonParent, getButtonParent);
    }
    private void Auto()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        Button[] buttons = buttonParent.GetComponentsInChildren<Button>(true);
        foreach (Button btn in buttons)
        {
            if(btn.CompareTag("IgnoreButtonEffect")) continue;
            if (btn.GetComponent<ButtonClickEffectDOTween>() == null)
                btn.gameObject.AddComponent<ButtonClickEffectDOTween>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                PlayClickSound();
            });
            AddHoverSound(btn);
        }
    }
    private void AddHoverSound(Button btn)
    {
        EventTrigger eventTrigger = btn.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = btn.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) =>
        {
            if (Time.time - lastPlayTime > cooldown)
            {
                PlayHoverSound();   
                lastPlayTime = Time.time;
            }
        });
        eventTrigger.triggers.Add(entry);
    }
    void PlayClickSound()
    {
        if (buttonClickClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickClip);
        }
    }
    void PlayHoverSound()
    {
        if (buttonHoverClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonHoverClip);
        }
    }
}
