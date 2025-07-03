using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManagerTwo : MonoBehaviour
{
    public static AudioManagerTwo Instance;
    [System.Serializable]
    public struct ButtonSFXEntry
    {
        public ButtonSFXType type;
        public AudioClip clip;
    }
    [SerializeField] AudioSource buttonSource;

    [SerializeField] private List<ButtonSFXEntry> buttonSFXList;
    private Dictionary<ButtonSFXType, AudioClip> buttonSFXDict;

    private float lastHoverSoundTime = -1f;
    [SerializeField] private float hoverSoundCooldown = 0.15f;

    private float lastAnyButtonSoundTime = -1f;
    [SerializeField] private float buttonSFXGlobalCooldown = 0.1f;
    private void Start()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        buttonSFXDict = new Dictionary<ButtonSFXType, AudioClip>();
        foreach (var entry in buttonSFXList)
        {
            if (!buttonSFXDict.ContainsKey(entry.type))
            {
                buttonSFXDict.Add(entry.type, entry.clip);
            }
        }
    }
    public void PlayButtonSFX(ButtonSFXType type)
    {
        if (Time.time - lastAnyButtonSoundTime < buttonSFXGlobalCooldown) return;
        if (type == ButtonSFXType.Hover)
        {
            if (Time.time - lastHoverSoundTime < hoverSoundCooldown) return;
            lastHoverSoundTime = Time.time;
        }
        lastAnyButtonSoundTime = Time.time;
        if (buttonSFXDict.TryGetValue(type, out var clip) && clip != null)
        {
            buttonSource.PlayOneShot(clip); 
        }
    }
}
