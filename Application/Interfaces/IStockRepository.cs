namespace Application.Interfaces;

public interface IStockRepository
{
    decimal GetStockPrice(string id);
    List<(string Id, decimal Price, DateTime UpdatedAt)> GetAllStocks();
    void UpdateStockPrice(string id, decimal newPrice);
}
