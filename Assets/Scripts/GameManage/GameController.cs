// File: GameController.cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    [Header("Maps")]
    [SerializeField] private List<Map_Controller> maps;
    [SerializeField] private Map_Controller currentMap;
    [SerializeField] private Map_Controller lastMap;

    public Action OnMapSwitched;

    [Header("Character")]
    [SerializeField] private Transform character;
    public Transform Character => character;
    public Map_Controller CurrentMap => currentMap;

    [Header("Screen Fade")]
    [SerializeField] private Image fadeImage;
    [SerializeField, Min(0f)] private float fadeDuration = 1.0f;
    public float FadeDuration => fadeDuration;

    [Header("Options")]
    [SerializeField] private bool autosaveCheckpointAtSpawn = true;
    [SerializeField] private bool ignoreSavedPositionOnStart = false;
    [SerializeField] private bool allowRespawnWhenSameMap = false;

    public int CurrentMapIndex => currentMap ? currentMap.MapIndex : 0;
    public int MapsCount
    {
        get
        {
            if (maps != null && maps.Count > 0) return maps.Count;
            return this.transform.GetComponentsInChildren<Map_Controller>(true).Length;
        }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (!character) character = GameObject.FindObjectOfType<CharacterCtrl>()?.transform;
        if (maps == null || maps.Count == 0)
        {
            maps = this.transform.GetComponentsInChildren<Map_Controller>(true).ToList();
            for (int i = 0; i < maps.Count; i++) maps[i].EditorSetMapIndex(i);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (!currentMap) currentMap = maps.FirstOrDefault();
        InitializeMapsAtStartup();

        if (!character) return;

        if (ignoreSavedPositionOnStart || !PositionSave.TryLoad(out var pos, out var rot))
        {
            if (currentMap) MoveCharacterPos(currentMap.currentMapSpawnPoint);
        }
        else
        {
            TeleportSafe(pos, rot);
        }
    }

    private void InitializeMapsAtStartup()
    {
        if (maps == null) return;
        for (int i = 0; i < maps.Count; i++)
        {
            bool isCurrent = (currentMap != null && maps[i] == currentMap) || (currentMap == null && i == 0);
            if (maps[i].map) maps[i].map.gameObject.SetActive(isCurrent);
            if (isCurrent) maps[i].EnableProcessing(); else maps[i].DisableProcessing();
        }
        if (currentMap == null && maps.Count > 0) currentMap = maps[0];
        var enterHooks = HooksOf(currentMap);
        enterHooks?.InvokeEnter();
    }

    public void TeleportSafe(Vector3 pos, Quaternion rot)
    {
        if (!character) { Debug.LogError("[GameController] Character is null!"); return; }

        var cc = character.GetComponent<CharacterController>();
        var rb = character.GetComponent<Rigidbody>();
        var agent = character.GetComponent<NavMeshAgent>();

        if (agent && agent.enabled)
        {
            agent.Warp(pos);
            character.rotation = rot;
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
        yield return null;
        if (fadeImage && fadeDuration > 0f) yield return StartCoroutine(FadeIn());
    }

    public void SwitchMap(int mapIndex)
    {
        if (maps == null || maps.Count == 0) { Debug.LogError("[GameController] No maps configured."); return; }
        if (mapIndex < 0 || mapIndex >= maps.Count) { Debug.LogError($"Map index {mapIndex} không hợp lệ!"); return; }

        if (!allowRespawnWhenSameMap && currentMap != null && currentMap.MapIndex == mapIndex)
            return;

        lastMap = currentMap;
        var next = maps[mapIndex];

        if (character) PositionSave.Save(character);

        StartCoroutine(FadeOutThen(() =>
        {
            var exitHooks = HooksOf(lastMap);
            exitHooks?.InvokeExit();

            currentMap = next;
            ActivateOnly(currentMap);

            MoveCharacterPos(currentMap.currentMapSpawnPoint);

            var enterHooks = HooksOf(currentMap);
            Debug.Log("-----------------------------------------------------");
            enterHooks?.InvokeEnter();

            OnMapSwitched?.Invoke();

            if (autosaveCheckpointAtSpawn && character != null)
            {
                var idx = CurrentMapIndex;
                CheckpointStore.Add(character, idx, $"Spawn Map {idx}", isAuto: true);
            }

            var idxNow = CurrentMapIndex;
            string mapName = currentMap && currentMap.map ? currentMap.map.name
                             : currentMap ? currentMap.gameObject.name
                             : $"Map {idxNow}";
            ChatNotify.Instance?.MapSwitched(idxNow, mapName);
        }));
    }

    public void GoToMapSpawn(int mapIndex) => SwitchMap(mapIndex);

    public void ReturnToCurrentMapSpawn()
    {
        if (!currentMap || !currentMap.currentMapSpawnPoint) return;
        StartCoroutine(FadeOutThen(() => { MoveCharacterPos(currentMap.currentMapSpawnPoint); }));
    }

    private void ActivateOnly(Map_Controller toEnable)
    {
        if (maps == null) return;

        foreach (var m in maps)
        {
            bool active = (m == toEnable);
            if (m.map) m.map.gameObject.SetActive(active);
            if (active) m.EnableProcessing(); else m.DisableProcessing();
        }
    }

    private MapHooks HooksOf(Map_Controller mapCtrl)
    {
        if (mapCtrl == null) return null;
        var root = mapCtrl.map ? mapCtrl.map : mapCtrl.transform;
        return root.GetComponentInChildren<MapHooks>(true);
    }
}
