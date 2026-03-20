using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern
{
    [DisallowMultipleComponent]
    public sealed class DisposalHandler : MonoBehaviour
    {
        private readonly List<EventSubscription> _subscriptions = new();

        internal void Track(EventSubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            if (_subscriptions.Count > 0 && (_subscriptions.Count & 31) == 0)
                PruneDisposedSubscriptions();

            _subscriptions.Add(subscription);
        }

        private void OnDestroy()
        {
            for (var i = _subscriptions.Count - 1; i >= 0; i--)
            {
                var subscription = _subscriptions[i];
                subscription.Dispose();
            }

            _subscriptions.Clear();
        }

        private void PruneDisposedSubscriptions()
        {
            var writeIndex = 0;
            for (var readIndex = 0; readIndex < _subscriptions.Count; readIndex++)
            {
                var subscription = _subscriptions[readIndex];
                if (subscription.IsDisposed)
                    continue;

                if (writeIndex != readIndex)
                    _subscriptions[writeIndex] = subscription;

                writeIndex++;
            }

            if (writeIndex < _subscriptions.Count)
                _subscriptions.RemoveRange(writeIndex, _subscriptions.Count - writeIndex);
        }
    }
}
