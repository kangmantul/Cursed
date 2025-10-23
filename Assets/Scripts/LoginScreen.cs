using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public Text feedbackText;

    public string correctUser = "ozul";
    public string correctPass = "echo";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            TryLogin();
        }
    }

    public void TryLogin()
    {
        if (usernameField.text == correctUser && passwordField.text == correctPass)
        {
            feedbackText.text = "Access granted.";
            GameFlowManager.Instance.OnLoginSuccessful();
        }
        else
        {
            feedbackText.text = "Wrong credentials.";
        }
    }
}
