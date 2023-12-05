using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Add this line at the top of your script

public class SignUpHandler : MonoBehaviour
{
    public TMP_InputField EmailInputField;
    public TMP_InputField DOBInputField;
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;

    public void onSignUpSubmitButtonClicked()
    {
        // Store the username and password values from the input fields.
        string email = EmailInputField.text;
        string DOB = DOBInputField.text; // maybe change to Date later
        string username = usernameInputField.text;
        string password = passwordInputField.text;
        
        // sign user up
        // send email, DOB, username, and password to http thing

        // load main menu screen
        SceneManager.LoadScene("Main Menu");
    }
}
