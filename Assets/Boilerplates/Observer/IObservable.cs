using Events;

namespace ObserverPattern
{
    public interface IObservable
    {
        public void Publish<TEvent>(TEvent eventMessage) where TEvent : IEvent;
    }

}
