using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SearchByActorOrDirectorHandler : MonoBehaviour
{
    public GameObject SearchCanvas;
    public GameObject DataCanvas;

    public TMP_InputField ActorOrDirectorName;
    
    public DisplayData displayDataScript;
    
    void Start()
    {
        SearchCanvas.SetActive(true);
        DataCanvas.SetActive(false);
    }

    public void onSubmitButton()
    {
        SearchCanvas.SetActive(false);
        DataCanvas.SetActive(true);

        string actorOrDirectorName = ActorOrDirectorName.text;
        
        // submit search to
        // FIX ME NEED URL

        // display data
        //displayDataScript.ProcessJsonData(jsonData); // This will be called to process and display data

    }
    
}
