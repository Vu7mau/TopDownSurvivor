using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadMainMenu(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
