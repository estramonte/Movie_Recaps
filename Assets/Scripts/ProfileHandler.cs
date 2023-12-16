using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class ProfileHandler : MonoBehaviour
{
    
    private string updateUrl = "http://localhost:3000/api/users/update";
    private string deleteUrl = "http://localhost:3000/api/users/delete";

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
    public GameObject UserStatisticsCanvas;

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
        UserStatisticsCanvas.SetActive(false);
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
        StartCoroutine(UpdateUserCoroutine(updatedUsername, updatedPassword, updatedEmail, updatedDOB));
    }
    
    private IEnumerator UpdateUserCoroutine(string username, string password, string email, string dob)
    {
        WWWForm form = new WWWForm();
        
        form.AddField("id", User.id);

        // Add the user's updated information to the form if they are not null or empty
        if (!string.IsNullOrEmpty(username))
        {
            form.AddField("username", username);
        }
        if (!string.IsNullOrEmpty(password))
        {
            form.AddField("password", password);
        }
        if (!string.IsNullOrEmpty(email))
        {
            form.AddField("email", email);
        }
        if (!string.IsNullOrEmpty(dob))
        {
            form.AddField("dob", dob);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(updateUrl, form))
        {
            // Send the request and wait for a response
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error updating user: " + www.error);
            }
            else
            {
                Debug.Log("User updated successfully");
            }
        }
    }

    public void onDeleteAccountButton()
    {
        AreYouSureCanvas.SetActive(true);
        ProfileCanvas.SetActive(false);
    }

    public void onYesButton()
    {
        // delete account 
        StartCoroutine(DeleteUserCoroutine(User.id, User.username, User.password));

        
        User.id = string.Empty;
        User.username = string.Empty;
        User.password = string.Empty;
        User.dob = string.Empty;
        User.email = string.Empty;
        SceneManager.LoadScene("Title Screen");
    }
    
    private IEnumerator DeleteUserCoroutine(string userId, string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", userId);
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(deleteUrl, form))
        {
            www.method = "DELETE"; // Manually set the method to DELETE

            // Send the request and wait for a response
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error deleting user: " + www.error);
            }
            else
            {
                Debug.Log("User deleted successfully");
            }
        }
    }


    public void onNoButton()
    {
        AreYouSureCanvas.SetActive(false);
        ProfileCanvas.SetActive(true);
    }

    public void onDownloadWatchlist()
    {
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

    public void onSeeUserStatistics()
    {
        UserStatisticsCanvas.SetActive(true);
        ProfileCanvas.SetActive(false);
    }
}
