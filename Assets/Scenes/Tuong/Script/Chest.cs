using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject[] itemPrefab;   
    public Transform spawnPoint;       
    public float spawnRadius = 5f;     
    public int itemCount = 10;         
    private bool isOpened = false;
    private Animator anim;
    public float attractSpeed = 3f;
    public float flyUpduration = 1f;
    public float flyUpSpeed = 2f;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOpened && other.CompareTag("Player"))
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        isOpened = true;
        Debug.Log("Mở hòm thành công");
        anim.SetTrigger("Open");
        StartCoroutine(SpawnItemsAfterDelay(1.5f));
    }

    IEnumerator SpawnItemsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < itemCount; i++)
        {
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        if (itemPrefab.Length > 0)
        {
            //Vector3 spawnPosition = spawnPoint.position; /*+ new Vector3(*/
            //    Random.Range(-spawnRadius, spawnRadius),   
            //    spawnPoint.position.y,                      
            //    Random.Range(-spawnRadius, spawnRadius)    
            //);
            Vector3 spawnPosition = spawnPoint.position + new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0f,  
            Random.Range(-spawnRadius, spawnRadius)
            );
            Vector3 targetPosition = spawnPoint.position + Vector3.up * flyUpSpeed; 
            //Vector3 targetPosition = spawnPoint + Vector3.up * flyUpSpeed;

            int randomIndex = Random.Range(0, itemPrefab.Length);
            GameObject item = Instantiate(itemPrefab[randomIndex], spawnPosition, Quaternion.identity);

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //rb.AddForce(new Vector3(Random.Range(-1f, 1f), 2f, Random.Range(-1f, 1f)) * 2f, ForceMode.Impulse);
                rb.isKinematic = true;
            }
            StartCoroutine(FlyUpThenAttract(item));
        }
    }
    IEnumerator FlyUpThenAttract(GameObject item)
    {
        Vector3 initialPosition = item.transform.position;
        Vector3 targetPosition = initialPosition + Vector3.up * flyUpSpeed;
        float elapsedTime = 0f;
        while (elapsedTime < flyUpduration)
        {
            float t = elapsedTime / flyUpduration;
            float smoothSteep = Mathf.SmoothStep(0f, 1f, t);
            item.transform.position = Vector3.Lerp(initialPosition, targetPosition, smoothSteep/*elapsedTime / flyUpduration*/);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        item.transform.position = targetPosition;
        StartCoroutine(AttractItem(item));  
    }
    IEnumerator AttractItem(GameObject item)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) yield break;
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        while (Vector3.Distance(item.transform.position, player.transform.position) > 1f)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, player.transform.position, attractSpeed * Time.deltaTime);

            yield return null;
        }
        Destroy(item);
    }
}
