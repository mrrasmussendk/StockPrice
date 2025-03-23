using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Presentation.Middleware;
using Presentation.Test.WebSocket.Fakes;

namespace Presentation.Test.WebSocket;

public class WebSocketTests
{
    [Fact]
    public async Task Middleware_Handles_Valid_WebSocket_Request()
    {
        // Arrange
        var handler = new FakeWebSocketHandler();
        var middleware = new WebSocketConnectionMiddleware(_ => Task.CompletedTask, handler);

        var context = new DefaultHttpContext
        {
            Request =
            {
                Path = "/ws",
                QueryString = new QueryString("?id=test123")
            }
        };
        context.Features.Set<IHttpWebSocketFeature>(new FakeWebSocketFeature());

        // Act
        await middleware.Invoke(context);

        // Assert
        Assert.True(handler.WasCalled);
        Assert.Equal("test123", handler.HandledId);
    }

    [Fact]
    public async Task Middleware_Returns_400_When_Id_Is_Missing()
    {
        // Arrange
        var handler = new FakeWebSocketHandler();
        var middleware = new WebSocketConnectionMiddleware(_ => Task.CompletedTask, handler);

        var context = new DefaultHttpContext();
        context.Request.Path = "/ws";
        context.Features.Set<IHttpWebSocketFeature>(new FakeWebSocketFeature());

        var responseStream = new MemoryStream();
        context.Response.Body = responseStream;

        // Act
        await middleware.Invoke(context);
        responseStream.Position = 0;
        var responseText = await new StreamReader(responseStream).ReadToEndAsync();

        // Assert
        Assert.Equal(400, context.Response.StatusCode);
        Assert.Contains("id symbol required", responseText);
    }
    
    [Theory]
    [InlineData("/ws", "test123", true)]
    [InlineData("/ws", "", false)]
    [InlineData("/api/stocks", null, false)]
    public async Task Invoke_WithVariousInputs_HandlesCorrectly(string path, string? id, bool shouldCallHandler)
    {
        // arrange
        var handler = new FakeWebSocketHandler();
        var middleware = new WebSocketConnectionMiddleware(_ => Task.CompletedTask, handler);
        var context = new DefaultHttpContext();
        context.Request.Path = path;
        context.Request.QueryString = string.IsNullOrEmpty(id) ? QueryString.Empty : new QueryString($"?id={id}");
        context.Features.Set<IHttpWebSocketFeature>(new FakeWebSocketFeature());

        // act
        await middleware.Invoke(context);

        // assert
        Assert.Equal(shouldCallHandler, handler.WasCalled);
    }
}