using MediatR;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable 1591
namespace HaruGaKita.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        protected BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}