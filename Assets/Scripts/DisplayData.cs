using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

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
        // Example data
        dataRows = new List<Dictionary<string, string>> {
            new Dictionary<string, string> {
                {"Name", "Alice"},
                {"Age", "29"},
                // More data...
            },
            new Dictionary<string, string> {
                {"Name", "Bob"},
                {"Age", "35"},
                {"Hair Color", "Brown"},
                // More data...
            },
            new Dictionary<string, string> {
                {"Name", "Mark"},
                {"Hair Color", "Red"},
                // More data...
            },
            new Dictionary<string, string> {
                {"Name", "Clark"},
                {"Age", "20"},
                {"DOB", "2005-06-06"},
                // More data...
            },
            new Dictionary<string, string> {
                {"Name", "Alex"},
                {"Age", "9"},
                {"DOB", "2003-03-03"}
                // More data...
            },
            new Dictionary<string, string> {
                {"Name", "Lina"},
                {"Age", "43"},
                {"Hair Color", "Blonde"},
                {"DOB", "1985-05-05"}
                // More data...
            }
            // Add more rows as needed
        };
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
}
