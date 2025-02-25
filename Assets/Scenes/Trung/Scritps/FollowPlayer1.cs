using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer1 : MonoBehaviour
{
    private Transform playerPosition;
    private void Awake()
    {
        playerPosition = FindAnyObjectByType<CharacterCtrl>().transform;
    }
    private void Update()
    {
        transform.parent = playerPosition;
    }
}
