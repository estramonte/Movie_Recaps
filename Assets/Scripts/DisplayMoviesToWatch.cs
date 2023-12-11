using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DisplayMoviesToWatch : MonoBehaviour
{
    // general info
    public GameObject movieNamePrefab;
    public Transform contentPanel;
    private string apiUrl = "http://localhost:3000/api/watchlist/?userId="; 
    
    [System.Serializable]
    public class FilmsToWatchResponse
    {
        public string message;
        public List<FilmToWatch> filmsToWatch;
    }

    [System.Serializable]
    public class FilmToWatch
    {
        public int id;
        public string original_language;
        public string overview;
        public string poster_path;
        public string release_date;
        public long revenue;
        public string tagline;
        public string title;
    }

    
    void Start()
    {
        StartCoroutine(GetUsersMoviesToWatchCoroutine()); 
    }
    
    IEnumerator GetUsersMoviesToWatchCoroutine() {
        string userReviewsUrl = apiUrl + ProfileHandler.User.id;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(userReviewsUrl)) {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("Error: " + webRequest.error);
            } else {
                DisplayNames(webRequest.downloadHandler.text);
            }
        }
    }

    void DisplayNames(string json) {
        // Deserialize JSON string into the FilmsToWatchResponse object
        FilmsToWatchResponse response = JsonConvert.DeserializeObject<FilmsToWatchResponse>(json);

        // Clear previous list
        foreach (Transform child in contentPanel) {
            Destroy(child.gameObject);
        }
        
        Debug.Log(response);

        // Loop through each film and instantiate a new prefab
        foreach (FilmToWatch film in response.filmsToWatch) {
            // Instantiate the prefab as a child of the content panel
            GameObject movieNameObject = Instantiate(movieNamePrefab, contentPanel);

            // Set the name text to the film's title
            TextMeshProUGUI movieNameText = movieNameObject.GetComponentInChildren<TextMeshProUGUI>();
            if (movieNameText != null) {
                movieNameText.text = film.title;
            }
        }
    }

}
