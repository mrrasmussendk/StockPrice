using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Application.Interfaces;
using Domain.Interfaces.Observer;

namespace Infrastructure.WebSocket;

public class StockWebSocketHandler(IStockPriceSubject stockPriceNotifier) : IWebSocketHandler, IStockPriceObserver
{
    private readonly Dictionary<string, System.Net.WebSockets.WebSocket> _clients = new();

    public async Task HandleConnection(System.Net.WebSockets.WebSocket socket, string id)
    {
        if (_clients.TryAdd(id, socket))
        {
            stockPriceNotifier.Subscribe(id, this);
        }

        var buffer = new byte[1024 * 4];
        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType != WebSocketMessageType.Close) continue;
            _clients.Remove(id);
            stockPriceNotifier.Unsubscribe(id, this);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
        }
    }

    public async Task UpdateStockPrice(string id, decimal price)
    {
        if (_clients.TryGetValue(id, out var socket) && socket.State == WebSocketState.Open)
        {
            var message = JsonSerializer.Serialize(new {Id = id, price, timestamp = DateTime.UtcNow });
            await socket.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}