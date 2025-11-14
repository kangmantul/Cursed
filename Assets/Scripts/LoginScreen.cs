using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour
{
    public InputField usernameField;
    public Text feedbackText;

    public string correctUser = "a";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            TryLogin();
        }
    }

    public void TryLogin()
    {
        if (usernameField.text == correctUser)
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
