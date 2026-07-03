using CashBox.Repository.Dtos.UserDtos;
using CashBox.Service.MiatR_pattern.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers.MediatRControllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserMediatorController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserMediatorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            var result = await _mediator.Send(new CreateUserCommand(dto));
            return Ok(result);
        }
    }
}
