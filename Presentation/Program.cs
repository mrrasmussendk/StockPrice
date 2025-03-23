using Application.Interfaces;
using Application.UseCases;
using BackgroundServices;
using Domain.Interfaces.Observer;
using Domain.Services;
using Infrastructure.Persistence;
using Infrastructure.WebSocket;
using Microsoft.AspNetCore.WebSockets;
using Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IStockRepository, InMemoryStockRepository>();
builder.Services.AddSingleton<IStockPriceSubject, StockPriceNotifier>();
builder.Services.AddScoped<IStockUseCase, StockUseCase>();

builder.Services.AddSingleton<IWebSocketHandler, StockWebSocketHandler>();

builder.Services.AddHostedService<StockPriceGenerator>();

builder.Services.AddControllers();

builder.Services.AddWebSockets(options =>
{
    options.KeepAliveInterval = TimeSpan.FromMinutes(2);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWebSockets();
app.UseMiddleware<WebSocketConnectionMiddleware>();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
