using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using EventMessage;
using UnityEngine;

namespace ObserverPattern
{
    public class Observer : IMessenger, IDisposeCollector
    {

        internal Dictionary<Type, List<object>> observerDict = new Dictionary<Type, List<object>>();

        public void Publish<IEvent>(IEvent eventMessage)
        {
            List<object> observers;

            if(observerDict.TryGetValue(typeof(IEvent), out observers))
            {
                foreach(Subject<IEvent> subject in observers)
                {
                    subject.InvokeAction(eventMessage);
                }
            }
        }

        public void Subscribe<IEvent>(Action<IEvent> observer, GameObject gameObject)
        {
            List<object> observers;
            Subject<IEvent> newObserver = new Subject<IEvent>(observer);

            if(observerDict.TryGetValue(typeof(IEvent), out observers))
            {
                observers.Add(newObserver);
                observerDict[typeof(IEvent)] = observers;
            }
            else
            {
                observers = new List<object>
                {
                    newObserver
                };
                observerDict.Add(typeof(IEvent), observers);
            }
            AddDisposable(newObserver, gameObject);
        }

        public void AddDisposable<IEvent>(Subject<IEvent>  observer, GameObject gameObject)
        {
            IDisposable disposable;
            if(gameObject.TryGetComponent<IDisposable>(out disposable))
            {
                disposable.AddSubjectInDisposable(observer, this);
            }
            else
            {
                disposable = gameObject.AddComponent<DisposalHandler>();
                disposable.AddSubjectInDisposable(observer, this);
            }
        }

        public void GetDictionary() // for testing purposes only
        {
            List<object> observers;
            foreach (Type key in observerDict.Keys)
            {
                if(observerDict.TryGetValue(key, out observers))
                {
                    UnityEngine.Debug.Log(key + " : " + observers.Count);
                }
            }
        }
    }

    public class Subject<T>
    {
        Action<T> OnInvoke;

        public Subject(Action<T> OnInvoke)
        {
            this.OnInvoke = OnInvoke;
        }

        public void InvokeAction(T eventMessage)
        {
            OnInvoke?.Invoke(eventMessage);
        }
    }

}
