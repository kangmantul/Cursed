using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    public Transform holdPosition;
    public float pickupRange = 3f;

    private Rigidbody heldObject;

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
            heldObject.MovePosition(holdPosition.position);
            heldObject.MoveRotation(holdPosition.rotation);
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
                Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    heldObject = rb;
                    heldObject.useGravity = false;
                    heldObject.isKinematic = true; 
                }
            }
        }
    }

    void DropObject()
    {
        heldObject.useGravity = true;
        heldObject.isKinematic = false;
        heldObject = null;
    }
}
