using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [Header("Progress flags")]
    public bool hasFloppy = false;
    public bool floppyInserted = false;
    public bool desktopUnlocked = false;

    // Tambahan untuk fitur MySpace & Clue ===
    private HashSet<string> unlockedApps = new();
    private HashSet<string> unlockedClues = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void UnlockApp(string appName)
    {
        if (string.IsNullOrEmpty(appName))
        {
            Debug.LogWarning("[GameStateManager] appName kosong, lewati.");
            return;
        }

        if (unlockedApps.Contains(appName))
        {
            Debug.Log($"[GameStateManager] App '{appName}' sudah terbuka sebelumnya.");
            return;
        }

        unlockedApps.Add(appName);
        Debug.Log($"[GameStateManager] App '{appName}' berhasil dibuka!");

        // Coba aktifkan GameObject-nya di scene
        var appObj = GameObject.Find(appName);
        if (appObj != null)
        {
            appObj.SetActive(true);
            Debug.Log($"[GameStateManager] GameObject '{appName}' diaktifkan di scene.");
        }
        else
        {
            Debug.LogWarning($"[GameStateManager] Tidak menemukan GameObject '{appName}' di scene — pastikan nama cocok!");
        }
    }

    public void UnlockClue(string clueName)
    {
        if (string.IsNullOrEmpty(clueName))
        {
            Debug.LogWarning("[GameStateManager] clueName kosong, lewati.");
            return;
        }

        if (unlockedClues.Contains(clueName))
        {
            Debug.Log($"[GameStateManager] Clue '{clueName}' sudah ditemukan.");
            return;
        }

        unlockedClues.Add(clueName);
        Debug.Log($"[GameStateManager] Clue '{clueName}' berhasil ditemukan!");

        var clueObj = GameObject.Find(clueName);
        if (clueObj != null)
        {
            clueObj.SetActive(true);
            Debug.Log($"[GameStateManager] Clue object '{clueName}' diaktifkan di scene.");
        }
        else
        {
            Debug.LogWarning($"[GameStateManager] Tidak menemukan GameObject '{clueName}' di scene — pastikan nama cocok!");
        }
    }

    // === Helper kalau mau dicek dari skrip lain ===
    public bool IsAppUnlocked(string appName) => unlockedApps.Contains(appName);
    public bool IsClueUnlocked(string clueName) => unlockedClues.Contains(clueName);
}
