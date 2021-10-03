using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTO;
using Application.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/values
        [HttpGet("{searchedText}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get(string searchedText) 
            => await _mediator.Send(new List.Query() { SearchedText = searchedText });

        [HttpPut]
        public async Task<ActionResult<Unit>> Update([FromBody] EditWeight.Command command)
            => await _mediator.Send(command);

    }
}