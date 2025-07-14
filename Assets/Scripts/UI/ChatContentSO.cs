using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewChatContent", menuName = "SO/ChatContent")]
public class ChatContentSO : ScriptableObject
{
    public string speakerName;       
    public Sprite speakerAvatar;   
    public string chatLines;

    public int chatDelatTimeBeforHide = 2;
}
