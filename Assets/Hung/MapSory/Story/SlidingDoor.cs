using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public enum DoorDirection { Horizontal, Vertical }
    public DoorDirection direction = DoorDirection.Horizontal;

    public Transform leftDoor;
    public Transform rightDoor;
    public float openDistance = 2f;
    public float moveSpeed = 2f;

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

        if (direction == DoorDirection.Horizontal)
            offset = new Vector3(openDistance, 0f, 0f);
        else
            offset = new Vector3(0f, openDistance, 0f);

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
