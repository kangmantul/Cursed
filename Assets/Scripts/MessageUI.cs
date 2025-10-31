using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    public TMP_Text combinedText; 

    public void SetMessage(string username, string message)
    {
        combinedText.text = $"{username}: {message}";
    }
}
