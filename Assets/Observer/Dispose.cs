using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObserverPattern
{
    [DisallowMultipleComponent]
    public class Dispose : MonoBehaviour, IDisposable
    {
        private Observer observerClass;
        private Dictionary<Type, List<object>> disposableDict = new Dictionary<Type, List<object>>();
        
        public void AddSubjectInDisposable<IEvent>(Subject<IEvent> observer, Observer observerClass)
        {
            List<object> observers;
            this.observerClass = observerClass;
            if(disposableDict.TryGetValue(typeof(IEvent), out observers))
            {
                observers.Add(observer);
                disposableDict[typeof(IEvent)] = observers;
            }
            else
            {
                observers = new List<object>
                {
                    observer
                };
                disposableDict.Add(typeof(IEvent), observers);
            }
        }

        private void OnDestroy() 
        {
            foreach (Type key in disposableDict.Keys)
            {
                if(observerClass.observerDict.TryGetValue(key, out List<object> observers))
                {
                    observers.RemoveAll(observer => disposableDict[key].Contains(observer));
                }
            }
        }
    }

}
