using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class SignUpHandler : MonoBehaviour
{
    public TMP_InputField EmailInputField;
    public TMP_InputField DOBInputField;
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;

    // URL to which you'll send the sign-up request
    private string signUpUrl = "http://192.168.1.26:3000/api/users/signup"; // Update with your actual server URL

    public void onSignUpSubmitButtonClicked()
    {
        string email = EmailInputField.text;
        string dob = DOBInputField.text;
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        // Start the coroutine to sign the user up
        StartCoroutine(SignUpUser(email, dob, username, password));
    }

    private IEnumerator SignUpUser(string email, string dob, string username, string password)
    {
        // Create a form and add the fields as per the API requirements
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("dob", dob);
        form.AddField("username", username);
        form.AddField("password", password);

        // Create a UnityWebRequest to send the form
        using (UnityWebRequest www = UnityWebRequest.Post(signUpUrl, form))
        {
            // Send the request and wait for a response
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error during sign-up: " + www.error);
                // Handle any errors here - show a message to the user, etc.
            }
            else
            {
                Debug.Log("Sign up successful!");
                // Optionally process the received JSON data
                ProcessSignUpResponse(www.downloadHandler.text);

                // Handle the successful sign-up - for example, by loading the main menu
                SceneManager.LoadScene("Main Menu");
            }
        }
    }

    private void ProcessSignUpResponse(string jsonData)
    {
        // Here you would parse the JSON data and extract the info you need
        Debug.Log("Received JSON data: " + jsonData);
        // Add your JSON processing here
        // For example, you could decode it to a C# object using JsonUtility or another JSON library
    }
}
