using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CenotaphAppController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject appWindow;              // Panel utama aplikasi Cenotaph
    public TMP_InputField usernameField;      // Input username (TMP)
    public TMP_InputField passwordField;      // Input password (TMP)
    public Button loginButton;                // Tombol login
    public TMP_Text feedbackText;             // Status feedback (TMP)

    [Header("App Settings")]
    public string correctUsername = "alex.sun";
    public string correctPassword = "luna.tulip";
    public string nextSceneName = "Cenotaph";

    [Header("Audio Keys (Optional)")]
    public string sfxOpen = "openfile";
    public string sfxClose = "closefile";
    public string sfxLocked = "locked";

    private bool isOpen = false;

    void Start()
    {
        if (appWindow != null)
            appWindow.SetActive(false);

        if (loginButton != null)
            loginButton.onClick.AddListener(OnLoginPressed);

        if (feedbackText != null)
            feedbackText.text = "";
    }

    // === Dipanggil saat player klik icon aplikasi ===
    public void OnOpenButtonClicked()
    {
        if (!GameStateManager.Instance.floppyInserted)
        {
            Debug.Log("[CenotaphApp] Terkunci — butuh floppy untuk membuka aplikasi.");
            FindObjectOfType<AudioManager>()?.PlaySFX(sfxLocked);
            return;
        }

        if (appWindow != null)
        {
            appWindow.SetActive(true);
            appWindow.transform.SetAsLastSibling();
            isOpen = true;
            FindObjectOfType<AudioManager>()?.PlaySFX(sfxOpen);
            Debug.Log("[CenotaphApp] Aplikasi Cenotaph dibuka.");
        }
    }

    // === Dipanggil dari tombol Close di UI ===
    public void OnCloseButtonClicked()
    {
        if (appWindow != null)
        {
            appWindow.SetActive(false);
            isOpen = false;
            FindObjectOfType<AudioManager>()?.PlaySFX(sfxClose);
            Debug.Log("[CenotaphApp] Aplikasi Cenotaph ditutup.");
        }
    }

    // === Tombol login ditekan ===
    public void OnLoginPressed()
    {
        if (!isOpen)
        {
            Debug.LogWarning("[CenotaphApp] Tidak bisa login — app belum dibuka!");
            return;
        }

        string userInput = usernameField.text.Trim();
        string passInput = passwordField.text.Trim();

        if (string.IsNullOrEmpty(userInput) || string.IsNullOrEmpty(passInput))
        {
            ShowMessage("Please enter username and password.");
            FindObjectOfType<AudioManager>()?.PlaySFX(sfxLocked);
            return;
        }

        if (userInput.ToLower() == correctUsername.ToLower() &&
            passInput == correctPassword)
        {
            ShowMessage("Access granted...");
            FindObjectOfType<AudioManager>()?.PlaySFX(sfxOpen);
            StartCoroutine(LoadCenotaphScene());
        }
        else
        {
            ShowMessage("Access denied. Wrong credentials.");
            StartCoroutine(FlashError());
            FindObjectOfType<AudioManager>()?.PlaySFX(sfxLocked);
        }
    }

    void ShowMessage(string msg)
    {
        if (feedbackText != null)
            feedbackText.text = msg;
        Debug.Log("[CenotaphApp] " + msg);
    }

    IEnumerator LoadCenotaphScene()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FlashError()
    {
        if (feedbackText == null) yield break;

        Color original = feedbackText.color;
        for (int i = 0; i < 3; i++)
        {
            feedbackText.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            feedbackText.color = original;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
