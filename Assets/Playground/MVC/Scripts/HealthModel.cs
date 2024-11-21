using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;

public class HealthModel : IModel
{
    public Property<float> CurrentHealth = new Property<float>();
    public Property<float> MaxHealth = new Property<float>();

    public void SetInitialValue(float value)
    {
        CurrentHealth.Value = MaxHealth.Value = value;
    }

}
