using System.IO;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CSVDownloader : MonoBehaviour
{
    private FilmsToWatchResponse filmsToWatch;
    private FilmsWatchedResponse filmsWatched;


    // Data structures for JSON deserialization
    [System.Serializable]
    public class FilmToWatch
    {
        public string title;
    }

    [System.Serializable]
    public class FilmsToWatchResponse
    {
        public FilmToWatch[] filmsToWatch;
    }

    [System.Serializable]
    public class WatchedFilm
    {
        public string title;
    }

    [System.Serializable]
    public class FilmsWatchedResponse
    {
        public WatchedFilm[] watchedFilms;
    }


    // Call this method to start the download process
    public void StartDownloadProcess()
    {
        StartCoroutine(GetUsersMoviesToWatchCoroutine());
        StartCoroutine(GetUsersMoviesWatchedCoroutine());
    }

    IEnumerator GetUsersMoviesToWatchCoroutine()
    {
        string userReviewsUrl = "http://localhost:3000/api/watchlist/?userId=" + ProfileHandler.User.id; // Adjust the endpoint as needed
        using (UnityWebRequest webRequest = UnityWebRequest.Get(userReviewsUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                filmsToWatch = JsonConvert.DeserializeObject<FilmsToWatchResponse>(webRequest.downloadHandler.text);
                CheckAndGenerateCSV(); // Check if both data are fetched
            }
        }
    }

    IEnumerator GetUsersMoviesWatchedCoroutine()
    {
        string userReviewsUrl = "http://localhost:3000/api/watchlist/watched/?userId=" + ProfileHandler.User.id; // Adjust the endpoint as needed
        using (UnityWebRequest webRequest = UnityWebRequest.Get(userReviewsUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                filmsWatched = JsonConvert.DeserializeObject<FilmsWatchedResponse>(webRequest.downloadHandler.text);
                CheckAndGenerateCSV(); // Check if both data are fetched
            }
        }
    }

    private void CheckAndGenerateCSV()
    {
        // Only generate CSV if both lists are available
        if (filmsToWatch != null && filmsWatched != null)
        {
            string filePath = GetFilePath();
            string csvData = GenerateCSVData();
            File.WriteAllText(filePath, csvData);
            Debug.Log("CSV file downloaded at: " + filePath);
        }
    }

    private string GenerateCSVData()
    {
        StringBuilder csvBuilder = new StringBuilder();

        // Add "Movies To Watch" section
        csvBuilder.AppendLine("Movies To Watch");
        foreach (FilmToWatch film in filmsToWatch.filmsToWatch)
        {
            csvBuilder.AppendLine(film.title);
        }

        // Add a blank line to separate sections
        csvBuilder.AppendLine();

        // Add "Movies Watched" section
        csvBuilder.AppendLine("Movies Watched");
        foreach (WatchedFilm film in filmsWatched.watchedFilms)
        {
            csvBuilder.AppendLine(film.title);
        }

        return csvBuilder.ToString();
    }
    
    private string GetFilePath()
    {
        // You can modify this path based on your requirements and platform
        return Application.persistentDataPath + "/YourWatchlist.csv";
    }
}