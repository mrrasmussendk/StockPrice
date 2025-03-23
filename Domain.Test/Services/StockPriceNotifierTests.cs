using Domain.Interfaces.Observer;
using Domain.Services;

namespace Domain.Test.Services;

public class StockPriceNotifierTests
{
    private class TestStockPriceObserver : IStockPriceObserver
    {
        public List<(string id, decimal price)> ReceivedUpdates { get; } = new();

        public Task UpdateStockPrice(string id, decimal price)
        {
            ReceivedUpdates.Add((id, price));
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task Subscribe_ShouldReceiveNotifications()
    {
        var notifier = new StockPriceNotifier();
        var observer = new TestStockPriceObserver();

        notifier.Subscribe("AAPL", observer);
        await notifier.NotifyObservers("AAPL", 150m);

        Assert.Single(observer.ReceivedUpdates);
        Assert.Contains(("AAPL", 150m), observer.ReceivedUpdates);
    }

    [Fact]
    public async Task Unsubscribe_ShouldStopReceivingNotifications()
    {
        var notifier = new StockPriceNotifier();
        var observer = new TestStockPriceObserver();

        notifier.Subscribe("AAPL", observer);
        notifier.Unsubscribe("AAPL", observer);
        await notifier.NotifyObservers("AAPL", 200m);

        Assert.Empty(observer.ReceivedUpdates);
    }

    [Fact]
    public async Task NotifyObservers_ShouldNotFail_WhenNoObservers()
    {
        var notifier = new StockPriceNotifier();

        await notifier.NotifyObservers("AAPL", 300m);

        Assert.True(true); 
    }

    [Fact]
    public async Task MultipleObservers_ShouldReceiveNotifications()
    {
        var notifier = new StockPriceNotifier();
        var observer1 = new TestStockPriceObserver();
        var observer2 = new TestStockPriceObserver();

        notifier.Subscribe("AAPL", observer1);
        notifier.Subscribe("AAPL", observer2);
        await notifier.NotifyObservers("AAPL", 175m);

        Assert.Single(observer1.ReceivedUpdates);
        Assert.Single(observer2.ReceivedUpdates);
        Assert.Contains(("AAPL", 175m), observer1.ReceivedUpdates);
        Assert.Contains(("AAPL", 175m), observer2.ReceivedUpdates);
    }
}
