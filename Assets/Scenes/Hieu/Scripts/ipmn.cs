using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ipmn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("start");
        transform.localScale = transform.localScale * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        transform.localScale = transform.localScale / 1.1f;
        Debug.Log("Exit");

    }
}
