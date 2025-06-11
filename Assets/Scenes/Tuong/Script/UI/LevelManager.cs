using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private GameObject loaderCanvas;
    [SerializeField] private Slider progressSlider;
    private float target;
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public async void LoadLevel(int levelName)
    {
        target = 0f;
        if(progressSlider != null) progressSlider.value = 0f;

        var scene = SceneManager.LoadSceneAsync(levelName);
        scene.allowSceneActivation = false;
        loaderCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
            target = scene.progress;
        } while (scene.progress < 0.9f);
        target = 1f; 
        await Task.Delay(1000);
        scene.allowSceneActivation = true;
        while(SceneManager.GetActiveScene().buildIndex != levelName)
        {
            await Task.Yield();
        }
        loaderCanvas.SetActive(false);
    }
    public void LoadLevel()
    {
        LoadLevel(2);
    }
    private void Update()
    {
        float currentValue = progressSlider != null ? progressSlider.value : 0f;
        float newValue = Mathf.MoveTowards(currentValue, target, Time.deltaTime * 3f);
        if (progressSlider != null)
        {
            progressSlider.value = newValue;
        }
    }
}
