using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeDialogueSystem : MonoBehaviour
{
    public static MazeDialogueSystem Instance;

    [System.Serializable]
    public class DialogueData
    {
        public string id;
        [TextArea(2, 5)] public string text;
        public float duration = 3f;
    }

    [System.Serializable]
    public class DialogueSequence
    {
        public string id;
        public List<DialogueData> lines = new List<DialogueData>();
    }

    [Header("Global Dialogue List")]
    public List<DialogueSequence> dialogues = new List<DialogueSequence>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Play(string id)
    {
        var seq = dialogues.Find(d => d.id == id);
        if (seq == null)
        {
            Debug.LogWarning($"[MazeDialogueSystem] Dialogue ID '{id}' not found!");
            return;
        }
        StartCoroutine(PlaySequence(seq));
    }

    IEnumerator PlaySequence(DialogueSequence seq)
    {
        if (MazeDialogueUI.Instance == null)
        {
            Debug.LogWarning("[MazeDialogueSystem] MazeDialogueUI.Instance missing!");
            yield break;
        }

        foreach (var line in seq.lines)
        {
            MazeDialogueUI.Instance.ShowDialogue(line.text, line.duration);
            yield return new WaitForSeconds(line.duration + 0.3f);
        }
    }
}
