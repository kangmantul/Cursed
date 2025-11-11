using UnityEngine;
using System.Collections.Generic;

public class MazeMemoryManager : MonoBehaviour
{
    public static MazeMemoryManager Instance;

    [Header("Settings")]
    public List<string> requiredFragments = new List<string>(); // id fragment
    public MazeDoorController doorToUnlock; 
    public AudioClip completionClip; // suara / bisikan saat puzzle selesai

    private HashSet<string> collected = new HashSet<string>();
    private bool completed = false;

    void Awake()
    {
        Instance = this;
    }

    public void RegisterFragment(string id)
    {
        if (completed || collected.Contains(id)) return;

        collected.Add(id);
        Debug.Log($"[MemoryManager] Fragment {id} tercatat ({collected.Count}/{requiredFragments.Count})");

        if (collected.Count >= requiredFragments.Count)
            CompletePuzzle();
    }

    void CompletePuzzle()
    {
        completed = true;
        Debug.Log("[MemoryManager] Semua kenangan ditemukan.");

        MazeDialogueSystem.Instance.Play("note2_memory_complete"); // bisa munculkan dialog
        if (completionClip) AudioSource.PlayClipAtPoint(completionClip, Camera.main.transform.position);

        if (doorToUnlock != null)
            doorToUnlock.OpenDoor();
    }
}
