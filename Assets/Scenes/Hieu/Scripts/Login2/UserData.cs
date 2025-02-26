using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData 
{    
    public class data
    {
        public List<dataEntry> listdata = new List<dataEntry>();
    }
    [Serializable]
    public class dataEntry
    {
        public string name;
        public string email;
        public string password;
        
    }
}
