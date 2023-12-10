using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; // For RawImage
using TMPro;
using System.Collections;

public class MovieListingHandler : MonoBehaviour
{
    public TextMeshProUGUI movieName;
    public TextMeshProUGUI movieDescription;
    public TextMeshProUGUI releaseDate;
    public RawImage moviePoster; // Assign this in the inspector

    public static class Movie
    {
        public static string name;
        public static string description;
        //public static string posterPath;
        public static string releaseDate;
        // Assume you have an array for cast
        public static string[] cast;
    }

    void Start()
    {
        DisplayMovieData();
    }

    public void DisplayMovieData()
    {
        movieName.text = Movie.name;
        movieDescription.text = Movie.description;
        releaseDate.text = Movie.releaseDate;

        // Start coroutine to download and display movie poster
        //StartCoroutine(DownloadImage(Movie.posterPath));
        
        // Display cast using the string of arrays for the cast
        // This part would depend on how you want to format and display the cast information
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
}