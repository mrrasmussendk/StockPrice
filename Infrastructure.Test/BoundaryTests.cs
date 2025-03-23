namespace Infrastructure.Test;

public class BoundaryTests
{
    [Fact]
    public void Infrastructure_Should_Not_Reference_Presentation()
    {
        var infraAssembly = typeof(Persistence.InMemoryStockRepository).Assembly;
        var references = infraAssembly.GetReferencedAssemblies();

        Assert.DoesNotContain(references, r => r.Name == "Presentation");
    }
}