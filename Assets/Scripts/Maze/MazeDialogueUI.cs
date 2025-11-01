using UnityEngine;
using TMPro;
using System.Collections;

public class MazeDialogueUI : MonoBehaviour
{
    public static MazeDialogueUI Instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public float fadeDuration = 0.3f;

    private CanvasGroup canvasGroup;
    private Coroutine currentRoutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        canvasGroup = dialoguePanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = dialoguePanel.AddComponent<CanvasGroup>();

        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(string message, float duration = 3f)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(ShowRoutine(message, duration));
    }

    IEnumerator ShowRoutine(string msg, float duration)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = msg;

        RectTransform rect = dialoguePanel.GetComponent<RectTransform>();
        Vector2 startPos = new Vector2(0, -150); 
        Vector2 endPos = Vector2.zero;           
        float slideTime = 0.25f;

        rect.anchoredPosition = startPos;
        canvasGroup.alpha = 0f;

        
        float t = 0f;
        while (t < slideTime)
        {
            t += Time.deltaTime;
            float p = t / slideTime;
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, p);
            canvasGroup.alpha = p;
            yield return null;
        }

        yield return new WaitForSeconds(duration);

        // slide-out
        t = 0f;
        while (t < slideTime)
        {
            t += Time.deltaTime;
            float p = t / slideTime;
            rect.anchoredPosition = Vector2.Lerp(endPos, startPos, p);
            canvasGroup.alpha = 1f - p;
            yield return null;
        }

        dialoguePanel.SetActive(false);
    }


    IEnumerator FadeCanvas(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
