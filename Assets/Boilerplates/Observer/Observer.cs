using System;
using System.Collections.Generic;
using UnityEngine;
using EventMessage;

namespace ObserverPattern
{
    public class Observer : IMessenger, IDisposeCollector
    {
        private readonly Dictionary<Type, List<ISubject>> observerDict = new();
        private readonly Dictionary<Type, List<ISubject>> observersToRemove = new();
        private readonly object _lock = new();

        private int publishDepth = 0;

        public void Publish<IEvent>(IEvent eventMessage)
        {
            List<ISubject> snapshot;

            lock (_lock)
            {
                if (!observerDict.TryGetValue(typeof(IEvent), out var observers))
                    return;

                publishDepth++;
                snapshot = new List<ISubject>(observers);
            }

            foreach (var subject in snapshot)
            {
                subject.Invoke(eventMessage);
            }

            lock (_lock)
            {
                publishDepth--;

                if (publishDepth == 0)
                    RemoveObservers();
            }
        }

        public void Subscribe<IEvent>(Action<IEvent> observer, GameObject gameObject)
        {
            var newObserver = new Subject<IEvent>(observer);
            var type = typeof(IEvent);

            lock (_lock)
            {
                if (!observerDict.TryGetValue(type, out var observers))
                {
                    observers = new List<ISubject>();
                    observerDict.Add(type, observers);
                }

                observers.Add(newObserver);
            }

            AddDisposable(newObserver, gameObject);
        }

        public void UnSubscribe(Type type, ISubject observer)
        {
            lock (_lock)
            {
                if (publishDepth == 0)
                {
                    if (observerDict.TryGetValue(type, out var observers))
                    {
                        observers.Remove(observer);
                    }
                }
                else
                {
                    if (!observersToRemove.TryGetValue(type, out var toRemove))
                    {
                        toRemove = new List<ISubject>();
                        observersToRemove[type] = toRemove;
                    }

                    toRemove.Add(observer);
                }
            }
        }

        private void RemoveObservers()
        {
            foreach (var pair in observersToRemove)
            {
                if (observerDict.TryGetValue(pair.Key, out var observers))
                {
                    foreach (var observer in pair.Value)
                    {
                        observers.Remove(observer);
                    }
                }
            }

            observersToRemove.Clear();
        }

        public void AddDisposable<IEvent>(Subject<IEvent> observer, GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<DisposalHandler>(out var disposable))
            {
                disposable = gameObject.AddComponent<DisposalHandler>();
            }

            disposable.AddSubjectInDisposable(observer, this);
        }

        public void GetDictionary()
        {
            lock (_lock)
            {
                foreach (var key in observerDict.Keys)
                {
                    Debug.Log($"{key} : {observerDict[key].Count}");
                }
            }
        }
    }

    public interface ISubject
    {
        void Invoke(object eventMessage);
    }

    public class Subject<T> : ISubject
    {
        private readonly Action<T> _onInvoke;

        public Subject(Action<T> onInvoke)
        {
            _onInvoke = onInvoke;
        }

        public void Invoke(object eventMessage)
        {
            if (eventMessage is T casted)
                _onInvoke?.Invoke(casted);
        }
    }
}