using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private GameObject loaderCanvas;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI progressText;

    private float target;
    private bool isLoading = false;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
    private void OnEnable()
    {
        if (progressSlider != null) progressSlider.value = 0f;
        else
        {
            RebindText();
        }
        if (progressText != null) progressText.text = "0%";
        target = 0f;
    }
    public void RebindText()
    {
        var allTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        progressText = allTexts.FirstOrDefault(t => t.name == "ProgressText");
        var allSliders = Resources.FindObjectsOfTypeAll<Slider>();
        progressSlider = allSliders.FirstOrDefault(s => s.name == "ProgressSlider");
    }

    public async Task LoadLevelAsync(int levelIndex)
    {
        if (isLoading) return;
        isLoading = true;
        target = 0f;

        if (progressSlider != null) progressSlider.value = 0f;
        loaderCanvas?.SetActive(true);

        for (int i = 0; i < 3; i++)  
            await Task.Yield();

        var scene = SceneManager.LoadSceneAsync(levelIndex);
        scene.allowSceneActivation = false;

        while (scene.progress < 0.9f)
        {
            target = scene.progress;
            await Task.Yield();
        }

        float smoothTimer = 0f;
        float smoothDuration = 0.5f;

        while (smoothTimer < smoothDuration)
        {
            smoothTimer += Time.deltaTime;
            target = Mathf.Lerp(0.9f, 1f, smoothTimer / smoothDuration);
            await Task.Yield();
        }

        target = 1f;

        float extraDelay = 0.3f;
        float delayTimer = 0f;
        while (delayTimer < extraDelay)
        {
            delayTimer += Time.deltaTime;
            await Task.Yield();
        }
        scene.allowSceneActivation = true;

        while (SceneManager.GetActiveScene().buildIndex != levelIndex)
        {
            await Task.Yield();
            if (this == null) return;
        }

        loaderCanvas?.SetActive(false);
        isLoading = false;
    }
    private void Update()
    {
        if (progressSlider == null) return;

        float currentValue = progressSlider.value;
        float newValue = Mathf.MoveTowards(currentValue, target, Time.deltaTime * 3f);
        progressSlider.value = newValue;

        if (progressText != null)
        {
            int percent = Mathf.RoundToInt(newValue * 100f);
            progressText.text = percent + "%";
        }
    }
}
