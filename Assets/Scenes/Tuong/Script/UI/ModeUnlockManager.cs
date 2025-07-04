using UnityEngine;
public static class ModeUnlockManager 
{
    public const string SURVIVE_UNLOCK_KEY = "SurviveUnlocked";
    public static bool IsSurviveUnlocked()
    {
        return PlayerPrefs.GetInt(SURVIVE_UNLOCK_KEY, 0) == 1;
    }
    public static void UnlockSurviveMode()
    {
        PlayerPrefs.SetInt(SURVIVE_UNLOCK_KEY, 1);
        PlayerPrefs.Save();
    }
    public static void ResetUnlocks()
    {
        PlayerPrefs.DeleteKey(SURVIVE_UNLOCK_KEY);
        PlayerPrefs.Save();
    }
}
