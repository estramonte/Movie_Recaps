using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    // Call this method when the Login button is clicked
    public void OnLoginButtonClicked()
    {
        SceneManager.LoadScene("Login Screen");
    }

    // Call this method when the Sign Up button is clicked
    public void OnSignUpButtonClicked()
    {
        SceneManager.LoadScene("Sign Up Screen");
    }
}

