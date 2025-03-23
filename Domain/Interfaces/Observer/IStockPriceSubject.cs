namespace Domain.Interfaces.Observer;

public interface IStockPriceSubject
{
    void Subscribe(string id, IStockPriceObserver observer);
    void Unsubscribe(string id, IStockPriceObserver observer);
    Task NotifyObservers(string id, decimal price);
}