using Application.Interfaces;

namespace ApplicationTest.Fakes;

public class StockRepositoryFake : IStockRepository
{
    private readonly Dictionary<string, (decimal Price, DateTime UpdatedAt)> _stocks = new();

    public StockRepositoryFake()
    {
        var stockSymbols = new[] {"AAPL", "GOOG", "TSLA", "MSFT", "AMZN"};
        foreach (var symbol in stockSymbols)
        {
            _stocks[symbol] = (100 + (decimal) new Random().NextDouble() * 50, DateTime.UtcNow);
        }
    }

    public decimal GetStockPrice(string Id)
    {
        if (!_stocks.TryGetValue(Id, out var stockData))
            throw new KeyNotFoundException($"Stock {Id} not found.");

        return stockData.Price;
    }

    public List<(string Id, decimal Price, DateTime UpdatedAt)> GetAllStocks()
    {
        return _stocks.Select(s => (s.Key, s.Value.Price, s.Value.UpdatedAt)).ToList();
    }

    public void UpdateStockPrice(string Id, decimal newPrice)
    {
        if (!_stocks.ContainsKey(Id))
            throw new KeyNotFoundException($"Stock {Id} not found.");

        _stocks[Id] = (newPrice, DateTime.UtcNow);
    }
}