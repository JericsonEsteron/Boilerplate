using System;
using Events;

namespace ObserverPattern
{
    internal abstract class EventSubscription : IDisposable
    {
        private readonly IEventBucket _bucket;
        private bool _isDisposed;

        protected EventSubscription(IEventBucket bucket)
        {
            _bucket = bucket;
        }

        internal bool IsDisposed => _isDisposed;

        internal void MarkDisposed()
        {
            _isDisposed = true;
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _bucket.Remove(this);
        }
    }

    internal sealed class EventSubscription<TEvent> : EventSubscription where TEvent : IEvent
    {
        internal EventSubscription(EventBucket<TEvent> bucket, Action<TEvent> callback, int index) : base(bucket)
        {
            Callback = callback;
            Index = index;
        }

        internal Action<TEvent> Callback { get; }
        internal int Index { get; set; }
    }
}
