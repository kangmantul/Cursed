using System.Collections;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    public enum PCState
    {
        OFF,                
        FIRST_BOOT,         
        LOGIN,              
        DESKTOP_WITH_POPUP, 
        WAITING_FOR_FLOPPY, 
        SECOND_BOOT,        
        SECOND_LOGIN,       
        MAIN_DESKTOP       
    }

    public PCState currentState = PCState.OFF;
    public ComputerController computerController;

    public float biosImageHoldTime = 5f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // === BOOT PERTAMA ===
    public void StartFirstBoot()
    {
        SetState(PCState.FIRST_BOOT);
        StartCoroutine(RunBootThenLogin(false));
    }

    // === Jalankan boot sequence ===
    IEnumerator RunBootThenLogin(bool isSecondBoot)
    {
        yield return computerController.RunBootSequenceCoroutine(isSecondBoot);

        if (!isSecondBoot)
        {
            // boot pertama → tampilkan login
            FindObjectOfType<AudioManager>().PlaySFX("startcomputer");
            SetState(PCState.LOGIN);
            computerController.ShowLogin();
            MazeDialogueSystem.Instance.Play("pw");
        }
        else
        {
            // boot kedua → tampilkan login kedua
            SetState(PCState.SECOND_LOGIN);
            FindObjectOfType<AudioManager>().PlaySFX("startcomputer");
            computerController.ShowLogin();
        }
    }

    // === LOGIN SUKSES ===
    public void OnLoginSuccessful()
    {
        if (currentState == PCState.LOGIN)
        {
            // setelah login pertama → desktop dengan popup
            SetState(PCState.DESKTOP_WITH_POPUP);
            MazeDialogueSystem.Instance.Play("popup");
            computerController.ShowDesktopWithPopup();
        }
        else if (currentState == PCState.SECOND_LOGIN)
        {
            // setelah login kedua → desktop utama (unlocked)
            SetState(PCState.MAIN_DESKTOP);
            GameStateManager.Instance.desktopUnlocked = true;
            computerController.ShowMainDesktop();
        }
    }

    public void RequestExitDuringPopup()
    {
        SetState(PCState.WAITING_FOR_FLOPPY);
    }

    public void OnFloppyInserted()
    {
        GameStateManager.Instance.floppyInserted = true;
        StartCoroutine(SecondBootRoutine());
    }

    IEnumerator SecondBootRoutine()
    {
        SetState(PCState.SECOND_BOOT);
        yield return RunBootThenLogin(true);
    }

    public void SetState(PCState s)
    {
        currentState = s;
        Debug.Log("[GameFlow] State -> " + s);
    }

    public bool CanCloseDesktop()
    {
        return currentState == PCState.WAITING_FOR_FLOPPY || currentState == PCState.MAIN_DESKTOP;
    }

    // === SAAT DESKTOP DITUTUP ===
    public void OnDesktopClosed()
    {
        if (currentState == PCState.MAIN_DESKTOP)
        {
            SetState(PCState.OFF); 
            Debug.Log("[GameFlow] Desktop closed by player. State set to OFF (unlocked).");
        }
        else if (currentState == PCState.DESKTOP_WITH_POPUP)
        {
            SetState(PCState.WAITING_FOR_FLOPPY);
        }
    }

    public void OnPlayerInteractWithPC()
    {
        if (GameStateManager.Instance.desktopUnlocked)
        {
            SetState(PCState.MAIN_DESKTOP);
            computerController.ShowMainDesktop();
            var pc = FindObjectOfType<InteractablePC>();
            if (pc != null) pc.LockToPC();
            return;
        }

        if (currentState == PCState.OFF)
        {
            var pc = FindObjectOfType<InteractablePC>();
            if (pc != null) pc.LockToPC();
            StartFirstBoot();
        }
    }
}
