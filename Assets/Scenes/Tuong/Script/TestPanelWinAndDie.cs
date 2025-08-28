using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPanelWinAndDie : MonoBehaviour
{
    public GameObject panelWin;
    public GameObject panelDie;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (panelWin != null)
                panelWin.SetActive(!panelWin.activeSelf);
                Time.timeScale = 0f;

            if (panelDie != null)
                panelDie.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (panelDie != null)
                panelDie.SetActive(!panelDie.activeSelf);
                Time.timeScale = 0f;

            if (panelWin != null)
                panelWin.SetActive(false);

        }
    }
}
