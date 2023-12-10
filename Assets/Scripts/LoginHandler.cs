using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginHandler : MonoBehaviour
{
    // Example of a class to represent the JSON data structure
    [System.Serializable]
    public class LoginResponse
    {
        public string message;
        public Result result;
    }

    [System.Serializable]
    public class Result
    {
        public string id;
        public string username;
        public string password; // Consider security implications
        public string email;
        public string dob;
    }

    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    
    private string id;
    private string username;
    private string password;
    private string dob;
    private string email;

    // URL to which you'll send the login request
    private string loginUrl = "http://localhost:3000/api/users/login"; // Update with your actual server URL

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
        Debug.Log("Received JSON data: " + jsonData);

   
    // Deserialize the JSON data
    LoginResponse response = JsonUtility.FromJson<LoginResponse>(jsonData);

        // Validate and assign data
        if(response != null && response.result != null)
    {
        ProfileHandler.User.id = response.result.id;
        ProfileHandler.User.username = response.result.username;
        // Consider security implications for password
        ProfileHandler.User.password = response.result.password;
        ProfileHandler.User.dob = response.result.dob;
        ProfileHandler.User.email = response.result.email;
    }
}

}
