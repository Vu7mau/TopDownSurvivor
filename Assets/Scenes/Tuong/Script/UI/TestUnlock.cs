using UnityEngine;
using UnityEngine.UI;

public class TestUnlock : MonoBehaviour
{
    public GameObject panelUnlock;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            panelUnlock.SetActive(!panelUnlock.activeSelf);
        }
    }
}
