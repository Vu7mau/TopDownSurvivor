using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] private PoolingObjectList pool;
    [SerializeField] private List<Transform> listPosition;
    [SerializeField] private List<AudioClip> clipSpiderSFX;

    private void Start()
    {
        AppearSFX();
    }
    private void AppearSFX()
    {
        AudioSource t = gameObject.AddComponent<AudioSource>();
        t.clip = clipSpiderSFX[0];
        t.playOnAwake = false;
        t.loop = false;
        t.Play();
        t.volume = 2f;
    }
    public void Attack2()
    {
        GameObject tinySpider = pool.GetPoolingObject();
        int randomPosition = Random.Range(0, listPosition.Count);
        if (tinySpider != null)
        {
            tinySpider.gameObject.SetActive(true);
            tinySpider.transform.position = listPosition[randomPosition].position;
        }
    }
    public void Attack3SFX()
    {
        SoundFXManager.Instance.PlaySoundFXClip(clipSpiderSFX[1],transform);
    }
    public void Update()
    {
        transform.localScale = new Vector3(1, 1, -1);
    }
}
