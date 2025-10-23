using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ComputerController : MonoBehaviour
{
    [Header("UI Screens (assign in inspector)")]
    public GameObject biosPanel;            // full-screen BIOS text/animation
    public GameObject loginPanel;           // login UI
    public GameObject desktopPanel;         // desktop UI (main layout)
    public GameObject popupInsertFloppy;    // small popup UI inside desktopPanel (disable by default)

    [Header("Boot settings")]
    public float bootDuration = 2f;
    public Text biosText; // optional for typed text

    private void Start()
    {
        HideAll();
    }

    public void HideAll()
    {
        biosPanel.SetActive(false);
        loginPanel.SetActive(false);
        desktopPanel.SetActive(false);
        popupInsertFloppy.SetActive(false);
    }

    public IEnumerator RunBootSequenceCoroutine(bool isSecondBoot)
    {
        HideAll();
        biosPanel.SetActive(true);
        // (optional) you could run animated BIOS text here
        yield return new WaitForSeconds(bootDuration);
        biosPanel.SetActive(false);
    }

    public void ShowLogin()
    {
        HideAll();
        loginPanel.SetActive(true);
    }

    public void ShowDesktopWithPopup()
    {
        HideAll();
        desktopPanel.SetActive(true);
        popupInsertFloppy.SetActive(true);

        GameFlowManager.Instance.RequestExitDuringPopup();
    }

    public void ShowMainDesktop()
    {
        HideAll();
        desktopPanel.SetActive(true);
        popupInsertFloppy.SetActive(false);

        GameFlowManager.Instance.currentState = GameFlowManager.PCState.MAIN_DESKTOP;

        InteractablePC pc = FindObjectOfType<InteractablePC>();
        if (pc != null)
        {
            pc.desktopUI = desktopPanel.GetComponent<Canvas>(); 
            pc.isDesktopOpen = true; 
            pc.LockToPC(); 
        }

        Debug.Log("[ComputerController] Main Desktop shown, player locked to PC.");
    }

}
