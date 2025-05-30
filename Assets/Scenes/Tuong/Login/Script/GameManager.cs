using UnityEngine;

public class GameManager : TuongMonobehaviour
{
    [SerializeField] private GameObject panelSetting;
    private string getPanelSetting = "PanelSettingSound";

    private void Start()
    {
        panelSetting.SetActive(false);
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGameObject();
    }
    protected virtual void LoadGameObject()
    {
        if (panelSetting == null) panelSetting = LoadGameObject(panelSetting, getPanelSetting);
    }
    public void OpenSetting()
    {
        panelSetting.SetActive(true);
    }
    public void CloseSetting()
    {
        panelSetting.SetActive(false);
    }
}
