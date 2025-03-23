using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;

namespace Presentation.Test;

public class ApiControllerTests
{
    [Fact]
    public async Task Stock_Not_Found()
    {
        string id = "RandomNotFoundString";
        IStockUseCase stockUseCase = new StockUseCaseFake();
        var controller = new StockController(stockUseCase);

       ActionResult result = (ActionResult) await controller.GetStockByTicker(id);
       Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Stock_Was_Found()
    {
        string id = "GOOG";
        IStockUseCase stockUseCase = new StockUseCaseFake();
        var controller = new StockController(stockUseCase);

        ActionResult result = (ActionResult) await controller.GetStockByTicker(id);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task All_Stock_Found()
    {
        IStockUseCase stockUseCase = new StockUseCaseFake();
        var controller = new StockController(stockUseCase);

        ActionResult result = (ActionResult) await controller.GetAllStocks();
        Assert.IsType<OkObjectResult>(result);
        
    }
}