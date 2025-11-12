using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public AudioClip clip;

    void Start()
    {
        var src = gameObject.AddComponent<AudioSource>();
        src.clip = clip;
        src.spatialBlend = 0f;
        src.volume = 1f;
        src.Play();

        Debug.Log($"[TEST] Playing clip: {clip?.name}");
    }
}
