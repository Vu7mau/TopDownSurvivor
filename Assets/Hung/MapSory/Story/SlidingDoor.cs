using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    public Vector3 leftOpenOffset = new Vector3(-2f, 0f, 0f);
    public Vector3 rightOpenOffset = new Vector3(2f, 0f, 0f);
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
        leftTargetPos = leftClosedPos + leftOpenOffset;
        rightTargetPos = rightClosedPos + rightOpenOffset;
        isMoving = true;
    }

    public void CloseDoor()
    {
        leftTargetPos = leftClosedPos;
        rightTargetPos = rightClosedPos;
        isMoving = true;
    }
}
