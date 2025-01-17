using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHitEnemy : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;
    private void OnTriggerEnter(Collider other)
    {
        //Nếu va chạm với người chơi thì người chơi bị trừ máu
        Debug.Log(other.gameObject.name + " đã dính đòn Attack!");
    }
}
