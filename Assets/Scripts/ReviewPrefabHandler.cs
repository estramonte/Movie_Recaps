using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReviewPrefabHandler : MonoBehaviour
{
    public TextMeshProUGUI movieNameText;
    public TextMeshProUGUI reviewContentText;
    public TextMeshProUGUI genre1RatingText;
    public TextMeshProUGUI genre2RatingText;
    public TextMeshProUGUI genre3RatingText;

    public void SetData(DisplayYourReviewsHandler.Review review) {
        movieNameText.text = review.movieName;
        reviewContentText.text = review.comments;
        genre1RatingText.text = review.genre1 + " " + review.genre1Rating + "/5";
        genre2RatingText.text = review.genre2 + " " + review.genre2Rating + "/5";
        genre3RatingText.text = review.genre3 + " " + review.genre3Rating + "/5";
    }
    
    public void SetMovieName(string movieName)
    {
        // Assuming you have a TextMeshProUGUI field for the movie name
        movieNameText.text = movieName;
    }

}
