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
        EventMessageBearer.Default.Publish<ButtonPressedEvent>(new ButtonPressedEvent("Hallo"));
        EventMessageBearer.Default.GetDictionary();
    }
}
