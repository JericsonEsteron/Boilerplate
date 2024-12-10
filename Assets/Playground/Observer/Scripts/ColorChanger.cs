using System.Collections;
using System.Collections.Generic;
using Messenger;
using Events;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] Image image;

    private void Awake() 
    {
        MessageBearer.Default.Subscribe<ButtonPressedEvent>(ButtonPressedEvent, this.gameObject);
    }

    private void ButtonPressedEvent(ButtonPressedEvent buttonPressedEvent)
    {
        Debug.Log("BUTTON PRESSED " + buttonPressedEvent.message);
    }
}
