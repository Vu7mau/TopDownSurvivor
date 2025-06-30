using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

public class LocaleSelector : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    private bool isInitialized = false;
    void Start()
    {
        if (dropdown == null)
        {
            Debug.LogError("Dropdown chưa được gán trong Inspector!");
            return;
        }
        StartCoroutine(SetupDropdown());
    }
    IEnumerator SetupDropdown()
    {
        yield return LocalizationSettings.InitializationOperation;
        int savedLocaleIndex = PlayerPrefs.GetInt("LocaleKey", 0);
        dropdown.value = savedLocaleIndex;
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        ChangeLocale(savedLocaleIndex);
        isInitialized = true;
    }

    public void OnDropdownValueChanged(int index)
    {
        if (isInitialized)
        {
            ChangeLocale(index);
        }
    }
    void ChangeLocale(int index)
    {
        PlayerPrefs.SetInt("LocaleKey", index);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
