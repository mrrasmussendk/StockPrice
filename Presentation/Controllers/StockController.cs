using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/stock")]
public class StockController(IStockUseCase stockUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllStocks()
    {
        var stocks = await stockUseCase.GetAllStocks();
        return Ok(stocks);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStockByTicker(string id)
    {
        try
        {
            var stock = await stockUseCase.GetStockPrice(id);
            return Ok(stock);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new {ex.Message });
        }
    }
}
