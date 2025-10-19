namespace VideoStream.Application.Events;

public partial interface IStopProcessingEvent
{
    bool StopProcessing { get; set; }
}