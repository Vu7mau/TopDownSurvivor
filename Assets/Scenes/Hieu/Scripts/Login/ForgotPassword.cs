using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgotPassword : MonoBehaviour
{
    [Serializable]
    public class ForgotRequestData
    {
        public string email;
        public ForgotRequestData (string email)
        {
            this.email = email;
        }
    }
    [Serializable]
    public class ResponseForgot
    {
        public bool isSuccess;
        public string notification;
    }
}
