using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern
{
    [DisallowMultipleComponent]
    public class DisposalHandler : MonoBehaviour, IDisposable
    {
        private Observer observerInstance;

        private readonly Dictionary<Type, List<ISubject>> disposableDict = new();

        public void AddSubjectInDisposable<IEvent>(Subject<IEvent> observer, Observer observerInstance)
        {
            this.observerInstance = observerInstance;
            var type = typeof(IEvent);

            if (!disposableDict.TryGetValue(type, out var observers))
            {
                observers = new List<ISubject>();
                disposableDict[type] = observers;
            }

            observers.Add(observer);
        }

        private void OnDestroy()
        {
            if (observerInstance == null)
                return;

            foreach (var pair in disposableDict)
            {
                foreach (var observer in pair.Value)
                {
                    observerInstance.UnSubscribe(pair.Key, observer);
                }
            }

            disposableDict.Clear();
        }

        public void Dispose()
        {
            OnDestroy();
        }
    }
}