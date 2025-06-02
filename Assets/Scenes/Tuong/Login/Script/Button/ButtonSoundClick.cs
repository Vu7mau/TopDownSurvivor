using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
public class ButtonSoundClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AudioSource audioSource;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GetComponent<Button>() != null && audioSource != null)
        {
            audioSource.Play();
        }
    }
}
