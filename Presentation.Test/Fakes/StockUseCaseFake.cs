using Application.DTO;
using Application.Interfaces;

namespace Presentation.Test;

public class StockUseCaseFake : IStockUseCase
{
    
    private readonly Dictionary<string, (decimal Price, DateTime UpdatedAt)> _stocks = new();

    public StockUseCaseFake()
    {
        var stockSymbols = new[] { "AAPL", "GOOG", "TSLA", "MSFT", "AMZN" };
        foreach (var symbol in stockSymbols)
        {
            _stocks[symbol] = (100 + (decimal)new Random().NextDouble() * 50, DateTime.UtcNow);
        }
    }
    public Task<StockDto> GetStockPrice(string id)
    {    if (!_stocks.TryGetValue(id, out _))
            throw new KeyNotFoundException($"Stock {id} not found.");
        
        return Task.FromResult(new StockDto(id, 1));
    }

    public Task UpdateStockPrice(string id, decimal price)
    {
        throw new NotImplementedException();

    }

    public Task<List<StockDto>> GetAllStocks()
    {
        return Task.FromResult(new List<StockDto>());
    }
}