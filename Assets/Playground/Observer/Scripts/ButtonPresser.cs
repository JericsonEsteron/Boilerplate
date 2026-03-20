using Events;
using UnityEngine;
using UnityEngine.UI;
using EventMessage;

public class ButtonPresser : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Awake()
    {
        if (_button == null)
            _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (_button == null)
            throw new MissingReferenceException($"{GetType().Name} requires a Button reference.");

        _button.onClick.AddListener(Publish);
    }

    private void OnDisable()
    {
        if (_button != null)
            _button.onClick.RemoveListener(Publish);
    }

    private void Publish()
    {
        EventMessenger.Default.Publish<ButtonPressedEvent>(new ButtonPressedEvent("Hallo"));
    }
}
