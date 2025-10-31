using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MySpaceChatController : MonoBehaviour
{
    [Header("References")]
    public ContactsPanelController contactsPanel;
    public ChatManager chatManager;

    [Header("Typing UI")]
    public GameObject typingPanel;
    public TMP_InputField typingInput;
    public Button typingSubmit;

    private string currentContact;
    private ChatBranch currentBranch;
    private Dictionary<string, ContactData> contactLookup = new();

    void Start()
    {
        contactsPanel.onContactSelected += OpenContact;
        typingSubmit.onClick.AddListener(OnTypingSubmit);

        foreach (var c in contactsPanel.contacts)
            contactLookup[c.id.ToLower()] = c;

        typingPanel.SetActive(false);
    }

    public void OpenContact(string contactId)
    {
        string key = contactId.ToLower();
        currentContact = key;


        if (!contactLookup.ContainsKey(key))
        {
            Debug.LogWarning($"[Chat] Kontak {key} tidak ditemukan!");
            return;
        }

        var contact = contactLookup[key];
        currentBranch = contact.rootBranch;

        typingPanel.SetActive(true);
        StartCoroutine(PlayBranch(currentBranch));
    }

    IEnumerator PlayBranch(ChatBranch branch)
    {
        if (branch == null)
        {
            Debug.LogWarning("[Chat] Branch null — tidak ada percakapan.");
            yield break;
        }

        foreach (var line in branch.initialLines)
        {
            chatManager.SpawnOne(line.username, line.message);
            yield return new WaitForSeconds(line.delayAfter);
        }
    }

    void OnTypingSubmit()
    {
        if (currentBranch == null) return;

        string input = typingInput.text.Trim().ToLower();
        typingInput.text = "";
        chatManager.SpawnOne("Alex", input);

        bool matched = false;

        foreach (var opt in currentBranch.possibleResponses)
        {
            foreach (var key in opt.acceptedKeywords)
            {
                if (input.Contains(key.ToLower()))
                {
                    matched = true;
                    StartCoroutine(HandleResponse(opt));
                    return;
                }
            }
        }

        if (!matched && currentBranch.defaultResponse.Count > 0)
        {
            StartCoroutine(PlayLines(currentBranch.defaultResponse));
        }
    }

    IEnumerator HandleResponse(ResponseOption option)
    {
        if (option.npcResponse != null && option.npcResponse.Count > 0)
            yield return PlayLines(option.npcResponse);

        if (option.nextBranch != null)
        {
            currentBranch = option.nextBranch;
            yield return PlayBranch(currentBranch);
        }
        else
        {
            Debug.Log("[Chat] Percakapan selesai di branch ini.");
        }
    }

    IEnumerator PlayLines(List<ChatLine> lines)
    {
        foreach (var line in lines)
        {
            SendMessageToContact(currentContact, line.username, line.message);
            yield return new WaitForSeconds(line.delayAfter);
        }
    }

    public void SendMessageToContact(string contactId, string sender, string message)
    {
        string key = contactId.ToLower();

        if (key == currentContact)
        {
            chatManager.SpawnOne(sender, message);
        }
        else
        {
            contactsPanel.NotifyIncoming(key, 1);
            Debug.Log($"[MySpaceChatController] New message for {key} (unread)");
        }
    }

}
