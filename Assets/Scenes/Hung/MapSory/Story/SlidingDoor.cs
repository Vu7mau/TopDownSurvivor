using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public enum DoorDirection { LeftRight, FrontBack }
    public DoorDirection direction = DoorDirection.LeftRight;

    public Transform leftDoor;
    public Transform rightDoor;

    public float openDistance = 2f;
    public float moveSpeed = 2f;
    [SerializeField] private AudioClip doorOpenAudio;

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
        if (isMoving)
        {
            leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftTargetPos, moveSpeed * Time.deltaTime);
            rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightTargetPos, moveSpeed * Time.deltaTime);
        }
    }

    public void OpenDoor()
    {
        Vector3 offset;
        if (doorOpenAudio != null)
        {
            SoundFXManager.Instance.PlaySoundFXClip(doorOpenAudio, this.transform);
        } 
        // Chọn hướng mở
        if (direction == DoorDirection.LeftRight)
            offset = new Vector3(openDistance, 0f, 0f); // X
        else
            offset = new Vector3(0f, 0f, openDistance); // Z

        // Một cánh đi -offset, một cánh đi +offset
        leftTargetPos = leftClosedPos - offset;
        rightTargetPos = rightClosedPos + offset;

        isMoving = true;
    }

    public void CloseDoor()
    {
        leftTargetPos = leftClosedPos;
        rightTargetPos = rightClosedPos;
        isMoving = true;
    }
}
