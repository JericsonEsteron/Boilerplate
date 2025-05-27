using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;
using UnityEngine.UI;

namespace Playground.MVC
{
    public class HealthView : AView<HealthModel>
    {
        [SerializeField] Image _image;

        private HealthModel _healthModel;

        protected override void OnModelBound(HealthModel model)
        {
            _healthModel = model;
            _healthModel.CurrentHealth.OnValueChanged += OnHealthChanged;
        }

        protected override void OnModelUnBound(HealthModel model)
        {
            _healthModel.CurrentHealth.OnValueChanged -= OnHealthChanged;
            _healthModel = default;
        }

        private void OnHealthChanged()
        {
            _image.fillAmount = Mathf.Clamp(_healthModel.CurrentHealth.Value, 0, _healthModel.MaxHealth.Value) / _healthModel.MaxHealth.Value;
        }
    }

}
