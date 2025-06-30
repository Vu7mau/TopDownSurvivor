using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMenu : MonoBehaviour
{
    public GameObject fightMenuPanel;
    public GameObject selectGamePanel;
    public void OpenFightMenuPanel()
    {
        selectGamePanel.SetActive(false);
    }
    public void CloseFightMenuPanel()
    {
        selectGamePanel.SetActive(true);
    }

}
