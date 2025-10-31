using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.Linq;

public class InkClueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Transform choicesParent;
    public Button choiceButtonPrefab;
    public TextAsset inkJSONAsset;

    private Story story;
    private List<Button> activeChoices = new List<Button>();

    void Start()
    {
        story = new Story(inkJSONAsset.text);
        dialoguePanel.SetActive(false);
        Debug.Log("Story dimulai. Semua clue default: false");
    }

    // === DIPANGGIL DARI BUTTON "MySpace" ===
    public void StartChat()
    {
        if (story == null)
            story = new Story(inkJSONAsset.text);

        story.ChoosePathString("chat_with_caca");
        dialoguePanel.SetActive(true);
        RefreshView();
        Debug.Log("StartChat() called, story path loaded.");
    }

    // === REFRESH DIALOG (BARIS + CHOICES) ===
    void RefreshView()
    {
        ClearChoices();

        // Tampilkan semua line teks hingga habis
        string textBuffer = "";
        while (story.canContinue)
        {
            string line = story.Continue().Trim();
            textBuffer += line + "\n";
        }

        dialogueText.text = textBuffer.Trim();

        // Jika ada pilihan, tampilkan
        if (story.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
        else
        {
            Debug.Log("Tidak ada pilihan aktif. Cerita mungkin selesai atau menunggu trigger lain.");
        }
    }

    // === TAMPILKAN PILIHAN ===
    void DisplayChoices()
    {
        ClearChoices();
        Debug.Log($"Menampilkan {story.currentChoices.Count} pilihan.");

        foreach (Choice choice in story.currentChoices)
        {
            Button choiceButton = Instantiate(choiceButtonPrefab, choicesParent);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = choice.text.Trim();

            // Capture variable 'choice' agar closure aman
            int choiceIndex = choice.index;

            choiceButton.onClick.AddListener(() =>
            {
                story.ChooseChoiceIndex(choiceIndex);
                RefreshView();
            });

            activeChoices.Add(choiceButton);
        }
    }

    // === HAPUS PILIHAN LAMA ===
    void ClearChoices()
    {
        foreach (Button btn in activeChoices)
            Destroy(btn.gameObject);
        activeChoices.Clear();
    }

    // === AKTIFKAN CLUE ===
    public void UnlockClue(string clueName)
    {
        if (story.variablesState.Contains(clueName))
        {
            story.variablesState[clueName] = true;
            Debug.Log($"Clue '{clueName}' berhasil diaktifkan!");
        }
        else
        {
            Debug.LogWarning($"Clue '{clueName}' tidak ditemukan di Ink!");
        }
    }

    // === CEK STATUS CLUE ===
    public bool IsClueUnlocked(string clueName)
    {
        if (story.variablesState.Contains(clueName))
            return (bool)story.variablesState[clueName];
        return false;
    }
}
