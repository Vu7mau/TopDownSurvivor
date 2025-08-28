using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewChatContent", menuName = "SO/ChatContent")]
public class ChatContentSO : ScriptableObject
{
    public string speakerName;
    public Sprite speakerAvatar;
    [TextArea(2, 6)]
    public string chatLines;

    [Tooltip("Chỉ dùng khi KHÔNG có notificationAudio. (giây)")]
    public int chatDelatTimeBeforHide = 2;

    public AudioClip notificationAudio;
}
