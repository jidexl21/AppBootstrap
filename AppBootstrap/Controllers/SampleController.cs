using AppBootstrap.CQRS;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppBootstrap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SampleController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<GetCaptains.GetCaptainsResponse>> Get()
        {
            var request = new GetCaptains.GetCaptainsRequest();
            var response = await _mediator.Send(request);
            return this.Respond(response);
        }
    }
}
