using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class TuongMonobehaviour : MonoBehaviour
{
    private void Awake()
    {
        LoadComponents();
    }
    private void Reset()
    {
        LoadComponents();
    }
    protected virtual void LoadComponents()
    {

    }
    protected virtual TMP_InputField LoadTMPInputField(TMP_InputField tMP_InputField, string name)
    {
        if (tMP_InputField != null) return tMP_InputField;
        return GameObject.Find(name).GetComponent<TMP_InputField>();
    }
    protected virtual GameObject LoadGameObject(GameObject gameObject, string name)
    {
        if (gameObject != null) return gameObject;
        return GameObject.Find(name);
    }
}
