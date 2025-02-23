using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private Transform Player;
    private Transform MiniIconPlayer;        
    private void Awake()
    {
        MiniIconPlayer = GameObject.Find("MiniIconPlayer").transform;
        Player = GameObject.Find("Character").transform;
    }
    private void LateUpdate()
    {        
        MiniIconPlayer.position = new Vector3(Player.position.x, 20, Player.position.z);
        transform.position = new Vector3(Player.position.x, transform.position.y, Player.position.z);
    }
}
