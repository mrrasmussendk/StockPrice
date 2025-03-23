using Domain.Interfaces.Observer;

namespace ApplicationTest.Fakes;
public class FakeStockPriceSubject : IStockPriceSubject
{
    public List<(string Id, decimal Price)> Notifications { get; } = [];

    public void Subscribe(string id, IStockPriceObserver observer)
    {
        throw new NotImplementedException();
    }

    public void Unsubscribe(string id, IStockPriceObserver observer)
    {
        throw new NotImplementedException();
    }

    public Task NotifyObservers(string id, decimal price)
    {
        Notifications.Add((id, price));
        return Task.CompletedTask;
    }
}