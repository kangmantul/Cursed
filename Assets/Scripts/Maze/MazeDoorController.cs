using UnityEngine;

public class MazeDoorController : MonoBehaviour
{
    [Header("References")]
    public AudioClip destroySound;
    public ParticleSystem destroyEffect;

    [Header("Settings")]
    public float destroyDelay = 0.2f;

    private bool isDestroyed = false;

    public void OpenDoor()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        Debug.Log("[MazeDoor] Door destroyed — puzzle solved!");

        if (destroySound)
            AudioSource.PlayClipAtPoint(destroySound, transform.position);

        if (destroyEffect)
            Instantiate(destroyEffect, transform.position, Quaternion.identity);

        Destroy(gameObject, destroyDelay);
    }
}
