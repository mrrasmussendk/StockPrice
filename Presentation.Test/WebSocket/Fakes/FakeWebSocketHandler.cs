using Application.Interfaces;

namespace Presentation.Test.WebSocket.Fakes;

public class FakeWebSocketHandler : IWebSocketHandler
{
    public string? HandledId { get; private set; }
    public bool WasCalled => HandledId != null;

    public Task HandleConnection(System.Net.WebSockets.WebSocket webSocket, string id)
    {
        HandledId = id;
        return Task.CompletedTask;
    }
}