namespace Domain.Entities;

public class Stock(string id, decimal initialPrice)
{
    public string Id { get; } = id;
    public decimal Price { get; private set; } = initialPrice;

    public void UpdatePrice(decimal newPrice)
    {
        Price = newPrice;
    }
}