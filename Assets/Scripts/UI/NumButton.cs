using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NumButton : VuMonoBehaviour
{
    [SerializeField] private InputField inputField;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadInput();
    }
    protected override void Start()
    {
        if (inputField != null)
        {
            inputField.characterLimit = 6;
        }
    }
    private void LoadInput()
    {
        if (inputField == null)
        {
            inputField =GameObject.Find("InputField_Num").GetComponent<InputField>();
        }
    }

    public void InputNum(int num)
    {
        Debug.Log("Input");
        if (inputField != null)
        {
            string text = inputField.text;

            // Chỉ cho phép thêm nếu chưa đủ 6 ký tự
            if (text.Length < 6)
            {
                inputField.text = text + num;
            }
        }
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.clickAudio,this.transform);
    }

    public void DeleteInput()
    {
        if (inputField != null && !string.IsNullOrEmpty(inputField.text))
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.clickAudio, this.transform);

    }
}
