using Application.Interfaces;

namespace Presentation.Middleware;

public class WebSocketConnectionMiddleware(RequestDelegate next, IWebSocketHandler webSocketHandler)
{
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/ws") && context.WebSockets.IsWebSocketRequest)
        {
            var id = context.Request.Query["id"];
            if (string.IsNullOrEmpty(id))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("id symbol required.");
                return;
            }

            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await webSocketHandler.HandleConnection(webSocket, id);
        }
        else
        {
            await next(context);
        }
    }
}