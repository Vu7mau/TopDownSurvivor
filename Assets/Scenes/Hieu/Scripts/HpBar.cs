using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public static HpBar Instance;
 [SerializeField]  private Slider SLider_Hpbar;
    private Coroutine currentCoroutine;
    private Menu Menu;
    bool isDead=false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;


        SLider_Hpbar = GetComponent<Slider>();
    }
    private void Start()
    {
      
        Menu = FindObjectOfType<Menu>();

    }
    protected virtual void LateUpdate()
    {
        if (0 >= SLider_Hpbar.value&&!isDead)
        {
            Menu.die();
            isDead = true;
        }
    }
    public void SetHealth(float deductHp, float currentHp)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);

        }
        float tarGetHp=currentHp-deductHp;
        currentCoroutine = StartCoroutine(smoothHealth(tarGetHp, currentHp));
    }
    private IEnumerator smoothHealth(float deductHp, float currentHp)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        float startHp = currentHp;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SLider_Hpbar.value = Mathf.Lerp(startHp, deductHp, elapsed / duration);
            yield return null;
        }
        //SLider_Hpbar.value = deductHp; 
        
    }
    public virtual void SetHealthMaxBarVolume(float maxVolume)
    {
        SLider_Hpbar.maxValue = maxVolume;
        SLider_Hpbar.value = maxVolume;
    }
}
