using System;
using System.Collections.Generic;
using Events;
using UnityEngine;

namespace ObserverPattern
{
    internal interface IEventBucket
    {
        Type EventType { get; }
        int ActiveSubscriptionCount { get; }
        void Remove(EventSubscription subscription);
    }

    internal sealed class EventBucket<TEvent> : IEventBucket where TEvent : IEvent
    {
        private readonly Observer _owner;
        private readonly List<EventSubscription<TEvent>> _subscriptions = new();
        private readonly Dictionary<Action<TEvent>, EventSubscription<TEvent>> _subscriptionsByCallback = new();
        private int _publishDepth;
        private int _activeSubscriptionCount;
        private bool _needsCompaction;

        internal EventBucket(Observer owner)
        {
            _owner = owner;
        }

        public Type EventType => typeof(TEvent);
        public int ActiveSubscriptionCount => _activeSubscriptionCount;

        internal IDisposable Subscribe(Action<TEvent> callback)
        {
            if (_subscriptionsByCallback.ContainsKey(callback))
                throw new InvalidOperationException($"Duplicate subscription detected for event type {EventType.Name}.");

            var subscription = new EventSubscription<TEvent>(this, callback, _subscriptions.Count);
            _subscriptions.Add(subscription);
            _subscriptionsByCallback.Add(callback, subscription);
            _activeSubscriptionCount++;
            return subscription;
        }

        internal void Publish(TEvent eventMessage)
        {
            _publishDepth++;

            var publishCount = _subscriptions.Count;
            for (var i = 0; i < publishCount; i++)
            {
                var subscription = _subscriptions[i];
                if (subscription.IsDisposed)
                    continue;

                subscription?.Callback(eventMessage);
            }

            _publishDepth--;
            if (_publishDepth == 0 && _needsCompaction)
                CompactDisposedSubscriptions();
        }

        public void Remove(EventSubscription subscription)
        {
            _owner.EnsureMainThread();

            if (subscription is not EventSubscription<TEvent> typedSubscription)
                throw new ArgumentException($"Invalid subscription type for event bucket {EventType.Name}.", nameof(subscription));

            if (typedSubscription.IsDisposed)
                return;

            _subscriptionsByCallback.Remove(typedSubscription.Callback);
            _activeSubscriptionCount--;
            typedSubscription.MarkDisposed();

            if (_publishDepth > 0)
            {
                _needsCompaction = true;
                return;
            }

            RemoveAtSwapBack(typedSubscription.Index);
            ReleaseBucketIfEmpty();
        }

        private void CompactDisposedSubscriptions()
        {
            var writeIndex = 0;
            for (var readIndex = 0; readIndex < _subscriptions.Count; readIndex++)
            {
                var subscription = _subscriptions[readIndex];
                if (subscription.IsDisposed)
                    continue;

                if (writeIndex != readIndex)
                {
                    _subscriptions[writeIndex] = subscription;
                    subscription.Index = writeIndex;
                }

                writeIndex++;
            }

            if (writeIndex < _subscriptions.Count)
                _subscriptions.RemoveRange(writeIndex, _subscriptions.Count - writeIndex);

            _needsCompaction = false;
            ReleaseBucketIfEmpty();
        }

        private void RemoveAtSwapBack(int index)
        {
            var lastIndex = _subscriptions.Count - 1;
            if (index < 0 || index > lastIndex)
                return;

            if (index != lastIndex)
            {
                var swappedSubscription = _subscriptions[lastIndex];
                _subscriptions[index] = swappedSubscription;
                swappedSubscription.Index = index;
            }

            _subscriptions.RemoveAt(lastIndex);
        }

        private void ReleaseBucketIfEmpty()
        {
            if (_activeSubscriptionCount == 0 && _publishDepth == 0)
                _owner.RemoveBucket(EventType);
        }
    }
}
