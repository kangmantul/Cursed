using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogune UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Text speakerNameText;

    [Header("Ink")]
    [SerializeField] private TextAsset inkJSONAsset;

    [Header("Scene")]
    [SerializeField]private string nextScene;

    private Story currStory;

    private bool dialogueIsPlaying;

    private static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";
  
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of DialogueManager detected. Destroying duplicate.");
        }
        instance = this;
    }
    public static DialogueManager GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        if(inkJSONAsset)
        {
            EnterDialogueMode(inkJSONAsset);
        }
        else
        {
            Debug.LogWarning("No ink JSON asset assigned to DialogueManager.");
        }
    }
    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            ContinueStory();
        }
    }
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }
    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        StartCoroutine(DelayLoadScene(1.5f));
    }
    private IEnumerator DelayLoadScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
    private void ContinueStory() 
    {
        if (currStory.canContinue)
        {
            dialogueText.text = currStory.Continue();
            HandleTags(currStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }
    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    speakerNameText.text = tagValue;
                    break;
                default:
                    Debug.LogWarning("Unhandled tag: " + tag);
                    break;
            }
        }
    }
}

