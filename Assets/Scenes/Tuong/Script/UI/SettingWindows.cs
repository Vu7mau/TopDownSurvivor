using UnityEngine;
public class SettingWindows : MonoBehaviour
{
    private void Start()
    {
        transform.localScale = Vector2.zero;
    }
    public void Open()
    {
        transform.LeanScale(Vector2.one, 0.05f);
    }
    public void Close()
    {
        transform.LeanScale(Vector2.zero, 0.05f).setEaseInBack();
    }
}
