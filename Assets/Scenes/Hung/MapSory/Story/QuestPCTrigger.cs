using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class QuestPCTrigger : VuMonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private SlidingDoor targetDoor;
    [SerializeField] private ChatDialogue dialogueUI;
    [SerializeField] private PlayableDirector timeLine;
    [SerializeField] private CinemachineVirtualCamera cam;

    public enum CameraOffMode { LowerPriority, DisableComponent, DisableGameObject }

    [Header("Camera Off")]
    [SerializeField] private bool turnOffCameraAfterTimeline = true;
    [SerializeField] private float delayAfterTimeline = 2f; // đợi x giây rồi tắt cam
    [SerializeField] private CameraOffMode cameraOffMode = CameraOffMode.LowerPriority;
    [SerializeField] private int targetPriorityAfterOff = 0; // dùng khi LowerPriority

    [Header("Dialogue")]
    [TextArea][SerializeField] private string unlockedMessage = "Cửa phụ đã được mở khoá";
    [SerializeField] private float messageDuration = 2.0f;
    [SerializeField] private AudioClip dialogueSfx;
    [SerializeField] private bool waitDialogueBeforeTimeline = true; // nếu true: đợi hết messageDuration rồi mới Play timeline

    [Header("Behavior")]
    [SerializeField] private bool autoOpenDoor = true; // MỞ CỬA SAU KHI TIMELINE KẾT THÚC
    [SerializeField] private bool oneTime = true;
    [SerializeField] private float cooldown = 1.0f;
    [SerializeField] private string playerTag = "Player";

    private bool triggered = false;
    private float lastTriggerTime = -999f;
    private Collider triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        if (triggerCollider == null)
            Debug.LogWarning($"{name}: QuestPCTrigger cần một Collider đặt IsTrigger = true.");
        else
            triggerCollider.isTrigger = true;
    }

    private bool CanTrigger()
    {
        if (oneTime) return !triggered;
        return Time.time - lastTriggerTime >= cooldown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (!CanTrigger()) return;

        lastTriggerTime = Time.time;
        if (oneTime) triggered = true;

        // Đánh dấu điều kiện cửa đã đạt (cửa chỉ mở SAU timeline nếu autoOpenDoor = true)
        if (targetDoor != null)
            targetDoor.SetConditionMet(true);

        // Hiện thông báo
        if (dialogueUI != null)
            dialogueUI.ShowDialogue(unlockedMessage, messageDuration, dialogueSfx, "Hệ thống", null,DialogueAnchor.Bottom);

        // Chờ thoại (optional) -> Play timeline -> chờ timeline xong -> mở cửa -> chờ vài giây -> tắt cam
        StartCoroutine(RunSequence());

        // Không tắt GameObject trigger; nếu oneTime thì chỉ tắt collider
        if (oneTime && triggerCollider != null)
            triggerCollider.enabled = false;
    }

    private IEnumerator RunSequence()
    {
        // 1) (Tuỳ chọn) đợi thoại xong
        if (waitDialogueBeforeTimeline && messageDuration > 0f)
            yield return new WaitForSeconds(messageDuration);

        // 2) Play timeline và đợi kết thúc
        yield return StartCoroutine(PlayTimelineAndWait());

        // 3) Sau khi timeline kết thúc mới mở cửa
        if (autoOpenDoor && targetDoor != null)
            targetDoor.OpenDoor();

        // 4) Đợi thêm vài giây rồi tắt camera
        if (turnOffCameraAfterTimeline && cam != null)
        {
            if (delayAfterTimeline > 0f)
                yield return new WaitForSeconds(delayAfterTimeline);

            TurnOffCamera();
        }
    }

    private IEnumerator PlayTimelineAndWait()
    {
        if (timeLine == null) yield break;

        bool finished = false;

        // dọn trước rồi đăng ký
        timeLine.stopped -= OnTimelineStopped;
        timeLine.stopped += OnTimelineStopped;

        if (timeLine.state != PlayState.Playing)
            timeLine.Play();

        while (!finished)
        {
            if (timeLine == null) break;
            if (timeLine.state != PlayState.Playing) break;
            yield return null;
        }

        void OnTimelineStopped(PlayableDirector director)
        {
            finished = true;
            director.stopped -= OnTimelineStopped;
        }
    }

    private void TurnOffCamera()
    {
        switch (cameraOffMode)
        {
            case CameraOffMode.LowerPriority:
                cam.Priority = targetPriorityAfterOff;
                break;

            case CameraOffMode.DisableComponent:
                cam.enabled = false;
                break;

            case CameraOffMode.DisableGameObject:
                cam.gameObject.SetActive(false);
                break;
        }
    }
}

