using UnityEngine;
using Zork.Common;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnityOutputService : MonoBehaviour, IOutputService
{
    [SerializeField]
    private TextMeshProUGUI TextLinePrefab;

    [SerializeField]
    private Image NewLinePrefab;

    [SerializeField]
    private Transform ContentTransform;

    public void Write(object obj)
    {
        ParseAndWriteLine(obj.ToString());
    }

    public void Write(string message)
    {
        ParseAndWriteLine(message.ToString());
    }

    public void WriteLine(object obj)
    {
        ParseAndWriteLine(obj.ToString());
    }

    public void WriteLine(string message)
    {
        ParseAndWriteLine(message.ToString());
    }

    private void ParseAndWriteLine(string message)
    {
        var textLine = Instantiate(TextLinePrefab, ContentTransform);
        textLine.text = message;
        _entries.Add(textLine.gameObject);
    }

    private List<GameObject> _entries = new List<GameObject>();
}
