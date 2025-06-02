using UnityEngine;
using UnityEngine.UI;
public class AutoButtonEffect : TuongMonobehaviour
{
    [SerializeField] private GameObject buttonParent;
    private string getButtonParent = "LoginGame";
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip buttonClickClip;
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
            if (btn.GetComponent<ButtonClickEffectDOTween>() == null)
                btn.gameObject.AddComponent<ButtonClickEffectDOTween>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                PlayClickSound();
            });
        }
    }
    void PlayClickSound()
    {
        if (buttonClickClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickClip);
        }
    }
}
