using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            PlayAgain();
        }
    }

    public void PlayAgain()
    {
        DisableFPSControls();

        ResetGameState();

        SceneManager.LoadScene(mainMenuSceneName);
    }

    void ResetGameState()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.hasFloppy = false;
            GameStateManager.Instance.floppyInserted = false;
            GameStateManager.Instance.desktopUnlocked = false;

            typeof(GameStateManager)
                .GetField("unlockedApps", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(GameStateManager.Instance, new System.Collections.Generic.HashSet<string>());

            typeof(GameStateManager)
                .GetField("unlockedClues", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(GameStateManager.Instance, new System.Collections.Generic.HashSet<string>());
        }

        FindObjectOfType<AudioManager>()?.StopAll();
        Time.timeScale = 1f;

        Debug.Log("[Ending] Game State RESET, kembali ke menu.");
    }

    void DisableFPSControls()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var movement = FindObjectOfType<FirstPersonMovement>();
        if (movement != null) movement.enabled = false;

        var look = FindObjectOfType<FirstPersonLook>();
        if (look != null) look.enabled = false;

        Debug.Log("[Ending] FPS controls disabled + cursor unlocked.");
    }
}
