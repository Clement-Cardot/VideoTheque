namespace VideoTheque.Controllers
{
    using Mapster;
    using Microsoft.AspNetCore.Mvc;
    using VideoTheque.Businesses.AgeRating;
    using VideoTheque.Businesses.Supports;
    using VideoTheque.DTOs;
    using VideoTheque.ViewModels;

    [ApiController]
    [Route("supports")]
    public class SupportsController : ControllerBase
    {
        private readonly ISupportsBusiness _supportsBusiness;
        private readonly ILogger<SupportsController> _logger;

        public SupportsController(ILogger<SupportsController> logger, ISupportsBusiness supportsBusiness)
        {
            _logger = logger;
            _supportsBusiness = supportsBusiness;
        }

        [HttpGet]
        public async Task<List<SupportViewModel>> GetSupports() => (await _supportsBusiness.GetSupports()).Adapt<List<SupportViewModel>>();

        [HttpGet("{id}")]
        public async Task<SupportViewModel> GetSupport([FromRoute] int id) => _supportsBusiness.GetSupport(id).Adapt<SupportViewModel>();

        [HttpPost]
        public async Task<IResult> InsertSupport([FromBody] SupportViewModel supportVM)
        {
            var created = _supportsBusiness.InsertSupport(supportVM.Adapt<SupportDto>());
            return Results.Created($"/support/{created.Id}", created);
        }

        [HttpPut("{id}")]
        public async Task<IResult> UpdateSupport([FromRoute] int id, [FromBody] SupportViewModel supportVM)
        {
            _supportsBusiness.UpdateSupport(id, supportVM.Adapt<SupportDto>());
            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteSupport([FromRoute] int id)
        {
            _supportsBusiness.DeleteSupport(id);
            return Results.Ok();
        }
    }
}
