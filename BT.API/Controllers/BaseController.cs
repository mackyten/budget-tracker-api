using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BT.API.Controllers
{
    [ApiController]
    //[Authorize(Roles = "Member", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class BaseController : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator => _mediator ??= (_mediator = HttpContext.RequestServices.GetService<IMediator>()!);
    }
}
