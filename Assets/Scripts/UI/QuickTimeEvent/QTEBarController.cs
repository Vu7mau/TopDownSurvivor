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

    [Header("Trigger")]
    [SerializeField] private KeyCode triggerKey = KeyCode.E;
    [SerializeField] private int maxFailAttempts = 3;

    [Header("Safe Zone Logic")]
    [SerializeField] private float shrinkFactor = 0.8f;
    [SerializeField] private Vector2 moveRange = new Vector2(150, 40);

    [Header("QTE Time")]
    [SerializeField] private float duration = 6f;

    private Coroutine qteCoroutine;
    private int currentFails = 0;

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player")) return;

        if (qteCoroutine == null)
        {
            qteCoroutine = StartCoroutine(HandleQTE());
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
    }

    private IEnumerator HandleQTE()
    {

        currentFails = 0;
        qteCanvas.enabled = true;

        pointer.ResetState(); // 🎯 reset tất cả
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

        if (success)
        {
            pointer.PlaySuccessEffect(audioSource, successClip);
            pointer.ShrinkSafeZoneWidthOnly(shrinkFactor, moveRange.x); // 💡 chỉ co chiều ngang
            pointer.IncreaseSpeed();
            pointer.StartQTE();
        }
        else
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
        }
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
