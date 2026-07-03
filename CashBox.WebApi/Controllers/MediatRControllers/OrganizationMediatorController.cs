using CashBox.Repository.Dtos.OrganizationDtos;
using CashBox.Service.MiatR_pattern.Organizations.Commands;
using CashBox.Service.MiatR_pattern.Organizations.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers.MiatRControllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrganizationMediatorController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrganizationMediatorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrganizationDto dto)
        {
            var result = await _mediator.Send(new CreateCommand(dto));
            return Ok(result);
        }
    }
}
