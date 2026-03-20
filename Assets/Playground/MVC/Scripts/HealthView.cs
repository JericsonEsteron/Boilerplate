using MVC;
using UnityEngine;
using UnityEngine.UI;

namespace Playground.MVC
{
    public class HealthView : AView<HealthModel>
    {
        [SerializeField] Image _image;

        protected override void OnModelBound(HealthModel model)
        {
            if (_image == null)
                throw new MissingReferenceException($"{GetType().Name} requires an Image reference.");

            model.CurrentHealth.OnValueChanged += OnHealthChanged;
            model.MaxHealth.OnValueChanged += OnHealthChanged;
            Refresh();
        }

        protected override void OnModelUnBound(HealthModel model)
        {
            model.CurrentHealth.OnValueChanged -= OnHealthChanged;
            model.MaxHealth.OnValueChanged -= OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            Refresh();
        }

        private void Refresh()
        {
            if (Model.MaxHealth.Value <= 0f)
            {
                _image.fillAmount = 0f;
                return;
            }

            _image.fillAmount = Mathf.Clamp(Model.CurrentHealth.Value, 0f, Model.MaxHealth.Value) / Model.MaxHealth.Value;
        }

        private void OnValidate()
        {
            if (_image == null)
                _image = GetComponent<Image>();
        }
    }

}
