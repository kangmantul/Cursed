using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractablePC : MonoBehaviour
{
    [Header("Setup")]
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public Canvas desktopUI;
    public Camera playerCamera;
    public GameObject playerController; 

    private bool isDesktopOpen = false;

    void Start()
    {
        if (desktopUI != null)
            desktopUI.gameObject.SetActive(false);
        else
            Debug.LogWarning("Desktop UI belum di-assign!");

        if (playerController == null)
            Debug.LogWarning("Player Controller belum di-assign!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
            {
                if (hit.collider.CompareTag("Computer"))
                {
                    OpenDesktop();
                }
            }
        }

        if (isDesktopOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseDesktop();
        }

        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactDistance, Color.red);
    }

    public void OpenDesktop()
    {
        if (isDesktopOpen) return;

        Debug.Log("Opening desktop...");
        isDesktopOpen = true;
        desktopUI.gameObject.SetActive(true);

        // Disable player movement
        if (playerController != null)
            playerController.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseDesktop()
    {
        if (!isDesktopOpen) return;

        Debug.Log("Closing desktop...");
        isDesktopOpen = false;
        desktopUI.gameObject.SetActive(false);

        // Re-enable player movement
        if (playerController != null)
            playerController.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
