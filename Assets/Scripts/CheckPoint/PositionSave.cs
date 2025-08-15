using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PositionSave
{
    const string K_HAS = "pos_has";
    const string K_PX = "pos_x";
    const string K_PY = "pos_y";
    const string K_PZ = "pos_z";
    const string K_RX = "rot_x";
    const string K_RY = "rot_y";
    const string K_RZ = "rot_z";

    public static void Save(Transform t)
    {
        if (t == null) return;
        var p = t.position;
        var r = t.eulerAngles;

        PlayerPrefs.SetInt(K_HAS, 1);
        PlayerPrefs.SetFloat(K_PX, p.x);
        PlayerPrefs.SetFloat(K_PY, p.y);
        PlayerPrefs.SetFloat(K_PZ, p.z);
        PlayerPrefs.SetFloat(K_RX, r.x);
        PlayerPrefs.SetFloat(K_RY, r.y);
        PlayerPrefs.SetFloat(K_RZ, r.z);
        PlayerPrefs.Save();
    }

    public static bool HasSave() => PlayerPrefs.GetInt(K_HAS, 0) == 1;

    public static bool TryLoad(out Vector3 pos, out Quaternion rot)
    {
        pos = default;
        rot = default;
        if (!HasSave()) return false;

        float px = PlayerPrefs.GetFloat(K_PX), py = PlayerPrefs.GetFloat(K_PY), pz = PlayerPrefs.GetFloat(K_PZ);
        float rx = PlayerPrefs.GetFloat(K_RX), ry = PlayerPrefs.GetFloat(K_RY), rz = PlayerPrefs.GetFloat(K_RZ);

        pos = new Vector3(px, py, pz);
        rot = Quaternion.Euler(rx, ry, rz);
        return true;
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteKey(K_HAS);
        PlayerPrefs.DeleteKey(K_PX);
        PlayerPrefs.DeleteKey(K_PY);
        PlayerPrefs.DeleteKey(K_PZ);
        PlayerPrefs.DeleteKey(K_RX);
        PlayerPrefs.DeleteKey(K_RY);
        PlayerPrefs.DeleteKey(K_RZ);
    }
}   
