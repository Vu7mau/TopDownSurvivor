using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatDialogueManager : Singleton<ChatDialogueManager>
{
    [SerializeField] public ChatDialogue chatDialogue;
    [SerializeField] private List<ChatContentSO> chatContents;

    private Coroutine chatCoroutine;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadChatDialogue();
    }

    private void LoadChatDialogue()
    {
        if (chatDialogue != null) return;
        chatDialogue = this.transform.GetComponentInChildren<ChatDialogue>();
    }

    public void ShowChat(int chatIndex)
    {
        if (chatCoroutine != null)
        {
            StopCoroutine(chatCoroutine);
            chatDialogue.HideDialogue(); // Ẩn ngay nếu cần
        }

        chatCoroutine = StartCoroutine(ActiveChat(chatIndex));
    }

    private IEnumerator ActiveChat(int chatIndex)
    {
        if (chatDialogue == null || chatContents.Count <= chatIndex || chatContents[chatIndex] == null)
            yield break;

        var chat = chatContents[chatIndex];

        chatDialogue.ShowDialogue(chat.chatLines, chat.chatDelatTimeBeforHide, chat.notificationAudio, chat.speakerName, chat.speakerAvatar);

        // Chờ tới khi đoạn chat kết thúc (tức là thời gian hiển thị + thời gian animate popup)
        yield return new WaitForSeconds(chat.chatDelatTimeBeforHide + 0.2f); // cộng thêm 0.2s để chắc animation xong

        chatCoroutine = null;
    }
}
