using UnityEngine;
using UnityEngine.UI;

public class MySpaceAppController : MonoBehaviour
{
    [Header("UI")]
    public GameObject appWindow;           
    public Button openButton;             
    public Button closeButton;             
    public bool openAsDraggable = true;
    public ChatManager chatManager;        

    [Header("Options")]
    public bool lockPlayerOnOpen = true;   

    void Start()
    {
        if (openButton != null) openButton.onClick.AddListener(OnOpenButtonClicked);
        if (closeButton != null) closeButton.onClick.AddListener(CloseAppWindow);
        if (appWindow != null) appWindow.SetActive(false);
    }

    public void OnOpenButtonClicked()
    {
        if (!GameStateManager.Instance.floppyInserted)
        {
            Debug.Log("MySpace terkunci. Masukkan floppy terlebih dahulu.");
            return;
        }

        OpenAppWindow();
    }

    public void OpenAppWindow()
    {
        if (appWindow == null) return;

        appWindow.SetActive(true);

        if (lockPlayerOnOpen)
        {
            var ipc = FindObjectOfType<InteractablePC>();
            if (ipc != null) ipc.LockToPC();
        }

        var rt = appWindow.GetComponent<RectTransform>();
        if (rt != null) rt.SetAsLastSibling();
    }

    public void CloseAppWindow()
    {
        if (appWindow == null) return;

        appWindow.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("[AppWindow] App closed, still inside desktop mode.");
    }

}
