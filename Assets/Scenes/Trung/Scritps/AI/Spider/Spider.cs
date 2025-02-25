using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] private PoolingObjectList pool;
    [SerializeField] private List<Transform> listPosition;
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
    public void Update()
    {
        transform.localScale = new Vector3(1, 1, -1);
    }
}
