using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class SearchResultsHandler : MonoBehaviour
{
    public static class SceneChangeManager
    {
        public static string LastCaller = string.Empty; // Initialize to an empty string
    }

    public enum SearchOption
    {
        BySearch,
        ByGenre
    }

    private SearchOption currentSearchOption;

    public void SetSearchOption(SearchOption option)
    {
        currentSearchOption = option;
    }
    
    public DisplayData displayDataScript; // Assign your DisplayData script here in inspector

    // Call this method when the scene is loaded
    void Start()
    {
        if (SceneChangeManager.LastCaller == "FindMovieGenreHandler")
        {
            currentSearchOption = SearchOption.ByGenre;
            string genre1 = FindMovieGenreHandler.Genre1Query;
            string genre2 = FindMovieGenreHandler.Genre2Query;
            string genre3 = FindMovieGenreHandler.Genre3Query;
            int limit = FindMovieGenreHandler.LimitQuery;
            StartCoroutine(GetMoviesDataGenre(genre1, genre2, genre3, limit));
        }
        else if (SceneChangeManager.LastCaller == "MainMenuHandler")
        {
            currentSearchOption = SearchOption.BySearch;
            string movieSearch = MainMenuHandler.MovieSearchQuery;
            StartCoroutine(GetMoviesData(movieSearch));
        }
    }

    private IEnumerator GetMoviesData(string movieSearch)
    {
        string url = "http://localhost:3000/api/movies/search?name=" + UnityWebRequest.EscapeURL(movieSearch);
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

    private IEnumerator GetMoviesDataGenre(string genre1, string genre2, string genre3, int limit)
    {
        string lim = limit.ToString();
        string url = "http://localhost:3000/api/movies/genre?genre1=" + UnityWebRequest.EscapeURL(genre1) + 
                     "&genre2=" + UnityWebRequest.EscapeURL(genre2) + "&genre3=" + UnityWebRequest.EscapeURL(genre3) + 
                     "&limit=" +UnityWebRequest.EscapeURL(lim);
        
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