using Infrastructure.Persistence;

namespace Infrastructure.Test;

public class InMemoryStockRepositoryTests
{
    private readonly InMemoryStockRepository _repository;
    private readonly Random _random = new();

    public InMemoryStockRepositoryTests()
    {
        _repository = new InMemoryStockRepository();
    }

    [Theory]
    [InlineData("AAPL")]
    [InlineData("GOOG")]
    [InlineData("TSLA")]
    [InlineData("MSFT")]
    [InlineData("AMZN")]
    public void GetStockPrice_ShouldReturnValidPrice(string Id)
    {
        decimal price = _repository.GetStockPrice(Id);
        Assert.InRange(price, 100, 150);
    }

    [Fact]
    public void GetStockPrice_ShouldThrow_WhenTickerNotFound()
    {
        Assert.Throws<KeyNotFoundException>(() => _repository.GetStockPrice("INVALID"));
    }

    [Fact]
    public void GetAllStocks_ShouldReturnAllStocks()
    {
        var stocks = _repository.GetAllStocks();
        Assert.NotEmpty(stocks);
        Assert.All(stocks, stock => Assert.InRange(stock.Price, 100, 150));
    }

    [Fact]
    public void UpdateStockPrice_ShouldModifyExistingStock()
    {
        string Id = "TSLA";
        decimal newPrice = _random.Next(200, 500);
        _repository.UpdateStockPrice(Id, newPrice);
        Assert.Equal(newPrice, _repository.GetStockPrice(Id));
    }

    [Fact]
    public void UpdateStockPrice_ShouldThrow_WhenTickerNotFound()
    {
        Assert.Throws<KeyNotFoundException>(() => _repository.UpdateStockPrice("INVALID", 300));
    }
}