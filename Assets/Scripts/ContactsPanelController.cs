using UnityEngine;
using System.Collections.Generic;

public class ContactsPanelController : MonoBehaviour
{
    [System.Serializable]
    public class ContactDataWrapper
    {
        public ContactData data;
    }

    public List<ContactData> contacts = new List<ContactData>();
    public Transform contactsParent;
    public GameObject contactButtonPrefab;

    public System.Action<string> onContactSelected;

    void Start()
    {
        Populate();
    }

    public void Populate()
    {
        foreach (Transform t in contactsParent)
            Destroy(t.gameObject);

        foreach (var c in contacts)
        {
            GameObject btn = Instantiate(contactButtonPrefab, contactsParent);
            var ui = btn.GetComponent<ContactButtonUI>();
            ui.Setup(c.id, c.displayName, c.avatar, 0);
            ui.onClick = OnContactClicked;
            c.uiInstance = ui; 
        }

        Debug.Log($"[ContactsPanel] Loaded {contacts.Count} contacts.");
    }

    void OnContactClicked(string id)
    {
        Debug.Log($"[ContactsPanel] Clicked contact: {id}");
        onContactSelected?.Invoke(id);
    }

    public void NotifyIncoming(string contactId, int count = 1)
    {
        var contact = contacts.Find(x => x.id.ToLower() == contactId.ToLower());
        if (contact != null && contact.uiInstance != null)
        {
            contact.uiInstance.IncrementBadge(count);
            Debug.Log($"[ContactsPanel] +{count} badge untuk {contact.displayName}");
        }
    }
}
