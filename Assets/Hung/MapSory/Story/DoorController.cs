using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform door;
    public float moveDistance = 3f;
    public float moveSpeed = 2f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isOpening = false;

    private void Start()
    {
        initialPosition = door.position;
        targetPosition = initialPosition + Vector3.up * moveDistance;
    }

    public void OpenDoor()
    {
        if (!isOpening)
        {
            isOpening = true;
            StartCoroutine(MoveDoorUp());
        }
    }

    private System.Collections.IEnumerator MoveDoorUp()
    {
        while (Vector3.Distance(door.position, targetPosition) > 0.01f)
        {
            door.position = Vector3.MoveTowards(door.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        door.position = targetPosition;
    }
}
