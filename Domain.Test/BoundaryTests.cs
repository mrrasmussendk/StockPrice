namespace Domain.Test;

public class BoundaryTests
{
    [Fact]
    public void Domain_Should_Not_Reference_Any_Outer_Layers()
    {
        var domainAssembly = typeof(Domain.Entities.Stock).Assembly;
        var references = domainAssembly.GetReferencedAssemblies();

        Assert.All(references, reference =>
        {
            Assert.DoesNotContain(reference.Name, new[] {
                "Application", "Infrastructure", "Presentation"
            });
        });
    }
}