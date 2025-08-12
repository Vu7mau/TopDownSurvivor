using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public enum DoorDirection { LeftRight, FrontBack }
    public DoorDirection direction = DoorDirection.LeftRight;

    [Header("Door Parts")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Motion")]
    public float openDistance = 2f;
    public float moveSpeed = 2f;

    [Header("Audio")]
    [SerializeField] private AudioClip doorOpenAudio;
    [SerializeField] private AudioClip doorLockedAudio; // âm báo khi chưa đủ điều kiện

    [Header("Condition")]
    [SerializeField] private bool requireCondition = false; // nếu bật, cửa chỉ mở khi conditionMet = true
    [SerializeField] private bool conditionMet = false;

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private Vector3 leftTargetPos;
    private Vector3 rightTargetPos;
    private bool isMoving = false;

    void Start()
    {
        leftClosedPos = leftDoor.position;
        rightClosedPos = rightDoor.position;
        leftTargetPos = leftClosedPos;
        rightTargetPos = rightClosedPos;
    }

    void Update()
    {
        if (!isMoving) return;

        leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftTargetPos, moveSpeed * Time.deltaTime);
        rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightTargetPos, moveSpeed * Time.deltaTime);

        // Dừng khi đã tới đích
        if (Vector3.Distance(leftDoor.position, leftTargetPos) < 0.001f &&
            Vector3.Distance(rightDoor.position, rightTargetPos) < 0.001f)
        {
            isMoving = false;
        }
    }

    /// <summary>
    /// Gọi từ QuestPCTrigger hoặc nơi khác để set điều kiện.
    /// </summary>
    public void SetConditionMet(bool value)
    {
        conditionMet = value;
    }

    public bool IsConditionRequired() => requireCondition;
    public bool IsConditionMet() => conditionMet;

    public void OpenDoor()
    {
        // Nếu cần điều kiện mà chưa đạt -> không mở
        if (requireCondition && !conditionMet)
        {
            if (doorLockedAudio != null)
                SoundFXManager.Instance.PlaySoundFXClip(doorLockedAudio, this.transform);
            return;
        }

        Vector3 offset = (direction == DoorDirection.LeftRight)
            ? new Vector3(openDistance, 0f, 0f)
            : new Vector3(0f, 0f, openDistance);

        if (doorOpenAudio != null)
            SoundFXManager.Instance.PlaySoundFXClip(doorOpenAudio, this.transform);

        leftTargetPos = leftClosedPos - offset; // cánh trái đi -offset
        rightTargetPos = rightClosedPos + offset; // cánh phải đi +offset
        isMoving = true;    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (requireCondition && conditionMet)
        {
            Vector3 offset = (direction == DoorDirection.LeftRight)
           ? new Vector3(openDistance, 0f, 0f)
           : new Vector3(0f, 0f, openDistance);

            if (doorOpenAudio != null)
                SoundFXManager.Instance.PlaySoundFXClip(doorOpenAudio, this.transform);

            leftTargetPos = leftClosedPos - offset; // cánh trái đi -offset
            rightTargetPos = rightClosedPos + offset; // cánh phải đi +offset
            isMoving = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") )return;

        if (requireCondition && conditionMet)
        {
            CloseDoor();
        }
    }
 
    public void CloseDoor()
    {
        leftTargetPos = leftClosedPos;
        rightTargetPos = rightClosedPos;
        isMoving = true;
    }
}
