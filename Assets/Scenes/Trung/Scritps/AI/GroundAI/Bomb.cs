using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        CharacterCtrl player = other.gameObject.GetComponent<CharacterCtrl>();
        if(player != null)
        {
            gameObject.SetActive(false);
            Debug.Log("Đã chạm Player!");
        }
    }
}
