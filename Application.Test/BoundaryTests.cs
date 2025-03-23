namespace ApplicationTest;

public class BoundaryTests
{
    [Fact]
    public void Application_Should_Not_Refer_Infrastructure_And_Presentation()
    {
        var appAssembly = typeof(Application.UseCases.StockUseCase).Assembly;
        var references = appAssembly.GetReferencedAssemblies();

        var forbidden = new[] { "Infrastructure", "Presentation" };

        foreach (var reference in references)
        {
            if (forbidden.Contains(reference.Name))
            {
                Assert.Fail($"Application layer should not reference '{reference.Name}'");
            }
        }
    }

}