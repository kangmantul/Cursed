using UnityEngine;
using System.Collections;

public class LightFlyer : MonoBehaviour
{
    public float speed = 6f;
    public float lifetime = 5f;
    public AudioSource flySound;
    private Transform player;
    private bool isChasing = false;
    public System.Action OnFlyerDestroyed;

    void OnDestroy()
    {
        OnFlyerDestroyed?.Invoke();
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (flySound != null)
        {
            flySound.Play();
            flySound.spatialBlend = 1f;
        }

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!isChasing && player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[LightFlyer] Player tertangkap! Respawn...");
            MazeRespawnManager.Instance.RespawnPlayer();

            // bunyi efek + hancurkan cahaya
            if (flySound != null) flySound.Stop();
            Destroy(gameObject);
        }
    }
}
