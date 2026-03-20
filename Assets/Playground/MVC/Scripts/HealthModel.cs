using MVC;
using UnityEngine;

namespace Playground.MVC
{
    public class HealthModel : IModel
    {
        public Property<float> CurrentHealth { get; } = new();
        public Property<float> MaxHealth { get; } = new();

        public void Initialize(float maxHealth)
        {
            var validatedMaxHealth = Mathf.Max(1f, maxHealth);

            MaxHealth.Value = validatedMaxHealth;
            CurrentHealth.Value = validatedMaxHealth;
        }

        public void ApplyDamage(float damageValue)
        {
            if (damageValue <= 0f)
                return;

            CurrentHealth.Value = Mathf.Clamp(CurrentHealth.Value - damageValue, 0f, MaxHealth.Value);
        }
    }
}
