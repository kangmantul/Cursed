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
    public GameObject chatPanel;

    [Header("Typing UI")]
    public GameObject typingPanel;
    public TMP_InputField typingInput;

    private string currentContact;
    private ChatBranch currentBranch;

    private Dictionary<string, List<ChatLine>> chatHistory = new();
    private Dictionary<string, ContactData> contactLookup = new();
    private Dictionary<string, ChatBranch> currentBranches = new();

    void Start()
    {
        contactsPanel.onContactSelected += OpenContact;

        foreach (var c in contactsPanel.contacts)
            contactLookup[c.id.ToLower()] = c;

        typingInput.onSubmit.AddListener(OnSubmitFromInput);
        typingInput.onEndEdit.AddListener(OnSubmitFromInput);

        chatPanel.SetActive(false);
        typingPanel.SetActive(false);
    }

    void OnSubmitFromInput(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;

        OnTypingSubmit();
        typingInput.text = "";
        typingInput.ActivateInputField();
    }

    public void OpenContact(string contactId)
    {
        string key = contactId.ToLower();

        if (currentContact == key && chatPanel.activeSelf)
        {
            chatPanel.SetActive(false);
            typingPanel.SetActive(false);
            currentContact = null;
            return;
        }

        currentContact = key;

        if (!contactLookup.ContainsKey(key))
        {
            Debug.LogWarning($"[Chat] Kontak {key} tidak ditemukan!");
            return;
        }

        var contact = contactLookup[key];
        if (!currentBranches.TryGetValue(key, out currentBranch) || currentBranch == null)
        {
            currentBranch = contact.rootBranch;
            currentBranches[key] = currentBranch;
        }

        chatPanel.SetActive(true);
        typingPanel.SetActive(true);
        chatManager.ClearChat();

        if (chatHistory.ContainsKey(key))
        {
            foreach (var line in chatHistory[key])
                chatManager.SpawnOne(line.username, line.message);

            Debug.Log($"[Chat] Menampilkan riwayat chat lama untuk {key}");
        }
        else
        {
            chatHistory[key] = new List<ChatLine>();
            StartCoroutine(PlayBranch(currentBranch));
        }

        Debug.Log($"[MySpace] Membuka kontak: {key}");
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
            SaveToHistory(currentContact, line);
            yield return new WaitForSeconds(line.delayAfter);
        }
    }

    void OnTypingSubmit()
    {
        if (currentBranch == null || string.IsNullOrEmpty(currentContact)) return;

        string input = typingInput.text.Trim().ToLower();
        if (string.IsNullOrEmpty(input)) return;

        var playerLine = new ChatLine { username = "Alex", message = input, delayAfter = 0.3f };
        chatManager.SpawnOne(playerLine.username, playerLine.message);
        SaveToHistory(currentContact, playerLine);

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
            StartCoroutine(PlayLines(currentBranch.defaultResponse));
    }

    IEnumerator HandleResponse(ResponseOption option)
    {
        if (option.npcResponse != null && option.npcResponse.Count > 0)
            yield return PlayLines(option.npcResponse);

        if (option.nextBranch != null)
        {
            currentBranch = option.nextBranch;

            if (!string.IsNullOrEmpty(currentContact))
                currentBranches[currentContact] = currentBranch;

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
            chatManager.SpawnOne(line.username, line.message);
            SaveToHistory(currentContact, line);
            yield return new WaitForSeconds(line.delayAfter);
        }
    }

    void SaveToHistory(string contactId, ChatLine line)
    {
        if (string.IsNullOrEmpty(contactId)) return;
        if (!chatHistory.ContainsKey(contactId))
            chatHistory[contactId] = new List<ChatLine>();
        chatHistory[contactId].Add(line);
    }
}
