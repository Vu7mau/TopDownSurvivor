using System;

public class Login
{
    [Serializable]
    public class RequestLoginData
    {
        public string email;
        public string password;
        public RequestLoginData(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }
    public class ResponseLogin
    {
        public bool isSuccess;
        public string notification;
        public LoginUserData data;
    }
    public class LoginUserData
    {
        public string id;
        public string name;
        public string email;
        public string avatar;
    }
}
