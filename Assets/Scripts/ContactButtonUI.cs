using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContactButtonUI : MonoBehaviour
{
    [Header("UI References")]
    public Image avatarImage;
    public TextMeshProUGUI nameText;
    public GameObject badgeGO;
    public TextMeshProUGUI badgeText;
    public Button button;

    [HideInInspector] public System.Action<string> onClick;

    private string contactId;
    private int badgeCount = 0;

    public void Setup(string id, string displayName, Sprite avatar, int initialBadge = 0)
    {
        contactId = id.ToLower();
        nameText.text = displayName;
        if (avatar != null) avatarImage.sprite = avatar;
        SetBadge(initialBadge);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            Debug.Log($"[ContactButtonUI] {displayName} clicked (id: {id})");
            ClearBadge();
            onClick?.Invoke(contactId);
        });
    }
    public void SetBadge(int n)
    {
        badgeCount = Mathf.Max(0, n);
        if (badgeGO != null)
        {
            badgeGO.SetActive(badgeCount > 0);
            badgeText.text = badgeCount.ToString();
        }
    }
    public void IncrementBadge(int delta = 1)
    {
        SetBadge(badgeCount + delta);
    }
    public void ClearBadge()
    {
        SetBadge(0);
    }

    public string GetContactId() => contactId;

    public int GetBadgeCount() => badgeCount;
}
