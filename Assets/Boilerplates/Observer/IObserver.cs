using System;
using UnityEngine;
using Events;

namespace ObserverPattern
{
    public interface IObserver
    {
        public IDisposable Subscribe<TEvent>(Action<TEvent> observer) where TEvent : IEvent;
        public IDisposable Subscribe<TEvent>(Action<TEvent> observer, GameObject owner) where TEvent : IEvent;
    }

}
