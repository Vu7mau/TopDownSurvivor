using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
public class LocaleSelector : MonoBehaviour
{
    private bool active = false;
    private void Start()
    {
        int ID = PlayerPrefs.GetInt("LocaleKey", 0);
        ChangeLocale(ID);   
    }
    public void ChangeLocale(int localeID)
    {
        if(active == true)
        {
            return;
        }
        StartCoroutine(SetLocale(localeID));
    }
    private IEnumerator SetLocale(int localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        PlayerPrefs.SetInt("LocaleKey", localeID);
        active = false;
    }
}
