// File: GameController.cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    [SerializeField] private List<Map_Controller> maps;
    [SerializeField] private Map_Controller currentMap;
    [SerializeField] private Map_Controller lastMap;

    public Action OnMapSwitched;
    public Action OnWaveStarted;

    [SerializeField] private Transform character;
    public Transform Character => character;
    public Map_Controller CurrentMap => currentMap;

    [Header("Screen Fade")]
    [SerializeField] private Image fadeImage;
    [SerializeField, Min(0f)] private float fadeDuration = 1.0f;
    public float FadeDuration => fadeDuration;

    [Header("Options")]
    [Tooltip("Tự động thêm checkpoint tại spawn của map mới sau khi SwitchMap xong")]
    [SerializeField] private bool autosaveCheckpointAtSpawn = true;

    protected override void Start()
    {
        base.Start();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCharacter();
        LoadAllMap();
    }

    private void LoadAllMap()
    {
        if (maps != null && maps.Count > 0) return;
        maps = this.transform.GetComponentsInChildren<Map_Controller>(true).ToList();
        // đảm bảo mapIndex hợp lệ theo thứ tự
        for (int i = 0; i < maps.Count; i++) maps[i].EditorSetMapIndex(i);
    }

    private void LoadCharacter()
    {
        if (!character)
            character = GameObject.FindObjectOfType<CharacterCtrl>()?.transform;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (maps == null || maps.Count == 0) LoadAllMap();
        if (!currentMap) currentMap = maps.FirstOrDefault();

        // Nếu có vị trí đơn lẻ cũ thì khôi phục, không thì về spawn map đầu
        if (character)
        {
            if (!(PositionSave.TryLoad(out var pos, out var rot)))
            {
                if (currentMap) MoveCharacterPos(currentMap.currentMapSpawnPoint);
            }
            else TeleportSafe(pos, rot);
        }
    }

    // ===== Teleport an toàn (CC/RB/NavMeshAgent) =====
    public void TeleportSafe(Vector3 pos, Quaternion rot)
    {
        if (!character) { Debug.LogError("[GameController] Character is null!"); return; }

        var cc = character.GetComponent<CharacterController>();
        var rb = character.GetComponent<Rigidbody>();
        var agent = character.GetComponent<NavMeshAgent>();

        if (agent && agent.enabled)
        {
            agent.Warp(pos);
            character.rotation = rot; // Warp không set rotation
            agent.ResetPath();
            return;
        }

        if (cc) cc.enabled = false;
        if (rb) { rb.isKinematic = true; rb.velocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }

        character.SetPositionAndRotation(pos, rot);

        if (cc) cc.enabled = true;
        if (rb) rb.isKinematic = false;
    }

    public void MoveCharacterPos(Transform pos)
    {
        if (!character || !pos)
        {
            Debug.LogError("Character hoặc pos không được gán!");
            return;
        }
        TeleportSafe(pos.position, pos.rotation);
    }

    // ===== Fade =====
    public void ScreenFadeIn() { if (fadeImage && fadeDuration > 0f) StartCoroutine(FadeIn()); }
    public void ScreenFadeOut() { if (fadeImage && fadeDuration > 0f) StartCoroutine(FadeOut()); }

    private IEnumerator FadeIn()
    {
        float t = 0f; var color = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 0f; fadeImage.color = color;
    }

    private IEnumerator FadeOut()
    {
        float t = 0f; var color = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1f; fadeImage.color = color;
    }

    private IEnumerator FadeOutThen(Action afterBlack)
    {
        if (fadeImage && fadeDuration > 0f) yield return StartCoroutine(FadeOut());
        afterBlack?.Invoke();
        yield return null; // ổn định 1 frame
        if (fadeImage && fadeDuration > 0f) yield return StartCoroutine(FadeIn());
    }

    // ===== Map Switch (trong 1 scene) =====
    public void SwitchMap(int mapIndex)
    {
        if (maps == null || maps.Count == 0)
        {
            Debug.LogError("[GameController] No maps configured.");
            return;
        }
        if (mapIndex < 0 || mapIndex >= maps.Count)
        {
            Debug.LogError($"Map index {mapIndex} không hợp lệ!");
            return;
        }

        lastMap = currentMap;
        var next = maps[mapIndex];

        // lưu vị trí đơn lẻ trước khi chuyển (tùy chọn của bạn)
        if (character) PositionSave.Save(character);

        StartCoroutine(FadeOutThen(() =>
        {
            // bật map mới
            currentMap = next;
            if (currentMap.map) currentMap.map.gameObject.SetActive(true);
            currentMap.EnableProcessing();

            // teleport về spawn map mới
            MoveCharacterPos(currentMap.currentMapSpawnPoint);

            // tắt map cũ
            if (lastMap != null)
            {
                if (lastMap.map) lastMap.map.gameObject.SetActive(false);
                lastMap.DisableProcessing();
            }

            // thông báo & autosave CP tại spawn
            OnMapSwitched?.Invoke();
            CharacterUIManager.OnScreenFadeIn?.Invoke();

            if (autosaveCheckpointAtSpawn && character != null)
            {
                var idx = CurrentMap ? CurrentMap.MapIndex : mapIndex;
                CheckpointStore.Add(character, idx, $"Spawn Map {idx}");
            }
        }));
    }

    // ===== tiện ích Editor =====
#if UNITY_EDITOR
    [ContextMenu("Rebuild Maps From Children")]
    private void EditorRebuildMaps()
    {
        maps = this.transform.GetComponentsInChildren<Map_Controller>(true).ToList();
        for (int i = 0; i < maps.Count; i++) maps[i].EditorSetMapIndex(i);
        UnityEditor.EditorUtility.SetDirty(this);
        Debug.Log($"[GameController] Rebuilt maps list: {maps.Count} entries.");
    }
#endif
}
