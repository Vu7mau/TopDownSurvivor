using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHovering = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        Debug.Log("Chuột đang di vào nút!");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        Debug.Log("Chuột đã rời khỏi nút!");
    }

    public bool IsHovering()
    {
        return isHovering;
    }
}
