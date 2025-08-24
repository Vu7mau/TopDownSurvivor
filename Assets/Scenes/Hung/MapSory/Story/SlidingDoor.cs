using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class SlidingDoor : MonoBehaviour
{
    public enum DoorDirection { LeftRight, FrontBack }
    public enum DoorState { Closed, Opening, Opened, Closing }
    public enum Side { A, B } // A = phía trước (cùng hướng transform.forward), B = phía sau
    public enum AllowedSide { Any, SideA, SideB }
    public enum FirstOpenLockMode { StayOpenAndLock, CloseAndLock } // ở lại mở, hoặc đóng lại rồi khoá

    [Header("Direction")]
    public DoorDirection direction = DoorDirection.LeftRight;  // hướng trượt của cánh
    [Tooltip("Chọn mặt nào được auto-open khi Player đi vào trigger")]
    public AllowedSide allowedAutoOpenSide = AllowedSide.Any;

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
    [SerializeField] private AudioClip doorLockedAudio; // âm báo khi bị từ chối (lock/điều kiện)

    [Header("Condition")]
    [SerializeField] private bool requireCondition = false; // cần điều kiện mới mở
    [SerializeField] private bool conditionMet = false;

    [Header("One-time Lock")]
    [Tooltip("Nếu bật, cửa chỉ cho mở 1 lần. Sau khi mở lần đầu sẽ bị khoá vĩnh viễn.")]
    public bool lockAfterFirstOpen = false;
    [Tooltip("Cách khoá sau lần mở đầu: ở lại mở hoặc tự đóng rồi khoá.")]
    public FirstOpenLockMode firstOpenLockMode = FirstOpenLockMode.StayOpenAndLock;
    [Tooltip("Độ trễ trước khi tự đóng rồi khoá (nếu chọn CloseAndLock).")]
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

    [Header("Events: Side Detection")]
    public UnityEvent onEnterSideA; // bước vào trigger từ phía A (trước cửa)
    public UnityEvent onEnterSideB; // bước vào trigger từ phía B (sau cửa)

    [Serializable] public class FloatEvent : UnityEvent<float> { } // 0..1 tiến độ
    [Header("Optional: tiến độ di chuyển 0..1")]
    public FloatEvent onMoveProgress;

    // C# events (nếu muốn đăng ký bằng code)
    public event Action OpenStarted;
    public event Action Opened;
    public event Action CloseStarted;
    public event Action Closed;
    public event Action OpenDenied;
    public event Action<float> MoveProgress; // 0..1
    public event Action EnteredSideA;
    public event Action EnteredSideB;

    // --- Internal state ---
    private Vector3 leftClosedPos, rightClosedPos;
    private Vector3 leftOpenPos, rightOpenPos;
    private Vector3 leftTargetPos, rightTargetPos;

    private bool isMoving = false;
    private DoorState state = DoorState.Closed;

    private bool isPermanentlyLocked = false; // bị khoá vĩnh viễn (do lockAfterFirstOpen)
    private bool hasOpenedOnce = false;       // đã từng mở lần đầu

    // cache: mặt cửa sử dụng transform.forward để phân biệt A/B
    private Transform _tr;

    // -------------------- Unity --------------------
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

        // Tính sẵn vị trí mở theo hướng trượt
        Vector3 offset = (direction == DoorDirection.LeftRight)
            ? new Vector3(openDistance, 0f, 0f)
            : new Vector3(0f, 0f, openDistance);

        leftOpenPos = leftClosedPos - offset;   // cánh trái đi -offset
        rightOpenPos = rightClosedPos + offset; // cánh phải đi +offset

        // mặc định là đóng
        leftTargetPos = leftClosedPos;
        rightTargetPos = rightClosedPos;
        state = DoorState.Closed;
    }

    private void Update()
    {
        if (!isMoving) return;

        leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftTargetPos, moveSpeed * Time.deltaTime);
        rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightTargetPos, moveSpeed * Time.deltaTime);

        // tiến độ 0..1 theo cánh trái so với vị trí đóng/mở
        float total = Vector3.Distance(leftClosedPos, leftOpenPos);
        if (total > 0.0001f)
        {
            float cur = Vector3.Distance(leftDoor.position, leftClosedPos);
            float progress = Mathf.InverseLerp(0f, total, cur);
            onMoveProgress?.Invoke(progress);
            MoveProgress?.Invoke(progress);
        }

        // tới đích
        if (Vector3.Distance(leftDoor.position, leftTargetPos) < arriveThreshold &&
            Vector3.Distance(rightDoor.position, rightTargetPos) < arriveThreshold)
        {
            isMoving = false;

            if (state == DoorState.Opening)
            {
                state = DoorState.Opened;
                onOpened?.Invoke(); Opened?.Invoke();

                // Lần đầu mở -> đánh dấu/khóa theo cấu hình
                if (!hasOpenedOnce)
                {
                    hasOpenedOnce = true;
                    if (lockAfterFirstOpen)
                    {
                        if (firstOpenLockMode == FirstOpenLockMode.CloseAndLock)
                        {
                            // đóng lại rồi khoá
                            Invoke(nameof(CloseAndPermanentLock), lockCloseDelay);
                        }
                        else
                        {
                            // ở trạng thái mở nhưng khoá vĩnh viễn (không mở lần 2 vì đã mở rồi)
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

    // -------------------- Public API --------------------
    public bool IsConditionRequired() => requireCondition;
    public bool IsConditionMet() => conditionMet;
    public DoorState CurrentState => state;
    public bool IsLockedPermanently() => isPermanentlyLocked;

    /// <summary>Gọi từ nơi khác để set điều kiện (quest/trigger...)</summary>
    public void SetConditionMet(bool value) => conditionMet = value;

    /// <summary>Khoá vĩnh viễn (không cho mở nữa).</summary>
    public void Lock() => isPermanentlyLocked = true;

    /// <summary>Mở khoá vĩnh viễn.</summary>
    public void Unlock() => isPermanentlyLocked = false;

    public void OpenDoor()
    {
        // từ chối nếu lock vĩnh viễn
        if (isPermanentlyLocked)
        {
            Denied();
            return;
        }

        // cần điều kiện mà chưa đạt
        if (requireCondition && !conditionMet)
        {
            Denied();
            return;
        }

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

    /// <summary>Đảo trạng thái, tôn trọng lock/điều kiện.</summary>
    public void ToggleDoor()
    {
        if (state == DoorState.Opened) CloseDoor();
        else if (state == DoorState.Closed) OpenDoor();
    }

    // -------------------- Trigger logic + Side detection --------------------
    private void OnTriggerEnter(Collider other)
    {
        if (!IsAllowedActor(other)) return;

        // phát hiện bên
        Side side = GetSideOf(other.transform);
        if (side == Side.A) { onEnterSideA?.Invoke(); EnteredSideA?.Invoke(); }
        else { onEnterSideB?.Invoke(); EnteredSideB?.Invoke(); }

        // auto-open theo cấu hình side
        if (allowedAutoOpenSide == AllowedSide.Any ||
            (allowedAutoOpenSide == AllowedSide.SideA && side == Side.A) ||
            (allowedAutoOpenSide == AllowedSide.SideB && side == Side.B))
        {
            OpenDoor();
        }
        else
        {
            // không đúng phía cho mở
            Denied();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsAllowedActor(other)) return;

        // bạn có thể đổi thành chỉ auto-close nếu ra cùng phía đã vào
        CloseDoor();
    }

    // -------------------- Helpers --------------------
    private bool IsAllowedActor(Collider other)
    {
        if (string.IsNullOrEmpty(playerTag)) return true;
        return other.CompareTag(playerTag);
    }

    /// <summary>Phân biệt bên theo dấu dot(transform.forward, vector tới đối tượng).</summary>
    public Side GetSideOf(Transform t)
    {
        Vector3 to = t.position - _tr.position;
        to.y = 0f; // bỏ chiều cao
        float dot = Vector3.Dot(_tr.forward.normalized, to.normalized);
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
            if (clip != null) SoundFXManager.Instance?.PlaySoundFXClip(clip, this.transform);
        }
        catch { /* ignore nếu SoundFXManager chưa setup */ }
    }

    private void CloseAndPermanentLock()
    {
        CloseDoor();
        isPermanentlyLocked = true;
    }

#if UNITY_EDITOR
    // Vẽ gizmo hiển thị Side A/B
    private void OnDrawGizmosSelected()
    {
        Transform t = transform;
        Vector3 p = t.position;
        Vector3 f = t.forward; f.y = 0f; f.Normalize();

        // Mặt phân chia A/B
        Gizmos.color = new Color(0f, 0.7f, 1f, 0.6f);
        Vector3 right = new Vector3(f.z, 0f, -f.x);
        Gizmos.DrawLine(p + right * 2f, p - right * 2f);

        // Hướng A
        Gizmos.color = new Color(0.2f, 1f, 0.2f, 0.8f);
        Gizmos.DrawRay(p, f * 1.5f);
        UnityEditor.Handles.Label(p + f * 1.6f, "Side A (forward)");

        // Hướng B
        Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.8f);
        Gizmos.DrawRay(p, -f * 1.0f);
        UnityEditor.Handles.Label(p - f * 1.2f, "Side B (back)");
    }
#endif
}
