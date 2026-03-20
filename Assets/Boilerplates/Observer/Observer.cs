using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Events;
using EventMessage;

namespace ObserverPattern
{
    public class Observer : IMessenger
    {
        private readonly Dictionary<Type, IEventBucket> _buckets = new();
        private readonly int _mainThreadId = Thread.CurrentThread.ManagedThreadId;

        public void Publish<TEvent>(TEvent eventMessage) where TEvent : IEvent
        {
            EnsureMainThread();

            if (eventMessage == null)
                throw new ArgumentNullException(nameof(eventMessage));

            if (!_buckets.TryGetValue(typeof(TEvent), out var bucket))
                return;

            ((EventBucket<TEvent>)bucket).Publish(eventMessage);
        }

        public IDisposable Subscribe<TEvent>(Action<TEvent> observer) where TEvent : IEvent
        {
            return Subscribe(observer, null);
        }

        public IDisposable Subscribe<TEvent>(Action<TEvent> observer, GameObject owner) where TEvent : IEvent
        {
            EnsureMainThread();

            if (observer == null)
                throw new ArgumentNullException(nameof(observer), "Observer callback cannot be null");

            var bucket = GetOrCreateBucket<TEvent>();
            var subscription = (EventSubscription)bucket.Subscribe(observer);

            if (owner != null)
                RegisterOwnerSubscription(subscription, owner);

            return subscription;
        }

        internal void RemoveBucket(Type eventType)
        {
            _buckets.Remove(eventType);
        }

        internal void EnsureMainThread()
        {
            if (Thread.CurrentThread.ManagedThreadId != _mainThreadId)
                throw new InvalidOperationException("Observer messenger is main-thread only. Publish, Subscribe, and Dispose must run on Unity's main thread.");
        }

        private EventBucket<TEvent> GetOrCreateBucket<TEvent>() where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            if (_buckets.TryGetValue(eventType, out var bucket))
                return (EventBucket<TEvent>)bucket;

            var createdBucket = new EventBucket<TEvent>(this);
            _buckets.Add(eventType, createdBucket);
            return createdBucket;
        }

        private void RegisterOwnerSubscription(EventSubscription subscription, GameObject owner)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            if (!owner.TryGetComponent<DisposalHandler>(out var disposalHandler))
                disposalHandler = owner.AddComponent<DisposalHandler>();

            disposalHandler.Track(subscription);
        }

        #if UNITY_EDITOR
        public void LogObserverStatus()
        {
            if (_buckets.Count == 0)
            {
                Debug.Log("[Observer] No observers registered");
                return;
            }

            Debug.Log("[Observer] Event Type => Observer Count");
            foreach (var kvp in _buckets)
            {
                Debug.Log($"  {kvp.Key.Name}: {kvp.Value.ActiveSubscriptionCount}");
            }
        }
        #endif
    }
}
