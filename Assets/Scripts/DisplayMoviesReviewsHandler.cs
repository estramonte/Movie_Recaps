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

public class DisplayMoviesReviewsHandler : MonoBehaviour
{
    // general info
    public GameObject reviewPrefab;
    public Transform contentPanel;
    private string apiUrl = "http://localhost:3000/api/reviews/"; 
    
    
    
    [System.Serializable]
    public class Review
    {
        public string movieName;
        public string comments;
        public string id;
        public string genre1Rating;
        public string genre2Rating;
        public string genre3Rating;
        public string genre1;
        public string genre2;
        public string genre3;
        public Action onMovieNameFetched; // Callback action
    }


    
    public class ReviewsApiResponse {
        public string message;
        public List<Review> result;
    }

    void Start()
    {
        StartCoroutine(GetUserReviewsCoroutine(MovieListingHandler.Movie.id)); 
    }
    
    IEnumerator GetUserReviewsCoroutine(string filmID) {
        string userReviewsUrl = apiUrl + "?film_id=" + filmID;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(userReviewsUrl)) {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("Error: " + webRequest.error);
            } else {
                ProcessReviews(webRequest.downloadHandler.text);
            }
        }
    }
    
    void ProcessReviews(string json) {
    var jsonObject = JObject.Parse(json);
    var reviewsArray = jsonObject["result"] as JArray;

    if (reviewsArray != null) {
        var reviewsList = new List<Review>();
        foreach (JObject reviewData in reviewsArray) {
            var review = new Review();
            review.comments = reviewData["comments"].ToString().Trim('"');
            review.id = reviewData["id"].ToString().Trim('"');

            // Prepare to collect the first three non-null genres and their ratings
            var genresFound = 0;

            // Start checking genres after "comments"
            var properties = reviewData.Properties().SkipWhile(p => p.Name != "comments").Skip(1);

            foreach (var property in properties) {
                var genreRating = property.Value?.ToObject<float?>();

                if (genreRating.HasValue) {
                    // Assign the genre and its rating to the next available slot
                    switch (genresFound) {
                        case 0:
                            review.genre1 = property.Name;
                            review.genre1Rating = genreRating.Value.ToString();
                            break;
                        case 1:
                            review.genre2 = property.Name;
                            review.genre2Rating = genreRating.Value.ToString();
                            break;
                        case 2:
                            review.genre3 = property.Name;
                            review.genre3Rating = genreRating.Value.ToString();
                            break;
                    }

                    genresFound++;
                    if (genresFound == 3) {
                        // Found three genres, no need to check further
                        break;
                    }
                }
            }

            reviewsList.Add(review);
        }
        DisplayReviews(reviewsList);
    }
}



    Dictionary<string, float> ExtractGenresAndRatings(JObject reviewData) {
        var genres = new Dictionary<string, float>();
        foreach (var prop in reviewData.GetType().GetProperties()) {
            if (prop.PropertyType == typeof(float?) && prop.GetValue(reviewData) is float rating) {
                genres.Add(prop.Name, rating);
            }
        }
        return genres;
    }

    void DisplayReviews(List<Review> reviews) {
        foreach (Review review in reviews) {
            GameObject newReview = Instantiate(reviewPrefab, contentPanel);
            MovieReviewPrefabHandler handler = newReview.GetComponent<MovieReviewPrefabHandler>();
            handler.SetData(review);
        }
    }



    
}
