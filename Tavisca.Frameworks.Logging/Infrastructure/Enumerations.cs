namespace Tavisca.Frameworks.Logging.Infrastructure
{
    /// <summary>
    /// Defines the severity level of events.
    /// </summary>
    public enum SeverityOptions : int
    {
        Undefined = 0,
        Critical = 1,
        Error = 2,
        Warning = 4,
        Information = 8,
        Verbose = 16
    }

    /// <summary>
    /// Defines the status of an operation.
    /// </summary>
    public enum StatusOptions : int
    {
        Success = 0,
        Failure = 1
    }

    public enum PriorityOptions : int
    {
        Undefined = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }

    public enum CompressionTypeOptions : int
    {
        Zip = 0,
        Deflate = 1,
        Custom = 2
    }
}
