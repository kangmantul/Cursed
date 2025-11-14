using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FINALE_STORY : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(2, 5)] public string text;
        public float duration = 3f;
    }

    [Header("Dialog Sequence")]
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    [Header("Trigger Settings")]
    public bool triggerOnce = true;
    public bool triggerOnStart = false;
    private bool triggered = false;

    [Header("Audio")]
    public AudioClip bgmClipToPlay;  

    void Start()
    {
        if (triggerOnStart)
        {
            TriggerDialogue();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered && triggerOnce) return;
        if (!other.CompareTag("Player")) return;

        TriggerDialogue();

        if (bgmClipToPlay != null)
        {
            FindObjectOfType<AudioManager>()?.PlayBGM(bgmClipToPlay);
        }
    }

    public void TriggerDialogue()
    {
        if (triggered && triggerOnce) return;
        triggered = true;

        if (MazeDialogueUI.Instance == null)
        {
            Debug.LogWarning("[MazeDialogueTrigger] MazeDialogueUI.Instance not found!");
            return;
        }

        StartCoroutine(PlayDialogueSequence());
    }

    IEnumerator PlayDialogueSequence()
    {
        foreach (var line in dialogueLines)
        {
            MazeDialogueUI.Instance.ShowDialogue(line.text, line.duration);
            yield return new WaitForSeconds(line.duration + 0.3f);
        }
    }
}
