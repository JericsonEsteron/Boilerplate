using ObserverPattern;

namespace EventMessage
{
    public interface IMessenger : IObservable, IObserver
    {
         #if UNITY_EDITOR
        public void LogObserverStatus();
        #endif
    }

}
