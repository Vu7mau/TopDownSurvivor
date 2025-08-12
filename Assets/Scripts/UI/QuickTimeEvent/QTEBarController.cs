using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class QTEBarController : VuMonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Canvas qteCanvas;
    [SerializeField] private PointerController pointer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successClip;
    [SerializeField] private AudioClip failClip;
    [SerializeField] private QTEResultPopup resultPopup;

    [Header("Trigger")]
    [SerializeField] private KeyCode triggerKey = KeyCode.E;
    [SerializeField] private int maxFailAttempts = 3;

    [Header("Safe Zone Logic")]
    [SerializeField] private float shrinkFactor = 0.8f;
    [SerializeField] private Vector2 moveRange = new Vector2(150, 40);

    [Header("QTE Time")]
    [SerializeField] private float duration = 6f;
    [SerializeField] private float increaseSpeed = 1f;

    [Header("QTE Canvas FX")]
    [SerializeField] private float popupDuration = 0.25f;
    [SerializeField] private float hideDuration = 0.2f;
    [SerializeField] private float startScale = 0.8f;
    [SerializeField] private bool useFade = true;

    [Header("QTE BGM")]
    [SerializeField] private AudioClip qteMusic;          // Nhạc khi QTE bật
    [SerializeField] private AudioClip qteOutMusic;
    [SerializeField] private bool stopMusicOnHide = true; // Tắt nhạc khi QTE ẩn
    [SerializeField] private float qteMusicVolume = 1f;

    [Header("Fail Spawn")]
    [SerializeField] private List<MonsterSpawnerTrigger> monsterSpawners = new(); // Kéo list spawner vào đây
    [SerializeField] private int spawnPerFail = 1;            // Số quái spawn mỗi lần fail
    [SerializeField] private int spawnIncreasePerFail = 0;    // Tăng thêm mỗi lần fail (0 = không tăng)
    [SerializeField] private bool randomSpawner = true;       // True: chọn 1 spawner ngẫu nhiên; False: spawn khắp tất cả
    [SerializeField] private float spawnDelay = 0.15f;        // Trễ nhỏ giữa các lần spawn 


    private Coroutine qteCoroutine;
    private int currentFails = 0;
    private QuestPasswordCondition quest;
    private List<char> revealedDigits = new();
    private bool isQteCompleted = false;
    private int successCount = 0;

    private CanvasGroup qteCanvasGroup;
    private RectTransform qteRoot;
    private Tween showTween, hideTween;

    protected override void Start()
    {
        if (quest == null)
            quest = GameObject.FindObjectOfType<QuestPasswordCondition>();

        if (qteCanvas != null)
        {
            qteRoot = qteCanvas.GetComponent<RectTransform>();
            qteCanvasGroup = qteCanvas.GetComponent<CanvasGroup>();
            if (qteCanvasGroup == null)
                qteCanvasGroup = qteCanvas.gameObject.AddComponent<CanvasGroup>();

            // Trạng thái ban đầu
            qteCanvas.enabled = false;
            qteRoot.localScale = Vector3.one;
            qteCanvasGroup.alpha = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (isQteCompleted)
        {
            if (resultPopup != null)
                resultPopup.Show("Giải thành công", quest.DoorPassword.ToString());
        }
        else
        {
            if (qteCoroutine == null)
            {
                qteCoroutine = StartCoroutine(HandleQTE());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Input.GetKeyDown(triggerKey) && qteCoroutine == null)
        {
            qteCoroutine = StartCoroutine(HandleQTE());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StopQTE();
        resultPopup.HidePopup();
        CharacterCtrl.Instance.CharacterShooting.SetCancel(false);
    }

    private IEnumerator HandleQTE()
    {
        CharacterCtrl.Instance.CharacterShooting.SetCancel(true);
        currentFails = 0;
        revealedDigits.Clear();
        pointer.logPass.text = "";


        bool ready = false;
        AnimateShowQTE(() => { ready = true; });
        while (!ready) yield return null;

        pointer.ResetState();
        pointer.StartQTE();
        pointer.OnQTEResult += OnQTEResult;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Debug.Log("QTE Timeout!");
        StopQTE();
    }

    private void OnQTEResult(bool success)
    {
        string fullPass = quest.DoorPassword.ToString();

        if (revealedDigits.Count >= fullPass.Length)
        {
            StopQTE();
            return;
        }

        if (!success)
        {
            currentFails++;
            pointer.PlayFailEffect(audioSource, failClip);




            if (currentFails >= maxFailAttempts)
            {
                Debug.Log("QTE failed too many times!");
                if (resultPopup != null)
                    resultPopup.Show("Giải thất bại", "Bạn đã sai quá số lần cho phép!");
                StopQTE();
                if (qteOutMusic)
                    BackgroundMusicManager.Instance.PlayMusic(qteOutMusic);
                SpawnOnFail(currentFails);
            }
            else
            {
                pointer.StartQTE();
            }
            return;
        }

        // Thành công
        pointer.PlaySuccessEffect(audioSource, successClip);
        pointer.ShrinkSafeZoneWidthOnly(shrinkFactor, moveRange.x);
        pointer.IncreaseSpeed(increaseSpeed);

        revealedDigits.Add(fullPass[revealedDigits.Count]);
        pointer.logPass.text = string.Join(" ", revealedDigits);

        if (revealedDigits.Count >= fullPass.Length)
        {
            StopQTE();
            if (resultPopup != null)
                resultPopup.Show("Giải thành công", quest.DoorPassword.ToString());
            isQteCompleted = true;
          //  UIMissionManager.Instance.AddMessage("kkk", "Hay tieu diet bosss");
            Debug.Log("Người chơi đã lấy đủ mật khẩu!");
            return;
        }

        pointer.StartQTE();
    }

    private void StopQTE()
    {
        if (qteCoroutine != null)
        {
            StopCoroutine(qteCoroutine);
            qteCoroutine = null;
        }

        pointer.StopQTE();
        pointer.OnQTEResult -= OnQTEResult;

        // Ẩn canvas bằng hiệu ứng
        AnimateHideQTE();
    }

    private void AnimateShowQTE(System.Action onComplete = null)
    {
        if (qteCanvas == null) { onComplete?.Invoke(); return; }

        showTween?.Kill();
        hideTween?.Kill();

        qteCanvas.enabled = true;

        if (useFade) qteCanvasGroup.alpha = 0f;
        if (qteRoot != null) qteRoot.localScale = Vector3.one * startScale;

        // Play nhạc nền khi QTE bật
        if (qteMusic != null && BackgroundMusicManager.Instance != null)
            BackgroundMusicManager.Instance.PlayMusic(qteMusic, loop: true, volume: qteMusicVolume);

        var seq = DOTween.Sequence().SetUpdate(true);
        if (qteRoot != null)
            seq.Join(qteRoot.DOScale(1f, popupDuration).SetEase(Ease.OutBack));
        if (useFade)
            seq.Join(qteCanvasGroup.DOFade(1f, popupDuration).SetEase(Ease.OutQuad));

        showTween = seq.OnComplete(() => onComplete?.Invoke());
    }

    private void AnimateHideQTE(System.Action onComplete = null)
    {
        if (qteCanvas == null) { onComplete?.Invoke(); return; }

        showTween?.Kill();
        hideTween?.Kill();

        var seq = DOTween.Sequence().SetUpdate(true);
        if (qteRoot != null)
            seq.Join(qteRoot.DOScale(startScale, hideDuration).SetEase(Ease.InBack));
        if (useFade)
            seq.Join(qteCanvasGroup.DOFade(0f, hideDuration).SetEase(Ease.OutQuad));

        hideTween = seq.OnComplete(() =>
        {

            if (stopMusicOnHide && BackgroundMusicManager.Instance != null)
                BackgroundMusicManager.Instance.StopMusic();

            qteCanvas.enabled = false;
            if (qteRoot != null) qteRoot.localScale = Vector3.one;
            onComplete?.Invoke();
        });
    }

    private void SpawnOnFail(int failIndex)
    {
        if (monsterSpawners == null || monsterSpawners.Count == 0) return;

        // Số lượng quái cần spawn ở lần fail này
        int count = Mathf.Max(0, spawnPerFail + (failIndex - 1) * spawnIncreasePerFail);
        if (count <= 0) return;

        StartCoroutine(SpawnFailWave(count));
    }

    private IEnumerator SpawnFailWave(int totalToSpawn)
    {
        if (randomSpawner)
        {
            // Chọn một spawner ngẫu nhiên cho cả đợt
            var spawner = PickRandomSpawner();
            if(!spawner.gameObject.activeSelf)
                spawner.gameObject.SetActive(true);
            if (spawner != null)
            {
                yield return SpawnBurst(spawner, totalToSpawn);
            }
        }
        else
        {
            // Chia đều qua tất cả spawners
            int perSpawner = Mathf.Max(1, totalToSpawn / monsterSpawners.Count);
            int remainder = Mathf.Max(0, totalToSpawn - perSpawner * monsterSpawners.Count);

            for (int i = 0; i < monsterSpawners.Count; i++)
            {
                var spawner = monsterSpawners[i];
                if (spawner == null) continue;

                int thisCount = perSpawner + (i < remainder ? 1 : 0);
                if (thisCount > 0)
                    yield return SpawnBurst(spawner, thisCount);
            }
        }
    }

    private IEnumerator SpawnBurst(MonsterSpawnerTrigger spawner, int amount)
    {

        if (spawner == null) yield break;
        if (!spawner.gameObject.activeSelf)
            spawner.gameObject.SetActive(true); 
        spawner.Spawn(amount);
        if (spawnDelay > 0f) yield return new WaitForSeconds(spawnDelay);

        yield break;
    }

    private MonsterSpawnerTrigger PickRandomSpawner()
    {
        // Lấy spawner hợp lệ
        List<MonsterSpawnerTrigger> pool = monsterSpawners.FindAll(s => s != null);
        if (pool.Count == 0) return null;
        int idx = Random.Range(0, pool.Count);
        return pool[idx];
    }

}
