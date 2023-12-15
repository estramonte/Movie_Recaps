using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ProfileHandler : MonoBehaviour
{
    public static class User
    {
        public static string id = string.Empty;
        public static string username = string.Empty;
        public static string password = string.Empty;
        public static string dob = string.Empty;
        public static string email = string.Empty;
    }

    public GameObject ProfileCanvas;
    public GameObject UpdateInformationCanvas;
    public GameObject AreYouSureCanvas;

    public TextMeshProUGUI bigUsername;
    public TextMeshProUGUI id;
    public TextMeshProUGUI smallusername;
    public TextMeshProUGUI password;
    public TextMeshProUGUI dob;
    public TextMeshProUGUI email;
    
    // update info
    public InputField usernameUpdateInputField;
    public InputField passwordUpdateInputField;
    public InputField emailUpdateInputField;
    public InputField dobUpdateInputField;

    public CSVDownloader csvDownloader;
    
    // Start is called before the first frame update
    void Start()
    {
        usernameUpdateInputField.enabled = false;
        passwordUpdateInputField.enabled = false;
        emailUpdateInputField.enabled = false;
        dobUpdateInputField.enabled = false;
        AreYouSureCanvas.SetActive(false);
        ProfileCanvas.SetActive(true);
        UpdateInformationCanvas.SetActive(false);
        DisplayUserInformation();
    }

    public void DisplayUserInformation()
    {
        bigUsername.text = User.username + "!";
        id.text = User.id;
        smallusername.text = User.username;
        password.text = User.password;
        dob.text = User.dob.Substring(0, 10);
        email.text = User.email;
    }

    public void onViewReviewsButton()
    {
        SceneManager.LoadScene("Your Reviews");
    }

    public void onUpdateUserInformationButton()
    {
        ProfileCanvas.SetActive(false);
        UpdateInformationCanvas.SetActive(true);
    }

    public void onSubmitButton()
    {
        string updatedUsername = usernameUpdateInputField.text;
        string updatedPassword = passwordUpdateInputField.text;
        string updatedEmail = emailUpdateInputField.text;
        string updatedDOB = dobUpdateInputField.text;

        // check if the string is full and then update user info with url FIX ME
    }

    public void onDeleteAccountButton()
    {
        AreYouSureCanvas.SetActive(true);
        ProfileCanvas.SetActive(false);
    }

    public void onYesButton()
    {
        // delete account FIX ME
        
        User.id = string.Empty;
        User.username = string.Empty;
        User.password = string.Empty;
        User.dob = string.Empty;
        User.email = string.Empty;
        SceneManager.LoadScene("Title Screen");
    }

    public void onNoButton()
    {
        AreYouSureCanvas.SetActive(false);
        ProfileCanvas.SetActive(true);
    }

    public void onDownloadWatchlist()
    {
        // download watchlist to a csv file
        csvDownloader.StartDownloadProcess();
    }

    public void onUsernameUpdateButton()
    {
        usernameUpdateInputField.enabled = true;
    }
    
    public void onPasswordUpdateButton()
    {
        passwordUpdateInputField.enabled = true;
    }
    
    public void onEmailUpdateButton()
    {
        emailUpdateInputField.enabled = true;
    }
    
    public void onDOBUpdateButton()
    {
        dobUpdateInputField.enabled = true;
    }
}
