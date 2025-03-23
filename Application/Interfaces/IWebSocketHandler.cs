using System.Net.WebSockets;

namespace Application.Interfaces;

public interface IWebSocketHandler
{
    Task HandleConnection(WebSocket socket, string id);

}