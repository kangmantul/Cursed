using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChatLine
{
    public string username;
    [TextArea(2, 5)] public string message;
    public float delayAfter = 0.5f;
}

[System.Serializable]
public class ResponseOption
{
    [Tooltip("Kata yang buat transisi ke next branch (contoh: 'belum', 'nanti')")]
    public List<string> acceptedKeywords = new List<string>();

    [Tooltip("Balasan NPC sebelum pindah ke next branch")]
    public List<ChatLine> npcResponse = new List<ChatLine>();

    [Tooltip("Branch yang akan dijalankan setelah respon ini (bisa null)")]
    public ChatBranch nextBranch;
}

public enum ChatBranchEventType
{
    None,
    PlayPopup,
    UnlockApp,
    UnlockClue
}

[System.Serializable]
public class ChatBranch
{
    [Tooltip("Dialog awal NPC di branch ini")]
    public List<ChatLine> initialLines = new List<ChatLine>();

    [Tooltip("Semua kemungkinan respon player")]
    public List<ResponseOption> possibleResponses = new List<ResponseOption>();

    [Tooltip("Balasan default")]
    public List<ChatLine> defaultResponse = new List<ChatLine>();

    [Header("Event Setelah Branch Ini")]
    public ChatBranchEventType onBranchCompleteEvent = ChatBranchEventType.None;

    [Tooltip("Untuk event PlayPopup, isi key dialog MazeDialogueSystem")]
    public string popupKey;

    [Tooltip("Untuk event UnlockApp, isi nama app yang ingin dibuka")]
    public string appName;

    [Tooltip("Untuk event UnlockClue, isi nama clue")]
    public string clueName;
}

[System.Serializable]
public class ContactData
{
    public string id;
    public string displayName;
    public Sprite avatar;

    [Header("Root Branch Percakapan")]
    public ChatBranch rootBranch;

    [HideInInspector] public ContactButtonUI uiInstance;
}
