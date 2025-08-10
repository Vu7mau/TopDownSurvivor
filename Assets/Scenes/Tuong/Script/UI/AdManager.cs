using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    private InterstitialAd interstitialAd;

    private string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // ID test Android

    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            LoadInterstitialAd();
        });
    }

    public void LoadInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest();

        InterstitialAd.Load(adUnitId, adRequest, (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Failed to load interstitial ad: " + error);
                return;
            }

            interstitialAd = ad;

            // Gán sự kiện khi quảng cáo bị đóng
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Ad closed.");
                LoadInterstitialAd(); // Load lại
            };

            Debug.Log("Interstitial ad loaded successfully.");
        });
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready.");
        }
    }

    private void OnDestroy()
    {
        interstitialAd?.Destroy();
    }
}
