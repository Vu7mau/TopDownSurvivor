using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class QTEBarController : VuMonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Canvas qteCanvas;
    [SerializeField] private PointerController pointer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successClip;
    [SerializeField] private AudioClip failClip;
    [SerializeField] private QTEResultPopup resultPopup;
   



    [Header("Trigger")]
    [SerializeField] private KeyCode triggerKey = KeyCode.E;
    [SerializeField] private int maxFailAttempts = 3;

    [Header("Safe Zone Logic")]
    [SerializeField] private float shrinkFactor = 0.8f;
    [SerializeField] private Vector2 moveRange = new Vector2(150, 40);

    [Header("QTE Time")]
    [SerializeField] private float duration = 6f;
    [SerializeField] private float increaseSpeed = 1f;

    private Coroutine qteCoroutine;
    private int currentFails = 0;
    private QuestPasswordCondition quest;
    private List<char> revealedDigits = new();
    private bool isQteCompleted = false;
    int successCount = 0;
    protected override void Start()
    {
        if (quest == null)
            quest = GameObject.FindObjectOfType<QuestPasswordCondition>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player")) return;
        if (isQteCompleted)
        {
           
            if (resultPopup != null)
                resultPopup.Show(quest.DoorPassword.ToString());
        }
        else
        {
            if (qteCoroutine == null)
            {
                qteCoroutine = StartCoroutine(HandleQTE());
            }
        }
       
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Input.GetKeyDown(triggerKey) && qteCoroutine == null)
        {
            qteCoroutine = StartCoroutine(HandleQTE());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StopQTE();
        resultPopup.HidePopup();
        CharacterCtrl.Instance.CharacterShooting.SetCancel(false);
    }

    private IEnumerator HandleQTE()
    {

        CharacterCtrl.Instance.CharacterShooting.SetCancel(true);
        currentFails = 0;
        revealedDigits.Clear();
        pointer.logPass.text = "";

        qteCanvas.enabled = true;


       
        pointer.ResetState();
        pointer.StartQTE();
        pointer.OnQTEResult += OnQTEResult;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Debug.Log("QTE Timeout!");
        StopQTE(); 
    }

    private void OnQTEResult(bool success)
    {
        string fullPass = quest.DoorPassword.ToString();


        if (revealedDigits.Count >= fullPass.Length)
        {
         
            StopQTE();
            return;
        }

        if (!success)
        {
            currentFails++;
            pointer.PlayFailEffect(audioSource, failClip);

            if (currentFails >= maxFailAttempts)
            {
                Debug.Log("QTE failed too many times!");
                StopQTE();
            }
            else
            {
                pointer.StartQTE();
            }
            return;
        }

        // Thành công
        pointer.PlaySuccessEffect(audioSource, successClip);
        pointer.ShrinkSafeZoneWidthOnly(shrinkFactor, moveRange.x);
        pointer.IncreaseSpeed(increaseSpeed);

        revealedDigits.Add(fullPass[revealedDigits.Count]);
        pointer.logPass.text = string.Join(" ", revealedDigits);

        // Kiểm tra lại sau khi thêm
        if (revealedDigits.Count >= fullPass.Length)
        {
            StopQTE();
            if (resultPopup != null)
                resultPopup.Show(quest.DoorPassword.ToString());
            isQteCompleted = true;
            Debug.Log("Người chơi đã lấy đủ mật khẩu!");
            return;
        }

        pointer.StartQTE();
    }

    private void StopQTE()
    {
        if (qteCoroutine != null)
        {
            StopCoroutine(qteCoroutine);
            qteCoroutine = null;
        }

        pointer.StopQTE();
        pointer.OnQTEResult -= OnQTEResult;
        qteCanvas.enabled = false;

    }
}
