using System;
using TMPro;
using UnityEngine;
using Zork.Common;

public class UnityInputService : MonoBehaviour, IInputService
{
    [SerializeField]
    private TMP_InputField InputField;

    public event EventHandler<string> InputReceived;

    public void ProcessInput()
    {
        if (string.IsNullOrWhiteSpace(InputField.text) == false)
        {
            InputReceived?.Invoke(this, InputField.text.Trim());
        }

        InputField.text = String.Empty;

    }

    public void SetFocus()
    {
        InputField.Select();
        InputField.ActivateInputField();
    }
}
