using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFade : VuMonoBehaviour
{
  
   public Material transparentMat; // dùng chung hoặc clone nếu mỗi material khác nhau
    [SerializeField] private Material[] originalMats;
    private MeshRenderer rend;
    protected override void Start()
    {
  
        rend = GetComponent<MeshRenderer>();
        originalMats = rend.materials;
        Collider col = GetComponent<Collider>();
        col.isTrigger = true; 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Material[] fadeMats = new Material[originalMats.Length];
            for (int i = 0; i < fadeMats.Length; i++)
            {
                fadeMats[i] = new Material(transparentMat); // tạo bản copy để tránh thay đổi toàn bộ
                fadeMats[i].color = originalMats[i].color; // giữ màu gốc nếu cần
                Color col = fadeMats[i].color;
                col.a = 0.3f;
                fadeMats[i].color = col;
            }
            rend.materials = fadeMats;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rend.materials = originalMats;
        }
    }
}
