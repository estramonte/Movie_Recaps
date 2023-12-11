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

    public TextMeshProUGUI AreYouSure;
    public Image areyousureBackground;

    public TextMeshProUGUI bigUsername;
    public TextMeshProUGUI id;
    public TextMeshProUGUI smallusername;
    public TextMeshProUGUI password;
    public TextMeshProUGUI dob;
    public TextMeshProUGUI email;
    
    // Start is called before the first frame update
    void Start()
    {
        AreYouSure.text = " ";
        areyousureBackground.enabled = false;
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
        
        // get
    }

    public void onViewReviewsButton()
    {
        SceneManager.LoadScene("Your Reviews");
    }

    public void onUpdateUserInformationButton()
    {
        ProfileCanvas.SetActive(false);
        UpdateInformationCanvas.SetActive(true);
        
        // ask user to click buttons of attributes they want to update
        // have them input new information
    }

    public void onDeleteAccountButton()
    {
        AreYouSure.text = "Are You Sure?";
        areyousureBackground.enabled = true;
        
        // set yes and no buttons active
    }

    public void onYesButton()
    {
        // delete account
        
        
        User.id = string.Empty;
        User.username = string.Empty;
        User.password = string.Empty;
        User.dob = string.Empty;
        User.email = string.Empty;
        SceneManager.LoadScene("Title Screen");
    }

    public void onNoButton()
    {
        AreYouSure.text = " ";
        areyousureBackground.enabled = false;
        // set yes and no buttons inactive
    }

    public void onDownloadWatchlist()
    {
        // download watchlist to a csv file
    }
}
