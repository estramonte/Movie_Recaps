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
    public TMP_Dropdown Limit;
    public static string Genre1Query;
    public static string Genre2Query;
    public static string Genre3Query;
    public static int LimitQuery;
    

    private List<string> allGenres = new List<string>
    {
        "Action", "Adventure", "Animation", "Comedy", "Crime", "Drama", "Fantasy", "Family", "Fiction", "International",
        "Horror", "Mystery", "Romance", "SciFi", "Thriller", "TeleFilm", "Documentary", "History", "Music", "War",
        "Western"
    };

    private List<string> allLimits = new List<string>
    {
        "5", "10", "15", "20", "25"
    };
    void Start()
    {
        InitializeDropdown(Genre1, allGenres);
        InitializeDropdown(Genre2, allGenres);
        InitializeDropdown(Genre3, allGenres);
        InitializeDropdown(Limit, allLimits);
    }

    void InitializeDropdown(TMP_Dropdown dropdown, List<string> options)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }
    
    public void onSubmitMovieGenresForSearchButtonClick()
    {
        // assign genre queries to strings
        Genre1Query = Genre1.options[Genre1.value].text;
        Genre2Query = Genre2.options[Genre2.value].text;
        Genre3Query = Genre3.options[Genre3.value].text;
        
        // assign limit query
        int limitValue = 0;
        switch (Limit.options[Limit.value].text)
        {
            case "5":
                limitValue = 5;
                break;
            case "10":
                limitValue = 10;
                    break;
            case "15":
                limitValue = 10;
                break;
            case "20":
                limitValue = 10;
                break;
            case "25":
                limitValue = 10;
                break;
            default:
                limitValue = 30;
                break;
        }
        LimitQuery = limitValue;
        
        SearchResultsHandler.SceneChangeManager.LastCaller = "FindMovieGenreHandler";
        SceneManager.LoadScene("Search Results");
    }
}
