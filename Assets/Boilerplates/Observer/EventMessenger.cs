using ObserverPattern;

namespace EventMessage
{
    public class EventMessenger
    {
        public static readonly IMessenger Default = new Observer();
    }

}
