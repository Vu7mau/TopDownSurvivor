using System.Collections.Generic;
using UnityEngine;

public class ChatDialogueSequenceRunner : MonoBehaviour
{
    [SerializeField] private ChatDialogue dialogue;
    [SerializeField] public DialogueAnchor? anchorOverride= DialogueAnchor.Header;
    [SerializeField] private List<ChatContentSO> items = new List<ChatContentSO>();
    [SerializeField] private bool playOnStart = false;

    private void Reset()
    {
        if (!dialogue) dialogue = FindObjectOfType<ChatDialogue>();
    }

    private void Start()
    {
        if (playOnStart && dialogue != null && items != null && items.Count > 0)
        {
            dialogue.PlaySequence(items, anchorOverride);
        }
    }

    public void PlayNow()
    {
        if (dialogue != null) dialogue.PlaySequence(items, anchorOverride);
    }

    public void StopNow()
    {
        if (dialogue != null) dialogue.StopSequenceIfAny();
    }
}
