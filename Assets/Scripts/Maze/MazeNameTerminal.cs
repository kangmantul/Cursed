using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MazeNameTerminal : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inputCanvas;
    public TMP_InputField inputField;
    public Button submitButton;
    public MazeDoorController linkedDoor;
    public string correctAnswer = "LUNA";

    private bool playerInside = false;

    void Start()
    {
        inputCanvas.SetActive(false);
        submitButton.onClick.AddListener(CheckAnswer);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            inputCanvas.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log("[MazeTerminal] Player dekat terminal — input aktif.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            inputCanvas.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Log("[MazeTerminal] Player menjauh — input ditutup.");
        }
    }

    void CheckAnswer()
    {
        if (!playerInside) return;

        string input = inputField.text.Trim().ToUpper();
        if (input == correctAnswer.ToUpper())
        {
            Debug.Log("[MazeTerminal] Jawaban benar — membuka pintu.");
            linkedDoor.OpenDoor();
            inputCanvas.SetActive(false);
            MazeDialogueSystem.Instance.Play("Note1");
        }
        else
        {
            Debug.Log("[MazeTerminal] Jawaban salah.");
        }

        inputField.text = "";
    }
}
