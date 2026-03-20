namespace Events
{
    public sealed class ButtonPressedEvent : IEvent
    {
        public ButtonPressedEvent(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

}
