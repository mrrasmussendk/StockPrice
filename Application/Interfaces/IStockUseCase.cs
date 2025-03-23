using Application.DTO;

namespace Application.Interfaces;

public interface IStockUseCase
{
    Task<StockDto> GetStockPrice(string id);
    Task UpdateStockPrice(string id, decimal price);
    Task<List<StockDto>> GetAllStocks();
}