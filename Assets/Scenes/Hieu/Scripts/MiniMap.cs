using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private Transform Player;
    [SerializeField] private Transform MiniIconPlayer;
    [SerializeField] private Transform MiniIconEnemy;
    private GameObject[] Enemy;
    private void Awake()
    {
        Player = GameObject.Find("Character").transform;
    }
    private void LateUpdate()
    {
        Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        MiniIconPlayer.position = new Vector3(Player.position.x, 20, Player.position.z);
        transform.position = new Vector3(Player.position.x, transform.position.y, Player.position.z);
    }
}
