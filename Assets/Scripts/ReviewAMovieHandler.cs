using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json.Linq; // Make sure to import the namespace


public class ReviewAMovieHandler : MonoBehaviour
{
    // Leaving a Review Screen
    public GameObject LeaveAReview;
    public TMP_Dropdown Genre1;
    public TMP_Dropdown Genre2;
    public TMP_Dropdown Genre3;
    public TMP_InputField Genre1Rating;
    public TMP_InputField Genre2Rating;
    public TMP_InputField Genre3Rating;
    public TMP_InputField MovieName;
    public TMP_InputField MovieRating;
    public TMP_InputField Review;
    
    // Successful Review Screen
    public GameObject SuccessfulReview;
    public TextMeshProUGUI movieNameReview;
    public TextMeshProUGUI usernameReview;
    public TextMeshProUGUI reviewContent;
    public TextMeshProUGUI genre1RatingReview;
    public TextMeshProUGUI genre2RatingReview;
    public TextMeshProUGUI genre3RatingReview;
    
    private List<string> allGenres = new List<string>
    {
        "Action", "Adventure", "Animation", "Comedy", "Crime", "Drama", "Fantasy", "Family", "Fiction", "International",
        "Horror", "Mystery", "Romance", "SciFi", "Thriller", "TeleFilm", "Documentary", "History", "Music", "War",
        "Western"
    };
    
    void Start()
    {
        LeaveAReview.SetActive(true);
        SuccessfulReview.SetActive(false);
        InitializeDropdown(Genre1, allGenres);
        InitializeDropdown(Genre2, allGenres);
        InitializeDropdown(Genre3, allGenres);
    }
    void InitializeDropdown(TMP_Dropdown dropdown, List<string> options)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public void onSubmitButtonClick()
    {
        Debug.Log("Submitting movie review");
        string movieName = MovieName.text;
        // check if movie exists in db with movie name and set film_id equal to the result's film_id
        StartCoroutine(GetMoviesData(movieName));
    }
    
    public IEnumerator SubmitReview(int filmId, int userId)
    {
        //string movieRating = MovieRating.text;
        string review = Review.text;
        string genre1Rating = Genre1Rating.text;
        string genre2Rating = Genre2Rating.text;
        string genre3Rating = Genre3Rating.text;
        
        // Construct the form data
        WWWForm form = new WWWForm();
        form.AddField("film_id", filmId); // Assuming you have filmId variable
        form.AddField("user_id", userId); // Assuming you have userId variable
        form.AddField("comments", review);
        form.AddField(Genre1.options[Genre1.value].text, genre1Rating);
        form.AddField(Genre2.options[Genre2.value].text, genre2Rating);
        form.AddField(Genre3.options[Genre3.value].text, genre3Rating);

        // Send the POST request
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/api/reviews/add", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                // Handle error.
                Debug.LogError(www.error);
                // Check if the error is due to a duplicate entry
                if (www.downloadHandler.text.Contains("ER_DUP_ENTRY"))
                {
                    // Handle the duplicate entry case
                    Debug.LogError("A review for this film by this user already exists.");
                }
            }
            else
            {
                // Successfully posted the review
                Debug.Log("Review submitted successfully: " + www.downloadHandler.text);
                SwitchToReviewSuccessScreen();
            }
        }
    }
    

    private IEnumerator GetMoviesData(string movieSearch)
    {
        Debug.Log("Getting Movie Data for: " + movieSearch);
        string url = "http://localhost:3000/api/movies/search?name=" + UnityWebRequest.EscapeURL(movieSearch);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonData = webRequest.downloadHandler.text;
                Debug.Log("JSON Data: " + jsonData); // Check the raw JSON response

                // Parse the JSON data to extract the film id
                JObject jsonResponse = JObject.Parse(jsonData);
                JArray results = (JArray)jsonResponse["result"];
                if (results != null && results.Count > 0)
                {
                    int filmId = results[0].Value<int>("id");
                    Debug.Log("Film ID: " + filmId);

                    // Now you can use filmId to start the SubmitReview coroutine
                    StartCoroutine(SubmitReview(filmId, int.Parse(ProfileHandler.User.id))); // Uncomment and adjust as necessary
                }
            }
            else
            {
                Debug.LogError("Error: " + webRequest.error);
            }
        }
    }

    public void SwitchToReviewSuccessScreen()
    {
        LeaveAReview.SetActive(false);
        SuccessfulReview.SetActive(true);
        movieNameReview.text = MovieName.text;
        usernameReview.text = ProfileHandler.User.username;
        reviewContent.text = Review.text;
        genre1RatingReview.text = Genre1.options[Genre1.value].text + " " + Genre1Rating.text + "/5";
        genre2RatingReview.text = Genre2.options[Genre2.value].text + " " + Genre2Rating.text + "/5";
        genre3RatingReview.text = Genre3.options[Genre3.value].text + " " + Genre3Rating.text + "/5";
    }

}
