using UnityEngine;

public class InteractablePC : MonoBehaviour
{
    [Header("Setup")]
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public Canvas desktopUI;
    public Camera playerCamera;
    public GameObject playerController;

    public bool isDesktopOpen = false;

    private bool isClosing = false;

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
                    if (!isDesktopOpen)
                    {
                        isDesktopOpen = true;
                        desktopUI.gameObject.SetActive(true);
                        GameFlowManager.Instance.OnPlayerInteractWithPC();
                    }
                }
            }
        }

        if (isDesktopOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isClosing) return;

            if (GameFlowManager.Instance != null && GameFlowManager.Instance.CanCloseDesktop())
            {
                CloseDesktop();
                GameFlowManager.Instance.OnDesktopClosed();
            }
            else
            {
                Debug.Log("Cannot exit PC in this state: " + GameFlowManager.Instance.currentState);
            }
        }

        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactDistance, Color.red);
    }

    // === FUNGSI (BOOT) ===
    public void OpenDesktop()
    {
        if (isDesktopOpen) return;

        Debug.Log("Opening desktop (first boot)...");
        isDesktopOpen = true;
        desktopUI.gameObject.SetActive(true);

        LockToPC(); 

        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.StartFirstBoot();
        }
    }

    public void LockToPC()
    {
        // Kunci player, tampilkan cursor
        if (playerController != null)
        {
            var fm = playerController.GetComponent<FirstPersonMovement>();
            if (fm != null)
            {
                fm.speed = 0f;
                fm.IsRunning = false;
            }
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnlockFromPC()
    {
        if (playerController != null)
        {
            var fm = playerController.GetComponent<FirstPersonMovement>();
            if (fm != null)
            {
                fm.speed = 3f;
                fm.IsRunning = true;
            }
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // === CLOSE DESKTOP ===
    public void CloseDesktop()
    {
        if (!isDesktopOpen) return;

        if (isClosing) return; 

        isClosing = true;
        Debug.Log("Closing desktop...");
        isDesktopOpen = false;

        if (desktopUI != null) desktopUI.gameObject.SetActive(false);

        UnlockFromPC();
        // kecilkan kemungkinan race condition, reset isClosing di next frame
        // supaya jika ada callback lain yang memanggil CloseDesktop() segera tidak menyebabkan recursion
        StartCoroutine(ClearClosingFlagNextFrame());
    }

    private System.Collections.IEnumerator ClearClosingFlagNextFrame()
    {
        yield return null;
        isClosing = false;
    }

    // Fungsi bantuan yang dipakai GameFlowManager jika perlu 'force open' desktop
    public void ForceOpenDesktop()
    {
        if (isDesktopOpen) return;
        isDesktopOpen = true;
        if (desktopUI != null) desktopUI.gameObject.SetActive(true);
        LockToPC();
    }
}
