using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeLetterClue : MonoBehaviour
{
    public string letter;
    public AudioClip whisperSound;
    private bool hasPlayed = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;
        if (other.CompareTag("Player"))
        {
            hasPlayed = true;
            AudioSource.PlayClipAtPoint(whisperSound, transform.position);
            StartCoroutine(GlitchEffect());
        }
    }

    IEnumerator GlitchEffect()
    {
        var rend = GetComponent<Renderer>();
        float t = 0;
        while (t < 1)
        {
            rend.enabled = !rend.enabled;
            yield return new WaitForSeconds(0.1f);
            t += 0.2f;
        }
        rend.enabled = false; 
    }
}
