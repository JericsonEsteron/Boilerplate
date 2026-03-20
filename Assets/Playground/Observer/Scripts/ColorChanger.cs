using System;
using EventMessage;
using Events;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Image _image;
    private IDisposable _subscription;

    private void Awake()
    {
        if (_image == null)
            _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _subscription = EventMessenger.Default.Subscribe<ButtonPressedEvent>(OnButtonPressed);
    }

    private void OnDisable()
    {
        _subscription?.Dispose();
        _subscription = null;
    }

    private void OnButtonPressed(ButtonPressedEvent buttonPressedEvent)
    {
        Debug.Log("BUTTON PRESSED " + buttonPressedEvent.Message);

        if (_image != null)
            _image.color = UnityEngine.Random.ColorHSV();
    }

    private void OnValidate()
    {
        if (_image == null)
            _image = GetComponent<Image>();
    }
}
