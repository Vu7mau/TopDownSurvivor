using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class QuestPasswordCondition : VuMonoBehaviour
{
    [SerializeField] private bool isSuccess = false;
    [SerializeField] private int password;

    [SerializeField] private InputField passwordInput;
    [SerializeField] private TMP_Text log;
    [SerializeField] private Transform passwordInputPanel;

    [SerializeField] private SlidingDoor SlidingDoor;

    private int failedAttempts = 0;
    [SerializeField] private int maxFailedAttempts = 3;

    [SerializeField] private AudioClip incorrectAudio;
    [SerializeField] private AudioClip correctAudio;


    public int DoorPassword => password;
    protected override void LoadComponents()
    {
        LoadInput();
        LoadSlidingDoor();
    }
    private void LoadInput()
    {
        if (passwordInput == null)
        {
          passwordInput = GameObject.Find("InputField_Num").GetComponent<InputField>();
        }

    }
    protected override void OnEnable()
    {
      passwordInput.onValueChanged.AddListener((value) => CheckPassCondition(value));
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    private void LoadSlidingDoor()
    {
        if(SlidingDoor == null)
            SlidingDoor=this.GetComponent<SlidingDoor>();
    }
    private int GeneratePass()
    {
        return Random.Range(100000, 1000000);
    }
    protected override void Awake()
    {
        base.Awake();
       password = GeneratePass();
    }
    private void CheckPassCondition(string _)
    {
        if (passwordInput.text.Length < 6)
            return;

        if (int.TryParse(passwordInput.text, out int pass))
        {
            if (pass == password)
            {
                if (correctAudio != null)
                    SoundFXManager.Instance.PlaySoundFXClip(correctAudio, this.transform);
                isSuccess = true;
                SlidingDoor.OpenDoor();
                passwordInputPanel.gameObject.SetActive(false);
            }
            else
            {
                if (incorrectAudio != null)
                    SoundFXManager.Instance.PlaySoundFXClip(incorrectAudio, this.transform);
                failedAttempts++;
                log.text = $"Sai mật khẩu! Lần thử: {failedAttempts}";

                passwordInputPanel.transform.DOKill();
                passwordInputPanel.transform.DOShakePosition(
                    0.4f, new Vector3(20f, 0, 0), 20, 90f, false, true
                );

                passwordInput.text = "";

                if (failedAttempts >= maxFailedAttempts)
                {
                    OnMaxFailedAttempts();
                }
            }
        }
    }

    protected virtual void OpenPasswordInput()
    {
        if (passwordInput == null) return;
        passwordInputPanel.gameObject.SetActive(true);
        passwordInputPanel.localScale = Vector3.zero;

        // Hiệu ứng popup scale
        passwordInputPanel.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack); // Mượt và bật nẩy nhẹ
    }
    protected virtual void ClosePasswordInput()
    {
        if (passwordInputPanel == null) return;

        passwordInputPanel.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.InBack)
            .OnComplete(() => passwordInputPanel.gameObject.SetActive(false));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!isSuccess)
        {
            OpenPasswordInput();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ClosePasswordInput();
    }
    private void OnMaxFailedAttempts()
    {
        log.text = "Sai quá nhiều lần! Cửa đã khoá.";
       
    }
}
