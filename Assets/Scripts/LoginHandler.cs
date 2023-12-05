using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginHandler : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;

    // URL to which you'll send the login request
    private string loginUrl = "http://192.168.1.26:3000/api/users/login"; // Update with your actual server URL

    public void onLoginSubmitButtonClicked()
    {
        // Store the username and password values from the input fields.
        string username = usernameInputField.text;
        string password = passwordInputField.text;
        
        // Start the coroutine to log the user in
        StartCoroutine(LoginUser(username, password));
    }

    private IEnumerator LoginUser(string username, string password)
    {
        // Create a form and add the fields
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        // Create a UnityWebRequest to send the form
        using (UnityWebRequest www = UnityWebRequest.Post(loginUrl, form))
        {
            // Send the request and wait for a response
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error during login: " + www.error);
                // Handle any errors here - show a message to the user, etc.
            }
            else
            {
                Debug.Log("Login successful!");
                // Optionally process the received JSON data
                ProcessLoginResponse(www.downloadHandler.text);

                // Handle the successful login - for example, by loading the main menu
                SceneManager.LoadScene("Main Menu");
            }
        }
    }

    private void ProcessLoginResponse(string jsonData)
    {
        // Here you would parse the JSON data and extract the info you need
        // For example, you could decode it to a C# object using JsonUtility or another JSON library
        Debug.Log("Received JSON data: " + jsonData);
        // Add your JSON processing here
    }
}
