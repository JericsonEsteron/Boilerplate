using Events;
using UnityEngine;
using UnityEngine.UI;
using EventMessage;

public class ButtonPresser : MonoBehaviour
{
    [SerializeField] Button button;

    private void Start() 
    {
        button.onClick.AddListener(Publish);   
    }

    private void Publish()
    {
        
        Debug.Log("BUTTON PRESSED PUBLISH");
        EventMessenger.Default.Publish<ButtonPressedEvent>(new ButtonPressedEvent("Hallo"));
        EventMessenger.Default.GetDictionary();
    }
}
