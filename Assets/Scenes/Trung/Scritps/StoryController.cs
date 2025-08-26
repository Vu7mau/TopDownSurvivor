using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryController : VuMonoBehaviour
{
    [SerializeField] protected EnemyAIController enemyAI;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.CheckLastEnemiesDeath();
    }

    protected virtual void CheckLastEnemiesDeath()
    {
        StartCoroutine(this.CheckLastEnemiesDeathRoutine());
    }

    IEnumerator CheckLastEnemiesDeathRoutine()
    {
        if (enemyAI != null)
        {
            EnemyHealth enemyHealth = enemyAI.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                yield return new WaitUntil(() => enemyHealth.Health <= 0);

                //do something
                this.UnlockSurvivalMode();

            }
        }
    }

    protected virtual void UnlockSurvivalMode()
    {
        Debug.Log("Đã mở chế độ Sinh tồn!");
        ModeUnlockManager.UnlockSurviveMode();
        ModePanel.Instance?.RefreshUIInstant();
    }
}
