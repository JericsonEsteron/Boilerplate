using MVC;
using UnityEngine;

namespace Playground.MVC
{
    [RequireComponent(typeof(HealthView))]
    public class HealthController : AController<HealthView, HealthModel>, IDamageable
    {
        [SerializeField] float _maxHealth = 100f;

        public void TakeDamage(float damageValue)
        {
            Model.ApplyDamage(damageValue);
        }

        protected override void InitializeModel(HealthModel model)
        {
            model.Initialize(_maxHealth);
        }

        protected override void OnModelBound(HealthModel model)
        {
        }

        protected override void OnModelUnBound(HealthModel model)
        {
        }
    }
}
