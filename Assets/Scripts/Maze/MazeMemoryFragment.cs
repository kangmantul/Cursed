using UnityEngine;

public class MazeMemoryFragment : MonoBehaviour
{
    [Header("Fragment Info")]
    public string fragmentId = "memory1";
    public string onActivateText = "Sebuah kenangan muncul di pikiranku...";
    public AudioClip whisperClip;
    public ParticleSystem collectEffect;

    private bool isCollected = false;

    public void Collect()
    {
        if (isCollected) return;
        isCollected = true;

        Debug.Log($"[MemoryFragment] Collected: {fragmentId}");

        // mainkan suara
        if (whisperClip)
            AudioSource.PlayClipAtPoint(whisperClip, transform.position);

        // mainkan efek partikel
        if (collectEffect)
            Instantiate(collectEffect, transform.position, Quaternion.identity);

        // tampilkan teks di layar
        //MazeDialogueSystem.Instance.PlayText(onActivateText);

        // kirim data ke Memory Manager
        MazeMemoryManager.Instance.RegisterFragment(fragmentId);

        // opsional: hancurkan objek supaya gak bisa diambil lagi
        Destroy(gameObject);
    }
}
