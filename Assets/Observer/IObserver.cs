using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ObserverPattern
{
    public interface IObserver
    {
        public void Subscribe<IEvent>(Action<IEvent> observer, GameObject gameObject);
    }

}
