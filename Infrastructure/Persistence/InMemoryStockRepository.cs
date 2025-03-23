using Application.Interfaces;

namespace Infrastructure.Persistence;

public class InMemoryStockRepository : IStockRepository
{
    private readonly Dictionary<string, (decimal Price, DateTime UpdatedAt)> _stocks = new();

    public InMemoryStockRepository()
    {
        var stockSymbols = new[] { "AAPL", "GOOG", "TSLA", "MSFT", "AMZN" };
        foreach (var symbol in stockSymbols)
        {
            _stocks[symbol] = (100 + (decimal)new Random().NextDouble() * 50, DateTime.UtcNow);
        }
    }

    public decimal GetStockPrice(string id)
    {
        if (!_stocks.TryGetValue(id, out var stockData))
            throw new KeyNotFoundException($"Stock {id} not found.");
        
        return stockData.Price;
    }

    public List<(string Id, decimal Price, DateTime UpdatedAt)> GetAllStocks()
    {
        return _stocks.Select(s => (s.Key, s.Value.Price, s.Value.UpdatedAt)).ToList();
    }

    
    public void UpdateStockPrice(string id, decimal newPrice)
    {
        if (!_stocks.ContainsKey(id))
            throw new KeyNotFoundException($"Stock {id} not found.");
        
        _stocks[id] = (newPrice, DateTime.UtcNow);
    }
}