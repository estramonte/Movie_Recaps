using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovieReviewPrefabHandler : MonoBehaviour
{
    public TextMeshProUGUI userIDText;
    public TextMeshProUGUI reviewContentText;
    public TextMeshProUGUI genre1RatingText;
    public TextMeshProUGUI genre2RatingText;
    public TextMeshProUGUI genre3RatingText;

    public void SetData(DisplayMoviesReviewsHandler.Review review)
    {
        userIDText.text = "User: " + review.id;
        reviewContentText.text = review.comments;
        genre1RatingText.text = review.genre1 + " " + review.genre1Rating + "/5";
        genre2RatingText.text = review.genre2 + " " + review.genre2Rating + "/5";
        genre3RatingText.text = review.genre3 + " " + review.genre3Rating + "/5";
    }

}