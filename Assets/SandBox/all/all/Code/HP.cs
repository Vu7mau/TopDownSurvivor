using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    private Transform player;
    public float attractSpeed = 5f;
    public float attractDistance = 5f;

    private bool Attracted = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= attractDistance)
        {
            Attracted = true;
        }
        if (Attracted)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, attractSpeed * Time.deltaTime);
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        PlayerData.Instance.AddHp(1);
    //        Destroy(gameObject);
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int currentHP = PlayerPrefs.GetInt("Coins", 0);
            PlayerPrefs.SetInt("HP", currentHP + 1);
            PlayerPrefs.Save(); ;
        }
    }
}
