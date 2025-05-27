using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;

namespace Playground.MVC
{
    public class HealthController : AController<HealthView, HealthModel>, IDamageable
    {
        [SerializeField] float _maxHealth = 100f;

        private HealthModel _healthModel;

        public void TakeDamage(float damageValue)
        {
            _healthModel.CurrentHealth.Value = Mathf.Clamp(_healthModel.CurrentHealth.Value - damageValue, 0, _healthModel.CurrentHealth.Value);
        }

        protected override void OnModelBound(HealthModel model)
        {
            _healthModel = model;
            InitializeValues();
        }

        protected override void OnModelUnBound(HealthModel model)
        {
            _healthModel = default;
        }

        private void InitializeValues()
        {
            _healthModel.SetInitialValue(_maxHealth);
        }


    }
}
