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

public class DisplayMoviesWatched : MonoBehaviour
{
    // general info
    public GameObject movieNamePrefab;
    public Transform contentPanel;
    private string apiUrl = "http://localhost:3000/api/watchlist/watched/?userId="; 
    
    [System.Serializable]
    public class FilmsWatchedResponse
    {
        public string message;
        public List<watchedFilms> watchedFilms;
    }

    [System.Serializable]
    public class watchedFilms
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
        StartCoroutine(GetUsersMoviesWatchedCoroutine()); 
    }
    
    IEnumerator GetUsersMoviesWatchedCoroutine() {
        string userReviewsUrl = apiUrl + ProfileHandler.User.id;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(userReviewsUrl)) {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("Error: " + webRequest.error);
            } else {
                DisplayNames(webRequest.downloadHandler.text);
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }

    void DisplayNames(string json) {
        // Clear previous entries
        foreach (Transform child in contentPanel) {
            Destroy(child.gameObject);
        }

        // Deserialize JSON string into the FilmsWatchedResponse object
        FilmsWatchedResponse response = JsonConvert.DeserializeObject<FilmsWatchedResponse>(json);

        // Loop through each film and instantiate a new prefab
        foreach (watchedFilms film in response.watchedFilms) {
            // Instantiate the prefab as a child of the content panel
            GameObject movieNameObject = Instantiate(movieNamePrefab, contentPanel);

            // Find the TextMeshProUGUI in the instantiated prefab and set the movie title
            TextMeshProUGUI textComponent = movieNameObject.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null) {
                textComponent.text = film.title;
            } else {
                Debug.LogError("TextMeshProUGUI component not found on the prefab.");
            }
        }
    }


}
