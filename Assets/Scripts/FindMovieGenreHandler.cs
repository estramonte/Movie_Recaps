using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FindMovieGenreHandler : MonoBehaviour
{
    public TMP_Dropdown Genre1;
    public TMP_Dropdown Genre2;
    public TMP_Dropdown Genre3;

    private List<string> allGenres = new List<string> { "Romance", "Comedy", "Horror", "Thriller", "Adventure", "Action", "Drama" };

    void Start()
    {
        InitializeDropdown(Genre1, allGenres);
        InitializeDropdown(Genre2, allGenres);
        InitializeDropdown(Genre3, allGenres);
    }

    void InitializeDropdown(TMP_Dropdown dropdown, List<string> options)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }
    
    public void onSubmitMovieGenresForSearchButtonClick()
    {
        // logic needed to use genre's from dropdown 
        SceneManager.LoadScene("Search Results");
    }
}
