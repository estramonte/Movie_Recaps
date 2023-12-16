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

public class UserStatsHandler : MonoBehaviour
{
    public TextMeshProUGUI favoriteActor;
    public TextMeshProUGUI favoriteDirector;
    public TextMeshProUGUI moviesWatchedCount;
    public TextMeshProUGUI moviesToWatchCount;

    private string apiUrl = "http://localhost:3000/api/users/stats?id=";
    
    [System.Serializable]
    public class Stats
    {
        public int watchedCount;
        public int watchlistCount;
        public FavoriteActor favoriteActor;
        public FavoriteDirector favoriteDirector;
    }

    [System.Serializable]
    public class FavoriteActor
    {
        public int id;
        public string actor_name;
        public int review_count;
    }

    [System.Serializable]
    public class FavoriteDirector
    {
        public int id;
        public string director_name;
        public int review_count;
    }

    [System.Serializable]
    public class UserStats
    {
        public string message;
        public Stats stats;
    }

    
    void Start()
    {
        StartCoroutine(GetUserReviewsCoroutine());
    }
    
    IEnumerator GetUserReviewsCoroutine() {
        string userReviewsUrl = apiUrl + ProfileHandler.User.id;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(userReviewsUrl)) {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("Error: " + webRequest.error);
            } else
            {
                ProcessStats(webRequest.downloadHandler.text);
            }
        }
    }

    public void ProcessStats(string json)
    {
        UserStats userStats = JsonUtility.FromJson<UserStats>(json);
        moviesWatchedCount.text = userStats.stats.watchedCount.ToString();
        moviesToWatchCount.text = userStats.stats.watchlistCount.ToString();
        favoriteActor.text = userStats.stats.favoriteActor.actor_name;
        favoriteDirector.text = userStats.stats.favoriteDirector.director_name;
    }
    
}
