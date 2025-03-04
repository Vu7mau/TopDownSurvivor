using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float delayTime = 0.1f;
    [SerializeField] private Vector3 offSet;
    [SerializeField] private AudioClip snd_spawn_pu;
    [SerializeField] private List<AudioClip> listSndCoinPickUp;

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
    private void Update()
    {
        
    }
    public void PlaySoundSFXPU()
    {
        SoundFXManager.Instance.PlaySoundFXClip(snd_spawn_pu, transform);
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
            SetCoin text = GameObject.Find("Coin").GetComponentInChildren<SetCoin>();
            SoundFXManager.Instance.PlaySoundFXClip(listSndCoinPickUp[Random.Range(0,listSndCoinPickUp.Count)], transform);
            text.SetCoinToUI(1);
            Debug.Log("Bạn đã thu thập được kim cương!");
        }
    }
    private IEnumerator FollowPlayerRoutine(CharacterCtrl player)
    {
        while (isFollowing && player != null)
        {
            transform.DOMove(player.transform.position + offSet, duration).SetEase(Ease.Linear);
            yield return new WaitForSeconds(delayTime);
        }
    }
    public void FollowPlayer(CharacterCtrl _player)
    {
        if (isFollowing) return;
        isFollowing = true;
        StartCoroutine(FollowPlayerRoutine(_player));
    }
}
