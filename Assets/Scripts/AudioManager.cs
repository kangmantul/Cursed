using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip bgmClip;
    public AudioClip[] sfxClips;
    public AudioClip ambientClip;

    [Header("Settings")]
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float ambientVolume = 0.6f;
    public bool playBGMOnStart = true;
    public bool playAmbientOnStart = false;

    private AudioSource bgmSource;
    private AudioSource sfxSource;
    private AudioSource ambientSource;

    void Awake()
    {
        // Biar gak duplikat, hancurin semua AudioManager lain di scene
        var allManagers = FindObjectsOfType<AudioManager>();
        foreach (var mgr in allManagers)
        {
            if (mgr != this)
            {
                Destroy(mgr.gameObject);
            }
        }

        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        ambientSource = gameObject.AddComponent<AudioSource>();

        bgmSource.loop = true;
        ambientSource.loop = true;
        bgmSource.spatialBlend = sfxSource.spatialBlend = ambientSource.spatialBlend = 0f;

        Debug.Log($"[AudioManagerScene] Initialized in scene '{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}'");
    }

    void Start()
    {
        if (playBGMOnStart && bgmClip != null)
        {
            PlayBGM(bgmClip);
        }

        if (playAmbientOnStart && ambientClip != null)
        {
            PlayAmbient(ambientClip);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("[AudioManagerScene] No BGM assigned!");
            return;
        }

        bgmSource.clip = clip;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
        Debug.Log($"[AudioManagerScene] Playing BGM: {clip.name}");
    }

    public void PlaySFX(string clipName)
    {
        AudioClip clip = System.Array.Find(sfxClips, c => c && c.name.ToLower() == clipName.ToLower());
        if (clip == null)
        {
            Debug.LogWarning($"[AudioManagerScene] SFX '{clipName}' not found!");
            return;
        }

        sfxSource.PlayOneShot(clip, sfxVolume);
        Debug.Log($"[AudioManagerScene] Playing SFX: {clip.name}");
    }

    public void PlayAmbient(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("[AudioManagerScene] No Ambient assigned!");
            return;
        }

        ambientSource.clip = clip;
        ambientSource.volume = ambientVolume;
        ambientSource.Play();
        Debug.Log($"[AudioManagerScene] Playing Ambient: {clip.name}");
    }

    public void StopAll()
    {
        bgmSource.Stop();
        sfxSource.Stop();
        ambientSource.Stop();
        Debug.Log("[AudioManagerScene] All audio stopped.");
    }
}
