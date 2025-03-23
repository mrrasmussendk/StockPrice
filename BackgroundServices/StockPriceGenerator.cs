using Application.Interfaces;

namespace BackgroundServices;
public class StockPriceGenerator : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Random _random = new();
    private readonly string[] _stocks = ["AAPL", "GOOG", "TSLA", "MSFT", "AMZN"];

    public StockPriceGenerator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var stockUseCase = scope.ServiceProvider.GetRequiredService<IStockUseCase>();

                foreach (var ticker in _stocks)
                {
                    var priceChange = (decimal)(_random.NextDouble() * 5 - 2.5);
                    var newPrice = Math.Max(100 + priceChange, 1);
                    await stockUseCase.UpdateStockPrice(ticker, newPrice);
                }
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}