using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class DisplayData : MonoBehaviour
{
    public GameObject cellPrefab; // Assign in inspector
    public GridLayoutGroup gridLayoutGroup; // Assign in inspector

    private List<Dictionary<string, string>> dataRows; // Populate with actual data

    private void Start() {
        PopulateDataRows(); // Assuming this populates dataRows with your actual data
        CreateDynamicGrid();
    }

    private void PopulateDataRows() {
        string jsonData = @"{
            'message': 'Movies fetched successfully',
            'result': [
                // ... (Your JSON data goes here)
            ]
        }";

        JObject parsedData = JObject.Parse(jsonData);
        JArray movies = (JArray)parsedData["result"];
        
        dataRows = new List<Dictionary<string, string>>();

        foreach (JObject movie in movies) {
            var row = new Dictionary<string, string>();
            row.Add("ID", movie["id"].ToString());
            row.Add("Language", movie["original_language"].ToString());
            row.Add("Overview", movie["overview"].ToString());
            row.Add("Poster Path", movie["poster_path"].ToString());
            row.Add("Release Date", DateTime.Parse(movie["release_date"].ToString()).ToString("yyyy-MM-dd"));
            row.Add("Revenue", movie["revenue"].ToString());
            row.Add("Tagline", movie["tagline"]?.ToString() ?? "N/A");
            row.Add("Title", movie["title"].ToString());

            dataRows.Add(row);
        }
    }

    private void CreateDynamicGrid() {
        // Get all unique keys from dataRows to use as headers
        var headers = new HashSet<string>(dataRows.SelectMany(dict => dict.Keys)).ToList();

        // Configure GridLayoutGroup based on the number of headers
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = headers.Count;

        // Instantiate header cells
        foreach (var header in headers) {
            InstantiateCell(header);
        }

        // Instantiate data cells
        foreach (var row in dataRows) {
            foreach (var header in headers) {
                string cellValue = row.TryGetValue(header, out var value) ? value : "NULL";
                InstantiateCell(cellValue);
            }
        }
    }

    private void InstantiateCell(string text) {
        var cell = Instantiate(cellPrefab, gridLayoutGroup.transform);
        cell.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
    
    public void ProcessJsonData(string jsonData)
    {
        JObject parsedData = JObject.Parse(jsonData);
        JArray movies = parsedData["result"] as JArray;

        if (movies == null || !movies.Any())
        {
            Debug.LogError("No movies found or JSON format is incorrect: " + jsonData);
            return; // Exit if there are no movies to display
        }

        dataRows.Clear(); // Clear existing data

        foreach (JObject movie in movies)
        {
            var row = new Dictionary<string, string>();
            // ... populate the row as shown previously
            dataRows.Add(row);
        }

        CreateDynamicGrid(); // Call the method that creates the UI grid
    }

}
