using UnityEngine;
using System.Collections.Generic;

public class MazeMemoryManager : MonoBehaviour
{
    public static MazeMemoryManager Instance;

    [Header("Settings")]
    public List<string> requiredFragments = new List<string>(); 
    public MazeDoorController doorToUnlock; 
    public AudioClip completionClip; 

    private HashSet<string> collected = new HashSet<string>();
    private bool completed = false;

    public GameObject note2Canvas;

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

        if (note2Canvas != null)
            note2Canvas.SetActive(true);

        MazeDialogueSystem.Instance.Play("note2_memory_complete");
        if (completionClip) AudioSource.PlayClipAtPoint(completionClip, Camera.main.transform.position);

        if (doorToUnlock != null)
            doorToUnlock.OpenDoor();
    }
}
