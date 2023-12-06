using UnityEngine;
using TMPro;

public class MovieListingHandler : MonoBehaviour
{
    public TextMeshProUGUI movieNameText; // Assign in inspector
    public TextMeshProUGUI movieDetailsText; // Assign in inspector

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the movie name saved from the previous scene
        string selectedMovieName = PlayerPrefs.GetString("SelectedMovieName", "No Movie Selected");
        
        // Display the movie name
        movieNameText.text = selectedMovieName;
        
        // Now, you'd normally fetch the rest of the movie details from your data source
        // Here, we'll just hardcode some example details for demonstration
        string movieDetails = "Details for " + selectedMovieName;
        // You would replace the above line with actual data fetching logic

        // Display the movie details
        movieDetailsText.text = movieDetails;
    }
}