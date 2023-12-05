using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Add this line at the top of your script

public class LoginHandler : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;

    public void onLoginSubmitButtonClicked()
    {
        // Store the username and password values from the input fields.
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        // load main menu screen
        SceneManager.LoadScene("Main Menu");
    }
}
