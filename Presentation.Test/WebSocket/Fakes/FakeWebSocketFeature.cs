using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Presentation.Test.WebSocket.Fakes;

public class FakeWebSocketFeature : IHttpWebSocketFeature
{
    public bool IsWebSocketRequest => true;

    public Task<System.Net.WebSockets.WebSocket> AcceptAsync(WebSocketAcceptContext context = null)
    {
        return Task.FromResult<System.Net.WebSockets.WebSocket>(new FakeWebSocket());
    }
}

public class FakeWebSocket : System.Net.WebSockets.WebSocket
{
    public override WebSocketCloseStatus? CloseStatus => null;
    public override string CloseStatusDescription => null;
    public override WebSocketState State => WebSocketState.Open;
    public override string SubProtocol => null;

    public override void Abort() { }
    public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken) => Task.CompletedTask;
    public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken) => Task.CompletedTask;
    public override void Dispose() { }
    public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken) =>
        Task.FromResult(new WebSocketReceiveResult(0, WebSocketMessageType.Text, true));
    public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken) => Task.CompletedTask;
}
