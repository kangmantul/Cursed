using UnityEngine;
using TMPro;

public class MazeMemoryInteractor : MonoBehaviour
{
    [Header("Setup")]
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public Camera playerCamera;
    public TextMeshProUGUI promptText;

    private MazeMemoryFragment currentTarget;

    void Start()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
        else
            Debug.LogWarning("[MazeMemoryInteractor] Prompt Text belum di-assign!");
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactDistance, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            MazeMemoryFragment fragment = hit.collider.GetComponent<MazeMemoryFragment>();

            if (fragment != null)
            {
                currentTarget = fragment;
                if (promptText != null)
                {
                    promptText.gameObject.SetActive(true);
                    promptText.text = "Press <b>[E]</b> to Remember";
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentTarget.Collect();
                    currentTarget = null;

                    if (promptText != null)
                        promptText.gameObject.SetActive(false);
                }

                return;
            }
        }
        currentTarget = null;
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }
}
