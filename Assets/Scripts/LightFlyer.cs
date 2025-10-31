using UnityEngine;
using System.Collections;

public class LightFlyer : MonoBehaviour
{
    public float speed = 15f;
    public float lifeTime = 3f;
    public AudioSource audioSource;
    private Transform target;
    private Light pointLight;

    void Start()
    {
        pointLight = GetComponentInChildren<Light>();
        Destroy(gameObject, lifeTime + 1f);
    }

    public void Init(Transform player)
    {
        target = player;
        if (audioSource != null)
        {
            audioSource.Play();
            audioSource.volume = 0f; 
        }

        StartCoroutine(FlyTowardPlayer());
    }

    IEnumerator FlyTowardPlayer()
    {
        if (target == null)
        {
            Debug.LogWarning("[LightFlyer] Target hilang sebelum mulai.");
            yield break;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        Vector3 endPos = target.position + dir * 2f;

        float totalTime = lifeTime;
        float t = 0f;

        while (t < totalTime)
        {
            if (target == null)
            {
                Debug.LogWarning("[LightFlyer] Target hilang di tengah jalan.");
                break;
            }

            t += Time.deltaTime;

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPos, step);

            if (audioSource != null)
            {
                float dist = Vector3.Distance(transform.position, target.position);
                float normalized = Mathf.Clamp01(1f - (dist / 10f));
                audioSource.volume = Mathf.Lerp(audioSource.volume, normalized, Time.deltaTime * 5f);
            }

            yield return null;
        }

        StartCoroutine(FadeOut());
    }


    IEnumerator FadeOut()
    {
        float duration = 1f;
        float startIntensity = pointLight ? pointLight.intensity : 1f;
        float startVolume = audioSource ? audioSource.volume : 1f;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float f = 1f - t / duration;

            if (pointLight != null)
                pointLight.intensity = startIntensity * f;

            if (audioSource != null)
                audioSource.volume = startVolume * f;

            yield return null;
        }

        if (audioSource != null) audioSource.Stop();
        Destroy(gameObject);
    }
}
