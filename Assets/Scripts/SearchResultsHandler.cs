using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class SearchResultsHandler : MonoBehaviour
{
    public DisplayData displayDataScript; // Assign your DisplayData script here in inspector

    // Call this method when the scene is loaded
    void Start()
    {
        string movieSearch = MainMenuHandler.MovieSearchQuery;
        StartCoroutine(GetMoviesData(movieSearch));
    }

    private IEnumerator GetMoviesData(string movieSearch)
    {
        string url = "http://10.208.138.164:3000/api/movies/search?name=" + UnityWebRequest.EscapeURL(movieSearch);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonData = webRequest.downloadHandler.text;
                Debug.Log("JSON Data: " + jsonData); // Check the raw JSON response
                displayDataScript.ProcessJsonData(jsonData); // This will be called to process and display data
            }
            else
            {
                Debug.LogError("Error: " + webRequest.error);
            }
        }
    }

}