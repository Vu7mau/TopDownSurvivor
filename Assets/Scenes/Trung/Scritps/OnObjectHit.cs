using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnObjectHit : MonoBehaviour
{
    private Rigidbody rb;
    private void Awake()
    {
        rb= GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision other)
    {
        CharacterCtrl player = other.gameObject.GetComponent<CharacterCtrl>();
        BulletCtrl bullet = other.gameObject.GetComponent<BulletCtrl>();
        if (player != null || bullet!= null)
        {
            gameObject.isStatic = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        CharacterCtrl player = other.gameObject.GetComponent<CharacterCtrl>();
        BulletCtrl bullet = other.gameObject.GetComponent<BulletCtrl>();
        if (player != null || bullet != null)
        {
            gameObject.isStatic = false;
        }
    }
    public void OnHurt()
    {
        gameObject.isStatic = true;
    }
    public void OffHurt()
    {
        gameObject.isStatic = false;
    }
}
