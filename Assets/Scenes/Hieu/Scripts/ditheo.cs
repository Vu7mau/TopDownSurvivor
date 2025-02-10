using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ditheo : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject player;
    private Animator animatorComponent;
    [SerializeField] private int hpEnemy;    
    private string CurrentAni = "wound";
    //private float lifeAtack1 = 5f;    
    private bool enemyAttack = false;
    [SerializeField] private float damageEnemy;
    private float cdAttack1 = 13f;
    [SerializeField] private float DistanceAttack1 = 10f;
    [SerializeField] private float DistanceAttack2 = 2f;
    [SerializeField] private float speed_enemy = 3.5f;
    [SerializeField] private float speed_enemy_at = 8f;
    private bool isdeath = false;
    private void Start()
    {
        player =GameObject.FindGameObjectWithTag("Player");        
        agent = GetComponent<NavMeshAgent>();
        animatorComponent = GetComponent<Animator>();
    }
    void Update()
    {                
        agent.SetDestination(player.transform.position);        
        dichuyen();
        if (Vector3.Distance(transform.position, player.transform.position)<=DistanceAttack1&&isdeath==false)
        {
            transform.LookAt(player.transform.position, Vector3.up);
            attack();
        }
        if (hpEnemy <= 0)
        {            
            isdeath = true;
            agent.isStopped = true;            
            ani("death", true);                        
            Destroy(gameObject, 5f);
        }

    }       
    private void dichuyen()
    {
        if (enemyAttack == false&&isdeath==false)
        {
            transform.LookAt(player.transform.position, Vector3.up);
            ani("wound", true);
            if (cdAttack1 < 13)
            {
                cdAttack1 += Time.deltaTime;                
            }            
        }        
    }        
    private void attack() 
    {       
        if (cdAttack1 >=13)
        {
            enemyAttack = true;
            agent.speed = speed_enemy_at;            
            ani("atack1",true);               
            StartCoroutine(CountDown());            
        }
        else if (Vector3.Distance(transform.position, player.transform.position) <= DistanceAttack2)
        {
            enemyAttack = true;                                
            ani("atack3", true);            
            enemyAttack = false;                            
        }        
    }    
    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(5);            
        //lifeAtack1 = 0;
        cdAttack1 = 0;
        enemyAttack = false;        
        agent.speed = speed_enemy;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            hpEnemy -= 2;
            Debug.Log(hpEnemy);            
        }
    }    
    private void ani(string ani,bool an)
    {
        if (CurrentAni != ani)
        {
            animatorComponent.SetBool(CurrentAni, false);
            CurrentAni = ani;
            animatorComponent.SetBool(ani, an);
        }
    }
}
