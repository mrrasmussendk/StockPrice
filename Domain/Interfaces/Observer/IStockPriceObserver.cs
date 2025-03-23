namespace Domain.Interfaces.Observer;

public interface IStockPriceObserver
{
    Task UpdateStockPrice(string id, decimal price);
}