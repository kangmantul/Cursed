using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FilePanelController : MonoBehaviour
{
    [System.Serializable]
    public class FileEntry
    {
        [Header("File Info")]
        public string fileName;

        [Header("References")]
        public Button button;               
        public GameObject targetWindow;     
        public Button closeButton;          

        [Header("Lock Conditions")]
        public bool requiresFloppy = false; 
        public string requiredClueName;     
    }

    [Header("Daftar File")]
    public List<FileEntry> files = new List<FileEntry>();

    void Start()
    {
        foreach (var entry in files)
        {
            if (entry.targetWindow != null)
                entry.targetWindow.SetActive(false);

            if (entry.button != null)
                entry.button.onClick.AddListener(() => OnFileClicked(entry));

            if (entry.closeButton != null)
                entry.closeButton.onClick.AddListener(() => CloseFile(entry));
        }
    }

    void OnFileClicked(FileEntry entry)
    {
        if (entry.requiresFloppy && !GameStateManager.Instance.floppyInserted)
        {
            Debug.Log($"[FilePanel] '{entry.fileName}' terkunci — masukkan floppy dulu!");
            FindObjectOfType<AudioManager>()?.PlaySFX("locked");
            return;
        }

        if (!string.IsNullOrEmpty(entry.requiredClueName) &&
            !GameStateManager.Instance.IsClueUnlocked(entry.requiredClueName))
        {
            Debug.Log($"[FilePanel] '{entry.fileName}' masih terkunci — butuh clue '{entry.requiredClueName}'.");
            FindObjectOfType<AudioManager>()?.PlaySFX("locked");
            return;
        }

        if (entry.targetWindow != null)
        {
            entry.targetWindow.SetActive(true);
            entry.targetWindow.transform.SetAsLastSibling();
            FindObjectOfType<AudioManager>()?.PlaySFX("openfile");
            Debug.Log($"[FilePanel] Membuka file '{entry.fileName}'.");
        }
    }

    public void CloseFile(FileEntry entry)
    {
        if (entry.targetWindow != null && entry.targetWindow.activeSelf)
        {
            entry.targetWindow.SetActive(false);
            FindObjectOfType<AudioManager>()?.PlaySFX("closefile");
            Debug.Log($"[FilePanel] Menutup file '{entry.fileName}'.");
        }
    }

    public void CloseAllFiles()
    {
        foreach (var entry in files)
        {
            if (entry.targetWindow != null && entry.targetWindow.activeSelf)
                entry.targetWindow.SetActive(false);
        }
        FindObjectOfType<AudioManager>()?.PlaySFX("closefile");
        Debug.Log("[FilePanel] Semua file ditutup.");
    }
}
