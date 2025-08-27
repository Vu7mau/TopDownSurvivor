using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("Default Options")]
    [Tooltip("Tên scene muốn load (nếu để trống thì dùng Index)")]
    public string sceneName;

    [Tooltip("Index scene muốn load (chỉ dùng nếu sceneName trống)")]
    public int sceneIndex = -1;

    [Tooltip("Có load lại cùng scene hiện tại không?")]
    public bool allowReload = false;

    /// <summary>
    /// Gọi hàm này để load scene theo thiết lập sẵn trong Inspector
    /// </summary>
    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            LoadSceneByName(sceneName);
        }
        else if (sceneIndex >= 0)
        {
            LoadSceneByIndex(sceneIndex);
        }
        else
        {
            Debug.LogWarning("SceneChanger: Chưa cấu hình sceneName hoặc sceneIndex!");
        }
    }

    /// <summary>
    /// Load scene theo tên
    /// </summary>
    public void LoadSceneByName(string name)
    {
        if (!allowReload && SceneManager.GetActiveScene().name == name)
        {
            Debug.Log("SceneChanger: Scene đã ở sẵn (" + name + ")");
            return;
        }
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// Load scene theo index
    /// </summary>
    public void LoadSceneByIndex(int index)
    {
        if (!allowReload && SceneManager.GetActiveScene().buildIndex == index)
        {
            Debug.Log("SceneChanger: Scene đã ở sẵn (index " + index + ")");
            return;
        }
        SceneManager.LoadScene(index);
    }

    /// <summary>
    /// Load lại scene hiện tại
    /// </summary>
    public void ReloadCurrentScene()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    /// <summary>
    /// Thoát game (chạy trong build)
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Thoát game...");
        Application.Quit();
    }
}
