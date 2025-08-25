using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class SlidingDoor : MonoBehaviour
{
    public enum DoorDirection { LeftRight, FrontBack }
    public enum DoorState { Closed, Opening, Opened, Closing }
    public enum Side { A, B } // A = cùng hướng transform.forward, B = ngược lại
    public enum AllowedSide { Any, SideA, SideB }
    public enum FirstOpenLockMode { StayOpenAndLock, CloseAndLock }

    [Header("Direction")]
    public DoorDirection direction = DoorDirection.LeftRight;
    [Tooltip("Chọn mặt nào được auto-open khi Player đi vào trigger")]
    public AllowedSide allowedAutoOpenSide = AllowedSide.Any;

    [Header("Auto Close on Exit")]
    [Tooltip("Tự đóng khi đối tượng rời trigger?")]
    public bool autoCloseOnExit = true;
    [Tooltip("Giới hạn phía được phép auto-close khi rời trigger.")]
    public AllowedSide allowedAutoCloseSide = AllowedSide.Any;

    [Header("Door Parts")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Motion")]
    public float openDistance = 2f;
    public float moveSpeed = 2f;
    [Tooltip("Ngưỡng coi như đã tới đích.")]
    public float arriveThreshold = 0.001f;

    [Header("Audio")]
    [SerializeField] private AudioClip doorOpenAudio;
    [SerializeField] private AudioClip doorLockedAudio;

    [Header("Condition")]
    [SerializeField] private bool requireCondition = false;
    [SerializeField] private bool conditionMet = false;

    [Header("One-time Lock")]
    [Tooltip("Nếu bật, cửa chỉ cho mở 1 lần. Sau khi mở lần đầu sẽ bị khoá vĩnh viễn.")]
    public bool lockAfterFirstOpen = false;
    public FirstOpenLockMode firstOpenLockMode = FirstOpenLockMode.StayOpenAndLock;
    public float lockCloseDelay = 0.5f;

    [Header("Tags/Filter")]
    [Tooltip("Chỉ những collider có Tag này mới trigger cửa.")]
    public string playerTag = "Player";

    [Header("Events (Inspector)")]
    public UnityEvent onOpenStarted;
    public UnityEvent onOpened;
    public UnityEvent onCloseStarted;
    public UnityEvent onClosed;
    public UnityEvent onOpenDenied;

    [Header("Events: Side Enter Detection")]
    public UnityEvent onEnterSideA; // vào từ phía A
    public UnityEvent onEnterSideB; // vào từ phía B

    [Header("Events: Side Exit Detection")]
    public UnityEvent onExitSideA;  // rời trigger ở phía A
    public UnityEvent onExitSideB;  // rời trigger ở phía B

    [Serializable] public class FloatEvent : UnityEvent<float> { } // 0..1 tiến độ
    [Header("Optional: tiến độ di chuyển 0..1")]
    public FloatEvent onMoveProgress;

    // C# events
    public event Action OpenStarted;
    public event Action Opened;
    public event Action CloseStarted;
    public event Action Closed;
    public event Action OpenDenied;
    public event Action<float> MoveProgress; // 0..1
    public event Action EnteredSideA;
    public event Action EnteredSideB;
    public event Action ExitedSideA;
    public event Action ExitedSideB;

    // --- Internal state ---
    private Vector3 leftClosedPos, rightClosedPos;
    private Vector3 leftOpenPos, rightOpenPos;
    private Vector3 leftTargetPos, rightTargetPos;

    private bool isMoving = false;
    private DoorState state = DoorState.Closed;

    private bool isPermanentlyLocked = false;
    private bool hasOpenedOnce = false;

    private Transform _tr;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    private void Awake()
    {
        _tr = transform;
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    private void Start()
    {
        if (!leftDoor || !rightDoor)
        {
            Debug.LogError("[SlidingDoor] Chưa gán leftDoor/rightDoor!");
            enabled = false;
            return;
        }

        leftClosedPos = leftDoor.position;
        rightClosedPos = rightDoor.position;

        Vector3 offset = (direction == DoorDirection.LeftRight)
            ? new Vector3(openDistance, 0f, 0f)
            : new Vector3(0f, 0f, openDistance);

        leftOpenPos = leftClosedPos - offset;
        rightOpenPos = rightClosedPos + offset;

        leftTargetPos = leftClosedPos;
        rightTargetPos = rightClosedPos;
        state = DoorState.Closed;
    }

    private void Update()
    {
        if (!isMoving) return;

        leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftTargetPos, moveSpeed * Time.deltaTime);
        rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightTargetPos, moveSpeed * Time.deltaTime);

        float total = Vector3.Distance(leftClosedPos, leftOpenPos);
        if (total > 0.0001f)
        {
            float cur = Vector3.Distance(leftDoor.position, leftClosedPos);
            float progress = Mathf.InverseLerp(0f, total, cur);
            onMoveProgress?.Invoke(progress);
            MoveProgress?.Invoke(progress);
        }

        if (Vector3.Distance(leftDoor.position, leftTargetPos) < arriveThreshold &&
            Vector3.Distance(rightDoor.position, rightTargetPos) < arriveThreshold)
        {
            isMoving = false;

            if (state == DoorState.Opening)
            {
                state = DoorState.Opened;
                onOpened?.Invoke(); Opened?.Invoke();

                if (!hasOpenedOnce)
                {
                    hasOpenedOnce = true;
                    if (lockAfterFirstOpen)
                    {
                        if (firstOpenLockMode == FirstOpenLockMode.CloseAndLock)
                        {
                            Invoke(nameof(CloseAndPermanentLock), lockCloseDelay);
                        }
                        else
                        {
                            isPermanentlyLocked = true;
                        }
                    }
                }
            }
            else if (state == DoorState.Closing)
            {
                state = DoorState.Closed;
                onClosed?.Invoke(); Closed?.Invoke();
            }
        }
    }

    // -------- Public API ----------
    public bool IsConditionRequired() => requireCondition;
    public bool IsConditionMet() => conditionMet;
    public DoorState CurrentState => state;
    public bool IsLockedPermanently() => isPermanentlyLocked;

    public void SetConditionMet(bool value) => conditionMet = value;
    public void Lock() => isPermanentlyLocked = true;
    public void Unlock() => isPermanentlyLocked = false;

    public void OpenDoor()
    {
        if (isPermanentlyLocked) { Denied(); return; }
        if (requireCondition && !conditionMet) { Denied(); return; }
        if (state == DoorState.Opened || state == DoorState.Opening) return;

        SafePlay(doorOpenAudio);
        SetTargetsOpen();
        isMoving = true;
        state = DoorState.Opening;

        onOpenStarted?.Invoke(); OpenStarted?.Invoke();
    }

    public void CloseDoor()
    {
        if (state == DoorState.Closed || state == DoorState.Closing) return;

        SetTargetsClosed();
        isMoving = true;
        state = DoorState.Closing;

        onCloseStarted?.Invoke(); CloseStarted?.Invoke();
    }

    public void ToggleDoor()
    {
        if (state == DoorState.Opened) CloseDoor();
        else if (state == DoorState.Closed) OpenDoor();
    }

    // -------- Trigger logic + Side detection ----------
    private void OnTriggerEnter(Collider other)
    {
        if (!IsAllowedActor(other)) return;

        var side = GetSideOf(other.transform);
        if (side == Side.A) { onEnterSideA?.Invoke(); EnteredSideA?.Invoke(); }
        else { onEnterSideB?.Invoke(); EnteredSideB?.Invoke(); }

        if (allowedAutoOpenSide == AllowedSide.Any ||
            (allowedAutoOpenSide == AllowedSide.SideA && side == Side.A) ||
            (allowedAutoOpenSide == AllowedSide.SideB && side == Side.B))
        {
            OpenDoor();
        }
        else
        {
            Denied();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsAllowedActor(other)) return;

        // Xác định đối tượng rời trigger ở phía nào (A/B) tại thời điểm Exit
        var side = GetSideOf(other.transform);
        if (side == Side.A) { onExitSideA?.Invoke(); ExitedSideA?.Invoke(); }
        else { onExitSideB?.Invoke(); ExitedSideB?.Invoke(); }

        // Tùy chọn auto-close theo phía rời
        if (!autoCloseOnExit) return;
        if (allowedAutoCloseSide == AllowedSide.Any ||
            (allowedAutoCloseSide == AllowedSide.SideA && side == Side.A) ||
            (allowedAutoCloseSide == AllowedSide.SideB && side == Side.B))
        {
            CloseDoor();
        }
    }

    // -------- Helpers ----------
    private bool IsAllowedActor(Collider other)
    {
        if (string.IsNullOrEmpty(playerTag)) return true;
        return other.CompareTag(playerTag);
    }

    public Side GetSideOf(Transform t)
    {
        Vector3 to = t.position - _tr.position;
        to.y = 0f;
        float dot = Vector3.Dot(_tr.forward.normalized, to.sqrMagnitude > 0.0001f ? to.normalized : Vector3.forward);
        return (dot >= 0f) ? Side.A : Side.B;
    }

    private void SetTargetsOpen()
    {
        leftTargetPos = leftOpenPos;
        rightTargetPos = rightOpenPos;
    }

    private void SetTargetsClosed()
    {
        leftTargetPos = leftClosedPos;
        rightTargetPos = rightClosedPos;
    }

    private void Denied()
    {
        SafePlay(doorLockedAudio);
        onOpenDenied?.Invoke();
        OpenDenied?.Invoke();
    }

    private void SafePlay(AudioClip clip)
    {
        try
        {
            if (clip != null) SoundFXManager.Instance?.PlaySoundFXClip(clip, transform);
        }
        catch { }
    }

    private void CloseAndPermanentLock()
    {
        CloseDoor();
        isPermanentlyLocked = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Transform t = transform;
        Vector3 p = t.position;
        Vector3 f = t.forward; f.y = 0f; f.Normalize();

        Gizmos.color = new Color(0f, 0.7f, 1f, 0.6f);
        Vector3 right = new Vector3(f.z, 0f, -f.x);
        Gizmos.DrawLine(p + right * 2f, p - right * 2f);

        Gizmos.color = new Color(0.2f, 1f, 0.2f, 0.8f);
        Gizmos.DrawRay(p, f * 1.5f);
        UnityEditor.Handles.Label(p + f * 1.6f, "Side A (forward)");

        Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.8f);
        Gizmos.DrawRay(p, -f * 1.0f);
        UnityEditor.Handles.Label(p - f * 1.2f, "Side B (back)");
    }
#endif
}
