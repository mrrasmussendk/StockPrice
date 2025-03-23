using Domain.Interfaces.Observer;

namespace Domain.Services;

public class StockPriceNotifier : IStockPriceSubject
{
    private readonly Dictionary<string, List<IStockPriceObserver>> _observers = new();

    public void Subscribe(string id, IStockPriceObserver observer)
    {
        if (!_observers.ContainsKey(id))
            _observers[id] = [];

        _observers[id].Add(observer);
    }

    public void Unsubscribe(string id, IStockPriceObserver observer)
    {
        if (!_observers.TryGetValue(id, out List<IStockPriceObserver>? value)) return;
        value.Remove(observer);
        if (value.Count == 0)
            _observers.Remove(id);
    }

    public async Task NotifyObservers(string id, decimal price)
    {
        if (_observers.TryGetValue(id, out var stocks))
        {
            var tasks = stocks.Select(observer => observer.UpdateStockPrice(id, price));
            await Task.WhenAll(tasks);
        }
    }
}
