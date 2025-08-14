using System;
using UnityEngine;

public class ScrollLockManager : MonoBehaviour
{
    public SmoothMouseScroll scrollA;
    public SmoothMouseScroll scrollB;

    private SmoothMouseScroll activeScroll = null;
    private float lastScrollTime = -10f;
    public float lockDuration = 0.1f;
    void Update()
    {
        if (activeScroll != null)
        {
            if (Time.unscaledTime - lastScrollTime > lockDuration)
            {
                activeScroll = null;
            }
        }
    }
    public bool RequestLock(SmoothMouseScroll requester)
    {
        if (activeScroll == null || activeScroll == requester)
        {
            activeScroll = requester;
            lastScrollTime = Time.unscaledTime;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void RefreshLock(SmoothMouseScroll requester)
    {
        if (activeScroll == requester)
        {
            lastScrollTime = Time.unscaledTime;
        }
    }
    public void ReleaseLock(SmoothMouseScroll requester)
    {
        if (activeScroll == requester)
        {
            activeScroll = null;
        }
    }
}
