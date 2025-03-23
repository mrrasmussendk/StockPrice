using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces.Observer;

namespace Application.UseCases;

public class StockUseCase(IStockPriceSubject stockPriceNotifier, IStockRepository stockRepository)
    : IStockUseCase
{
    public Task<StockDto> GetStockPrice(string id)
    {
        var price = stockRepository.GetStockPrice(id);
        return Task.FromResult(new StockDto(id, price));
    }

    public Task<List<StockDto>> GetAllStocks()
    {
        var stocks = stockRepository.GetAllStocks();
        return Task.FromResult(stocks.Select(s => new StockDto(s.Id, s.Price)).ToList());
    }

    public async Task UpdateStockPrice(string id, decimal price)
    {
        var stock = new Stock(id, stockRepository.GetStockPrice(id));
        stock.UpdatePrice(price);
        stockRepository.UpdateStockPrice(stock.Id, stock.Price);
        await stockPriceNotifier.NotifyObservers(id, stock.Price);
    }
}