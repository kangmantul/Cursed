using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    public Transform holdPosition;
    public float pickupRange = 3f;
    public GameObject holdPrefab;

    private GameObject heldObject;
    private GameObject originalObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
                TryPickup();
            else
                DropObject();
        }

        if (heldObject != null)
        {
            heldObject.transform.position = holdPosition.position;
            heldObject.transform.rotation = holdPosition.rotation;
        }
    }

    void TryPickup()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if (hit.transform.CompareTag("canPickUp"))
            {
                originalObject = hit.transform.gameObject;

                foreach (Renderer r in originalObject.GetComponentsInChildren<Renderer>())
                {
                    r.enabled = false;
                }

                heldObject = Instantiate(
                    holdPrefab,
                    holdPosition.position,
                    holdPosition.rotation,
                    holdPosition
                );
            }
        }
    }


    void DropObject()
    {
        Destroy(heldObject);
        heldObject = null;

        if (originalObject != null)
        {
            foreach (Renderer r in originalObject.GetComponentsInChildren<Renderer>())
            {
                r.enabled = true;
            }

            Collider col = originalObject.GetComponent<Collider>();
            if (col) col.enabled = true;

            originalObject = null;
        }
    }

}
