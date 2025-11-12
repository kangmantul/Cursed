using UnityEngine;

[CreateAssetMenu(fileName = "NewChatBranch", menuName = "Chat/Chat Branch Asset")]
public class ChatBranchAsset : ScriptableObject
{
    public ChatBranch branch;

    [Header("Next Chat Segment")]
    [Tooltip("SO berikutnya yang akan dijalankan setelah branch ini selesai")]
    public ChatBranchAsset nextBranchAsset;
}
