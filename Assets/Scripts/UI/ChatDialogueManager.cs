using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatDialogueManager : VuMonoBehaviour
{
    [SerializeField] private ChatDialogue chatDialogue;
    [SerializeField] private List<ChatContentSO> chatContents;

    private Coroutine chatCoroutine;
    private Queue<int> chatQueue = new();
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadChatDialogue();
    }
    protected override void OnEnable()
    {
       
    }
    private void LoadChatDialogue()
    {
        if (chatDialogue != null) return;

        chatDialogue = this.transform.GetComponentInChildren<ChatDialogue>();
    }
    public void EnqueueChat(int chatIndex)
    {
        chatQueue.Enqueue(chatIndex);

        if (chatCoroutine == null)
        {
            chatCoroutine = StartCoroutine(ProcessChatQueue());
        }
    }

    private IEnumerator ProcessChatQueue()
    {
        while (chatQueue.Count > 0)
        {
            int index = chatQueue.Dequeue();
            yield return StartCoroutine(ActiveChat(index));
        }

        chatCoroutine = null;
    }
    //public void SignalActiveChat(int chatId)
    //{
    //    if (chatCoroutine == null)
    //    {
    //        chatCoroutine = StartCoroutine(ActiveChat(0));

    //    }
    //}    
    private IEnumerator ActiveChat(int chatIndex)
    {
        if (chatDialogue == null || chatContents.Count < 1) yield break;

        if (chatContents[chatIndex] != null)
        {
            var chat = chatContents[chatIndex];
            chatDialogue.ShowDialogue(chat.speakerName, chat.speakerAvatar, chat.chatLines);
            yield return new WaitForSeconds(chat.chatDelatTimeBeforHide);
            chatDialogue.HideDialogue();
        }

        chatCoroutine = null;
    }


}
