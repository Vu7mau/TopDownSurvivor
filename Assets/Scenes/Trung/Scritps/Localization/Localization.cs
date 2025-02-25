using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Localization")]
public class Localization:ScriptableObject
{
    [Header("Login/Register")]
    public string DLG_LOGIN_SUCCESS = "Đăng nhập thành công!";
    public string DLG_LOGIN_FAILED = "Đăng nhập thất bại!";
    public string DLG_LOGIN_NOT_FULL_INFO = "Vui lòng nhập đầy đủ thông tin";
    public string DLG_REGISTER_SUCCESS = "Đăng ký tài khoản thành công!";
    public string DLG_REGISTER_FAILED = "Đăng ký tài khoản thất bại!";

    [Header("Panel ở phía góc trên bên trái màn hình Gameplay")]
    public string TITLE_ENEMIES_LEFT;
    public string TITLE_ENEMY_WAVES;
    public string TITLE_TIME_TO_NEXT_WAVE;

    [Header("Victory Bosss")]
    public string DLG_WHEN_KILL_BOSS = "BOSS IS DEAD!";
}
