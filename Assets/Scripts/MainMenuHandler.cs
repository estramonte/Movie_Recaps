using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuHandler : MonoBehaviour
{
    public TMP_InputField MovieSearch;

    public void onMovieRecapsLogoButtonClick()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void onSearchButtonClick()
    {
        string movieSearch = MovieSearch.text;
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
