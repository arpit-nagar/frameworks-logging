namespace Tavisca.Frameworks.Logging
{
    public interface IEventEntry : ILogEntry
    {
        string Source { get; set; }

        string EventType { get; set; }

        object PayloadObject { get; set; }

        string Payload { get; }

        void SetPayloadString(string payload);
    }
}
