using Application.Interfaces;
using Application.UseCases;
using BackgroundServices;
using Domain.Interfaces.Observer;
using Domain.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IStockPriceSubject, StockPriceNotifier>();
builder.Services.AddSingleton<IStockUseCase, StockUseCase>();
builder.Services.AddHostedService<StockPriceGenerator>();

var host = builder.Build();
host.Run();