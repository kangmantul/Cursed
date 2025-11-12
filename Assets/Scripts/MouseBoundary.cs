using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseBoundary : MonoBehaviour
{
    [Header("Panel Hitam")]
    public RectTransform leftPanel;
    public RectTransform rightPanel;
    public Canvas mainCanvas; // CanvasDesktop

    private Camera uiCamera;
    private GraphicRaycaster[] raycasters;
    private bool inputBlocked;

    void Start()
    {
        if (mainCanvas.renderMode == RenderMode.ScreenSpaceCamera)
            uiCamera = mainCanvas.worldCamera;

        // Ambil semua GraphicRaycaster di anak-anak CanvasDesktop
        raycasters = GetComponentsInChildren<GraphicRaycaster>(includeInactive: true);
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        bool overLeft = RectTransformUtility.RectangleContainsScreenPoint(leftPanel, mousePos, uiCamera);
        bool overRight = RectTransformUtility.RectangleContainsScreenPoint(rightPanel, mousePos, uiCamera);

        if (overLeft || overRight)
        {
            if (!inputBlocked)
                BlockInput(true);
        }
        else
        {
            if (inputBlocked)
                BlockInput(false);
        }
    }

    private void BlockInput(bool state)
    {
        inputBlocked = state;

        // Aktif/nonaktif semua raycast UI
        foreach (var r in raycasters)
            r.enabled = !state;

        // Optional efek visual cursor
        Cursor.lockState = state ? CursorLockMode.Confined : CursorLockMode.None;

        // Kosongkan UI selection biar gak ketahan input
        EventSystem.current.SetSelectedGameObject(null);
    }
}
