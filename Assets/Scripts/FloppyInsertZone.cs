using UnityEngine;

public class FloppyInsertZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameFlowManager.Instance.currentState != GameFlowManager.PCState.WAITING_FOR_FLOPPY)
        {
            Debug.Log("PC belum siap menerima floppy.");
            return;
        }

        PickupSystem pickup = FindObjectOfType<PickupSystem>();
        if (pickup == null)
        {
            Debug.LogWarning("PickupSystem tidak ditemukan di scene.");
            return;
        }

        var heldField = typeof(PickupSystem)
    .GetField("heldObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        GameObject held = heldField.GetValue(pickup) as GameObject;

        if (held == null)
        {
            Debug.Log("Tidak sedang memegang apapun.");
            return;
        }

        if (!held.CompareTag("canPickUp") || !held.name.ToLower().Contains("floppy"))
        {
            Debug.Log("Objek yang dipegang bukan floppy.");
            return;
        }

        held.transform.position = transform.position;
        held.transform.rotation = transform.rotation;

        heldField.SetValue(pickup, null);

        Debug.Log("Floppy inserted into PC.");

        GameStateManager.Instance.floppyInserted = true;
        GameStateManager.Instance.hasFloppy = true;

        GameFlowManager.Instance.OnFloppyInserted();

        InteractablePC pc = FindObjectOfType<InteractablePC>();
        if (pc != null)
        {
            pc.LockToPC(); 
        }


        Destroy(held.gameObject, 0.3f);
    }
}
