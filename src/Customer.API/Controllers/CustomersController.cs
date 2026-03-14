using Customer.Application.Customer.Commands.CreateCustomer;
using Customer.Application.Customer.Queries.GetCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CustomersController(ISender sender, ILogger<CustomersController> logger) : BaseController(logger)
{
    private readonly ISender _sender = sender;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(Guid id)
    {
        var result = await _sender.Send(new GetCustomerQuery(id));
        return result.IsSuccess ? Ok(result.Value) : HandleError(result.Error);
    }
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command)
    {
        var result = await _sender.Send(command);
        if(result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetCustomer), new { id = result.Value}, result.Value);
        }
        return HandleError(result.Error);
    }
}