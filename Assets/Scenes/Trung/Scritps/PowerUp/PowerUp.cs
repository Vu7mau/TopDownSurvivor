using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float delayTime = 0.1f;
    [SerializeField] private Vector3 offSet;

    private Rigidbody rb;
    private bool isFollowing =false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        isFollowing = false;
        rb.isKinematic = false;
        CharacterCtrl player = FindAnyObjectByType<CharacterCtrl>();
        FollowPlayer(player);
        //CollectPowerUp.Instance.AddPowerUpToArea(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        //Nếu va chạm với mặt đất thì nằm trên mặt đất
        if (other.GetComponent<GroundInstrcution>() != null)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            Debug.Log("Đã va chạm với ground!");
        }

        ////Nếu va chạm với người chơi thì xóa powerup
        CharacterCtrl player = other.GetComponent<CharacterCtrl>();
        if (player != null)
        {
            gameObject.SetActive(false);
            Debug.Log("Bạn đã thu thập được kim cương!");
        }
    }
    private IEnumerator FollowPlayerRoutine(CharacterCtrl player)
    {
        while(isFollowing && player != null)
        {
            transform.DOMove(player.transform.position + offSet, duration).SetEase(Ease.Linear);
            yield return new WaitForSeconds(delayTime);
        }
    }
    public void FollowPlayer(CharacterCtrl _player)
    {
        if (isFollowing) return;
        isFollowing=true;
        StartCoroutine(FollowPlayerRoutine(_player));
    }
}
