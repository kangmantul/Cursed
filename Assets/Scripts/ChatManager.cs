using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform chatContainer;       
    public GameObject chatMessagePrefab;  
    public ScrollRect scrollRect;         

    [Header("Auto-scroll Settings")]
    public bool autoScroll = true;
    public float smoothScrollDuration = 0.25f;

    private Coroutine smoothScrollCoroutine = null;

    public void SpawnOne(string username, string message)
    {
        if (chatMessagePrefab == null || chatContainer == null)
        {
            Debug.LogError("[ChatManager] prefab atau chatContainer belum di-assign!");
            return;
        }

        GameObject newMsg = Instantiate(chatMessagePrefab, chatContainer);
        var ui = newMsg.GetComponent<MessageUI>();
        if (ui != null)
        {
            ui.SetMessage(username, message);
        }
        else
        {
            Debug.LogWarning("[ChatManager] Chat prefab tidak memiliki komponen MessageUI.");
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContainer.GetComponent<RectTransform>());

        if (autoScroll && scrollRect != null)
        {
            if (smoothScrollCoroutine != null)
                StopCoroutine(smoothScrollCoroutine);
            smoothScrollCoroutine = StartCoroutine(SmoothScrollToBottom(smoothScrollDuration));
        }
        else if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }

    public void ClearChat()
    {
        foreach (Transform child in chatContainer)
        {
            Destroy(child.gameObject);
        }
    }

    IEnumerator SmoothScrollToBottom(float duration)
    {
        if (scrollRect == null)
            yield break;

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContainer.GetComponent<RectTransform>());

        float start = scrollRect.verticalNormalizedPosition;
        float end = 0f;
        if (duration <= 0f)
        {
            scrollRect.verticalNormalizedPosition = end;
            yield break;
        }

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float v = Mathf.SmoothStep(start, end, t / duration);
            scrollRect.verticalNormalizedPosition = v;
            yield return null;
        }

        scrollRect.verticalNormalizedPosition = end;
    }
}
