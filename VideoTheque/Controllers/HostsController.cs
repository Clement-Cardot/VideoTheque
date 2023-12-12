using Mapster;
using Microsoft.AspNetCore.Mvc;
using VideoTheque.Businesses.Hosts;
using VideoTheque.DTOs;
using VideoTheque.ViewModels;

namespace VideoTheque.Controllers
{
    [ApiController]
    [Route("hosts")]
    public class HostsController : ControllerBase
    {
        private readonly IHostsBusiness _hostsBusiness;
        protected readonly ILogger<HostsController> _logger;

        public HostsController(ILogger<HostsController> logger, IHostsBusiness hostsBusiness)
        {
            _logger = logger;
            _hostsBusiness = hostsBusiness;
        }

        [HttpGet]
        public async Task<List<HostViewModel>> GetHosts() => (await _hostsBusiness.GetHosts()).Adapt<List<HostViewModel>>();

        [HttpGet("{id}")]
        public async Task<HostViewModel> GetHost([FromRoute] int id) => _hostsBusiness.GetHost(id).Adapt<HostViewModel>();

        [HttpPost]
        public async Task<IResult> InsertHost([FromBody] HostViewModel hostVM)
        {
            var created = _hostsBusiness.InsertHost(hostVM.Adapt<HostDto>());
            return Results.Created($"/hosts/{created.Id}", created);
        }

        [HttpPut("{id}")]
        public async Task<IResult> UpdateHost([FromRoute] int id, [FromBody] HostViewModel hostVM)
        {
            _hostsBusiness.UpdateHost(id, hostVM.Adapt<HostDto>());
            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteHost([FromRoute] int id)
        {
            _hostsBusiness.DeleteHost(id);
            return Results.Ok();
        }
    }
}
