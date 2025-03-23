using Application.DTO;
using Application.Interfaces;
using Application.UseCases;
using ApplicationTest.Fakes;

namespace ApplicationTest.UseCases;

public class StockUseCaseTests
{
    private readonly FakeStockPriceSubject _fakeStockPriceSubject;
    private readonly StockUseCase _stockUseCase;

    public StockUseCaseTests()
    {
        _fakeStockPriceSubject = new FakeStockPriceSubject();
        IStockRepository stockRepositoryFake = new StockRepositoryFake();
        _stockUseCase = new StockUseCase(_fakeStockPriceSubject, stockRepositoryFake);
    }

    [Fact]
    public async Task GetStockPrice_ReturnsCorrectStockDTO()
    {
        const string Id = "AAPL";

        StockDto stock = await _stockUseCase.GetStockPrice(Id);

        Assert.NotNull(stock);
        Assert.Equal(Id, stock.Id);
        Assert.True(stock.Price is >= 100 and <= 150);
    }

    [Fact]
    public async Task GetStockPrice_ThrowsException_ForInvalidTicker()
    {
        string invalidTicker = "INVALID";
        var exception =
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _stockUseCase.GetStockPrice(invalidTicker));
        Assert.Equal($"Stock {invalidTicker} not found.", exception.Message);
    }

    [Fact]
    public async Task UpdateStockPrice_ChangesStockPriceAndNotifiesObservers()
    {
        string Id = "GOOG";
        decimal newPrice = 200m;

        await _stockUseCase.UpdateStockPrice(Id, newPrice);
        StockDto updatedStock = await _stockUseCase.GetStockPrice(Id);


        Assert.Equal(newPrice, updatedStock.Price);
        Assert.Single(_fakeStockPriceSubject.Notifications);
        Assert.Equal(Id, _fakeStockPriceSubject.Notifications[0].Id);
        Assert.Equal(newPrice, _fakeStockPriceSubject.Notifications[0].Price);
    }

    [Fact]
    public async Task GetAllStocks_ReturnsArray()
    {
        var stocks = await _stockUseCase.GetAllStocks();
        Assert.NotEmpty(stocks);
    }
}