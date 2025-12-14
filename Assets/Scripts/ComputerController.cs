using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ComputerController : MonoBehaviour
{
    [Header("UI Screens")]
    public GameObject biosPanel_Normal;      
    public GameObject biosPanel_Corrupted;   
    public GameObject bootUpPanel;          
    public GameObject loginPanel;            
    public GameObject desktopPanel;          
    public GameObject popupInsertFloppy;     

    [Header("Boot Durations")]
    public float biosDuration = 3f;          
    public float bootUpDuration = 2f;        
    public float delayBetweenPhases = 0.5f;  

    [Header("Video Components")]
    public UnityEngine.Video.VideoPlayer bootUpVideo;
    public RawImage bootUpRawImage;

    private void Start()
    {
        HideAll();
    }

    public void HideAll()
    {
        if (biosPanel_Normal) biosPanel_Normal.SetActive(false);
        if (biosPanel_Corrupted) biosPanel_Corrupted.SetActive(false);
        if (loginPanel) loginPanel.SetActive(false);
        if (desktopPanel) desktopPanel.SetActive(false);
        if (popupInsertFloppy) popupInsertFloppy.SetActive(false);
    }


    // === URUTAN BOOT SEQUENCE (BIOS → BOOTUP → LOGIN) ===
    public IEnumerator RunBootSequenceCoroutine(bool isSecondBoot)
    {
        HideAll();

        // --- BIOS PHASE ---
        GameObject activeBiosPanel = isSecondBoot ? biosPanel_Corrupted : biosPanel_Normal;

        if (activeBiosPanel)
        {
            activeBiosPanel.SetActive(true);

            yield return new WaitForSeconds(biosDuration);

            activeBiosPanel.SetActive(false);
        }

        yield return new WaitForSeconds(delayBetweenPhases);

        // --- BOOTUP PHASE ---
        if (bootUpPanel)
        {
            bootUpPanel.SetActive(true);
            FindObjectOfType<AudioManager>().PlaySFX("bootup");

            if (bootUpVideo)
            {
                bootUpVideo.Stop();
                bootUpVideo.frame = 0;
                bootUpVideo.isLooping = false;

                yield return null;

                bootUpVideo.Play();
                Debug.Log("[ComputerController] BootUp video started properly.");
            }

            float startTime = Time.time;
            bool started = false;

            if (bootUpVideo)
            {
                while (!started && Time.time - startTime < 2f)
                {
                    if (bootUpVideo.texture != null)
                        started = true;
                    yield return null;
                }
            }

            if (bootUpVideo && bootUpVideo.length > 0)
                yield return new WaitForSeconds((float)bootUpVideo.length);
            else
                yield return new WaitForSeconds(bootUpDuration);

            if (bootUpVideo) bootUpVideo.Stop();
            bootUpPanel.SetActive(false);
        }

        yield return new WaitForSeconds(delayBetweenPhases);

        // --- LOGIN PHASE ---
        if (!isSecondBoot)
        {
            GameFlowManager.Instance.SetState(GameFlowManager.PCState.LOGIN);
            ShowLogin();
            MazeDialogueSystem.Instance.Play("pw");
        }
        else
        {
            GameFlowManager.Instance.SetState(GameFlowManager.PCState.SECOND_LOGIN);
            ShowLogin();
        }

        yield break;
    }

    // === LOGIN SCREEN ===
    public void ShowLogin()
    {
        HideAll();
        if (loginPanel) loginPanel.SetActive(true);
    }

    // === DESKTOP (dengan popup) ===
    public void ShowDesktopWithPopup()
    {
        HideAll();
        if (desktopPanel) desktopPanel.SetActive(true);
        if (popupInsertFloppy) popupInsertFloppy.SetActive(true);

        GameFlowManager.Instance.RequestExitDuringPopup();
    }

    // === DESKTOP UTAMA ===
    public void ShowMainDesktop()
    {
        HideAll();
        if (desktopPanel) desktopPanel.SetActive(true);
        if (popupInsertFloppy) popupInsertFloppy.SetActive(false);

        GameFlowManager.Instance.currentState = GameFlowManager.PCState.MAIN_DESKTOP;
        GameStateManager.Instance.desktopUnlocked = true;

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
