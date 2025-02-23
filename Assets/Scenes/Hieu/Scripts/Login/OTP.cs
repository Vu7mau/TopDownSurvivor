using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OTP : MonoBehaviour
{
    [Serializable]
    public class OTPrequest
    {
        public string Email;
        public int OTP;
        public string NewPassword;        
        public OTPrequest(string email, int OTP,string newpass)
        {
            this.Email = email;
            this.OTP = OTP;
            this.NewPassword = newpass;
        }
    }
    [Serializable]
    public class ResponseOTP
    {
        public bool isSuccess;
        public string notification;
    }
}
