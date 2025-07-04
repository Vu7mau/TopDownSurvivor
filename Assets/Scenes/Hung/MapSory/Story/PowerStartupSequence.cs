using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerStartupSequence : MonoBehaviour
{
    public float startupTime = 60f;
    public Slider countdownSlider;
    public GameEventManager gameEventManager;
    public List<MonsterSpawnerTrigger> monsterSpawners;

    private bool isRunning = false;

    private void Start()
    {
        if (countdownSlider != null)
            countdownSlider.gameObject.SetActive(false);
    }

    public void StartSequence()
    {
        if (isRunning) return;

        StartCoroutine(RunStartup());
    }

    private IEnumerator RunStartup()
    {
        isRunning = true;

        gameEventManager.dialogManager.ShowDialog("Hệ thống đang khởi động, vui lòng chờ 1 phút...");

        if (countdownSlider != null)
        {
            countdownSlider.gameObject.SetActive(true);
            countdownSlider.maxValue = startupTime;
            countdownSlider.value = 0;
        }

        // Gọi quái xuất hiện
        foreach (var spawner in monsterSpawners)
        {
            if (spawner != null)
                spawner.SpawnNow();
        }

        float elapsed = 0f;
        while (elapsed < startupTime)
        {
            elapsed += Time.deltaTime;
            if (countdownSlider != null)
                countdownSlider.value = elapsed;

            yield return null;
        }

        if (countdownSlider != null)
            countdownSlider.gameObject.SetActive(false);

        gameEventManager.dialogManager.ShowDialog("Nguồn điện đã được khởi động lại!");
        gameEventManager.FinishPowerActivation();

        isRunning = false;
    }
}
