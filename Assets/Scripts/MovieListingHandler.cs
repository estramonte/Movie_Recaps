using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; // For RawImage
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using Newtonsoft.Json;

public class MovieListingHandler : MonoBehaviour
{
    // Canvases in Scene
    public GameObject movieDetails;
    public GameObject viewReviews;
    public GameObject viewExtraDetails;
    
    //Movie Details
    public TextMeshProUGUI movieName;
    public TextMeshProUGUI movieDescription;
    public TextMeshProUGUI releaseDate;
    public RawImage moviePoster; // Assign this in the inspector
    public TextMeshProUGUI info;
    public Image infoBackground;
    
    //View Extra Details
    public TextMeshProUGUI directorsNames;
    public GameObject characterDisplayPrefab;
    public Transform contentPanel;
    private string apiUrl = "http://localhost:3000/api/movies/details?id="; 
    public ScrollRect scrollRect;


    public static class Movie
    {
        public static string name;
        public static string id;
        public static string description;
        public static string posterPath;
        public static string releaseDate;
    }
    
    [System.Serializable]
    public class Actor
    {
        public int id;
        public string actor_name;
        public string profile_path;
        public string character_name;
    }

    [System.Serializable]
    public class Director
    {
        public int id;
        public string director_name;
        public string profile_path;
    }
    
    [System.Serializable]
    public class MovieDetailsResponse
    {
        public string message;
        public Result result;
    }
    
    [System.Serializable]
    public class Movies
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

    [System.Serializable]
    public class Result
    {
        public Movies movie;
        public List<Actor> actors;
        public List<Director> directors;
        public List<DisplayMoviesReviewsHandler.Review> reviews;
    }
    
    [System.Serializable]
    public class ReviewResponse
    {
        public string message;
        public ReviewResult[] result;
    }
    
    [System.Serializable]
    public class ReviewResult
    {
        public int id;
        public int film_id;
        public int user_id;
        // Add other fields if necessary
    }
    
    [System.Serializable]
    public class WatchedFilmsResponse
    {
        public string message;
        public WatchedFilm[] watchedFilms;
    }

    [System.Serializable]
    public class WatchedFilm
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
        info.text = " ";
        infoBackground.enabled = false;
        movieDetails.SetActive(true);
        viewReviews.SetActive(false);
        viewExtraDetails.SetActive(false);
        DisplayMovieData();
        StartCoroutine(AdjustScrollPosition());
        StartCoroutine(GetMovieDetailsCoroutine()); 
    }
    
    IEnumerator AdjustScrollPosition()
    {
        // Wait for end of frame to ensure all content has been loaded and layout updated
        yield return new WaitForEndOfFrame();

        // Set to 1 to scroll to the top
        scrollRect.verticalNormalizedPosition = 1f;
    }
    
    IEnumerator GetMovieDetailsCoroutine() {
        string movieDetailsUrl = apiUrl + Movie.id; // Use the movie's ID here
        using (UnityWebRequest webRequest = UnityWebRequest.Get(movieDetailsUrl)) {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("Error: " + webRequest.error);
            } else {
                DisplayMovieDetails(webRequest.downloadHandler.text);
            }
        }
    }

    void DisplayMovieDetails(string json) {
        MovieDetailsResponse response = JsonConvert.DeserializeObject<MovieDetailsResponse>(json);
        if (response.result != null) {
            // Display directors
            directorsNames.text = string.Join("\n", response.result.directors.Select(d => d.director_name));

            // Clear previous actors list
            foreach (Transform child in contentPanel) {
                Destroy(child.gameObject);
            }

            // Display actors and their characters
            foreach (Actor actor in response.result.actors) {
                GameObject characterObject = Instantiate(characterDisplayPrefab, contentPanel);
                TextMeshProUGUI characterText = characterObject.GetComponentInChildren<TextMeshProUGUI>();
                if (characterText != null) {
                    characterText.text = $"{actor.actor_name}: {actor.character_name}";
                }
            }
        }
    }

    public void DisplayMovieData()
    {
        movieName.text = Movie.name;
        movieDescription.text = Movie.description;
        releaseDate.text = Movie.releaseDate;

        // Start coroutine to download and display movie poster
        StartCoroutine(DownloadImage(Movie.posterPath));
    }

    private IEnumerator DownloadImage(string imageUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            moviePoster.texture = texture;
            // Optionally adjust the aspect ratio of the image
            AspectRatioFitter fitter = moviePoster.GetComponent<AspectRatioFitter>();
            if (fitter != null)
            {
                fitter.aspectRatio = (float)texture.width / texture.height;
            }
        }
        else
        {
            Debug.LogError("Failed to download image: " + request.error);
        }
    }

    public IEnumerator AddToWatchList()
    {
        // First, check if the movie has already been reviewed.
        string watchedMoviesUrl = $"http://localhost:3000/api/watchlist/watched/?userId={ProfileHandler.User.id}";
        UnityWebRequest watchedMoviesRequest = UnityWebRequest.Get(watchedMoviesUrl);
        yield return watchedMoviesRequest.SendWebRequest();

        if (watchedMoviesRequest.result == UnityWebRequest.Result.Success)
        {
            var responseText = watchedMoviesRequest.downloadHandler.text;
            var jsonResponse = JsonUtility.FromJson<WatchedFilmsResponse>(responseText);

            // Check if the watchedFilms array contains a film with the same id as Movie.id
            bool hasWatched = jsonResponse.watchedFilms.Any(film => film.id == int.Parse(Movie.id));

            if (hasWatched)
            {
                // The movie has been watched
                StartCoroutine(ShowInfo("You have already watched this movie."));
                yield break;
            }

        }
        else
        {
            Debug.LogError("Error checking watched movies: " + watchedMoviesRequest.error);
        }
        
        // If the movie hasn't been reviewed, proceed to add it to the watch list.
        WWWForm form = new WWWForm();
        form.AddField("userId", ProfileHandler.User.id);
        form.AddField("filmId", Movie.id);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/api/watchlist/add", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            var responseText = www.downloadHandler.text;
            var jsonResponse = JsonUtility.FromJson<ReviewResponse>(responseText);

            if (jsonResponse.message.Contains("success"))
            {
                StartCoroutine(ShowInfo("Movie added to Movies To Watch List"));
            }
            else
            {
                StartCoroutine(ShowInfo("Movie already in Movies To Watch List"));
            }
        }
        else
        {
            Debug.LogError("Error adding movie to watch list: " + www.error);
        }
    }
    
    private IEnumerator CheckReviewAndAddToWatchedList()
    {
        string watchedMoviesUrl = $"http://localhost:3000/api/watchlist/watched/?userId={ProfileHandler.User.id}";
        UnityWebRequest watchedMoviesRequest = UnityWebRequest.Get(watchedMoviesUrl);
        yield return watchedMoviesRequest.SendWebRequest();

        if (watchedMoviesRequest.result == UnityWebRequest.Result.Success)
        {
            var responseText = watchedMoviesRequest.downloadHandler.text;
            var jsonResponse = JsonUtility.FromJson<WatchedFilmsResponse>(responseText);

            // Check if the watchedFilms array contains a film with the same id as Movie.id
            bool hasWatched = jsonResponse.watchedFilms.Any(film => film.id == int.Parse(Movie.id));

            if (hasWatched)
            {
                // The movie has been watched
                StartCoroutine(ShowInfo("This movie is already on your Movies Watched List"));
            }
            else
            {
                // The movie has not been watched
                StartCoroutine(ShowInfo("You can add this movie to your Movies Watched List"));
                yield return new WaitForSeconds(3);
                SceneManager.LoadScene("Review a Movie");
            }
        }
        else
        {
            Debug.LogError("Error checking watched movies: " + watchedMoviesRequest.error);
        }
    }
    
    public void onAddToMoviesToWatchList()
    {
        StartCoroutine(AddToWatchList());
    }


    public void onAddToMoviesWatchedList()
    {
        StartCoroutine(CheckReviewAndAddToWatchedList());
    }

    public void onViewReviews()
    {
        // sets Canvas movieDetail inactive and Canvas viewReviews active
        // displays reviews of the movie on a scrollable panel
        movieDetails.SetActive(false);
        viewReviews.SetActive(true);
    }

    public void onViewExtraDetails()
    {
        // sets Canvas movieDetail inactive and Canvas viewExtraDetails active
        movieDetails.SetActive(false);
        viewExtraDetails.SetActive(true);
    }
    
    private IEnumerator ShowInfo(string message)
    {
        info.text = message;
        infoBackground.enabled = true;
        yield return new WaitForSeconds(3);
        info.text = " ";
        infoBackground.enabled = false;
    }

    public void onBackToMovieListingButton()
    {
        movieDetails.SetActive(true);
        viewReviews.SetActive(false);
        viewExtraDetails.SetActive(false);
    }
}