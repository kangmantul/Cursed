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
            SetState(PCState.LOGIN);
            computerController.ShowLogin();
            MazeDialogueSystem.Instance.Play("pw");
        }
        else
        {
            // boot kedua → tampilkan login kedua
            SetState(PCState.SECOND_LOGIN);
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

    // === KELUAR SAAT POPUP ===
    public void RequestExitDuringPopup()
    {
        SetState(PCState.WAITING_FOR_FLOPPY);
    }

    // === SAAT FLOPPY DIMASUKKAN ===
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

    // === Helper ganti state dengan debug ===
    void SetState(PCState s)
    {
        currentState = s;
        Debug.Log("[GameFlow] State -> " + s);
    }

    // === BOLEH CLOSE DESKTOP? ===
    public bool CanCloseDesktop()
    {
        return currentState == PCState.WAITING_FOR_FLOPPY || currentState == PCState.MAIN_DESKTOP;
    }

    // === SAAT DESKTOP DITUTUP ===
    public void OnDesktopClosed()
    {
        if (currentState == PCState.MAIN_DESKTOP)
        {
            // tetap unlocked — biarkan state MAIN_DESKTOP, tapi kita juga bisa set ke OFF
            // jika kamu ingin reopen langsung tanpa boot, tetap biarkan desktopUnlocked = true
            SetState(PCState.OFF); // kalau ingin PC dianggap "idle" tapi unlocked, ini OK
            Debug.Log("[GameFlow] Desktop closed by player. State set to OFF (unlocked).");
        }
        else if (currentState == PCState.DESKTOP_WITH_POPUP)
        {
            SetState(PCState.WAITING_FOR_FLOPPY);
        }
    }


    // === SAAT PLAYER TEKAN E DI DEPAN PC ===
    public void OnPlayerInteractWithPC()
    {
        // --- Jika sudah unlock main desktop ---
        if (GameStateManager.Instance.desktopUnlocked)
        {
            SetState(PCState.MAIN_DESKTOP);
            computerController.ShowMainDesktop();
            var pc = FindObjectOfType<InteractablePC>();
            if (pc != null) pc.LockToPC();
            return;
        }

        // --- Jika belum unlock, jalankan boot normal pertama ---
        if (currentState == PCState.OFF)
        {
            var pc = FindObjectOfType<InteractablePC>();
            if (pc != null) pc.LockToPC();
            StartFirstBoot();
        }
    }
}
