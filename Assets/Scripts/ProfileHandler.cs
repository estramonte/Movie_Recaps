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

    public TextMeshProUGUI bigUsername;
    public TextMeshProUGUI id;
    public TextMeshProUGUI smallusername;
    public TextMeshProUGUI password;
    public TextMeshProUGUI dob;
    public TextMeshProUGUI email;
    
    // Start is called before the first frame update
    void Start()
    {
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
        
        // get
    }

    public void onViewReviewsButton()
    {
        SceneManager.LoadScene("Your Reviews");
    }
}
