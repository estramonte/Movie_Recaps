using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuHandler : MonoBehaviour
{
    public TMP_InputField MovieSearch;
    public static string MovieSearchQuery; // Static variable to pass the search query to the next scene

    public void onMovieRecapsLogoButtonClick()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void onSearchButtonClick()
    {
        MovieSearchQuery = MovieSearch.text;
        Debug.Log("Search Query: " + MovieSearchQuery); // Check the value being set
        SceneManager.LoadScene("Search Results");
    }


    public void onMoviesToWatchButtonClick()
    {
        SceneManager.LoadScene("Movies to Watch");
    }

    public void onMoviesWatchedButtonClick()
    {
        SceneManager.LoadScene("Movies Watched");
    }

    public void onFindAMovieByGenreButtonClick()
    {
        SceneManager.LoadScene("Find a Movie - G");
    }

    public void onReviewAMovieButtonClick()
    {
        SceneManager.LoadScene("Review a Movie");
    }

    public void onProfileButtonClick()
    {
        SceneManager.LoadScene("Profile");
    }
}
