using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterData
{
    [Serializable]
    public class RegisterRequestData
    {
        public string email;
        public string password;
        public string name;
        public string linkAvatar;
        public RegisterRequestData(string email, string password, string name,string linkavatar)
        {
            this.email = email;
            this.password = password;
            this.name = name;
            this.linkAvatar = linkavatar;
        }
    }
    [Serializable]
    public class ResponseUserSuccess
    {
        public bool isSuccess;
        public string notification;
        public RegisterUserData data;
    }
    [Serializable]
    public class RegisterUserData
    {
        public string name;
    }
}
