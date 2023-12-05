namespace VideoTheque.Controllers
{
    using Mapster;
    using Microsoft.AspNetCore.Mvc;
    using VideoTheque.Businesses.AgeRating;
    using VideoTheque.DTOs;
    using VideoTheque.ViewModels;

    [ApiController]
    [Route("age-ratings")]
    public class AgeRatingsController : ControllerBase
    {
        private readonly IAgeRatingsBusiness _ageRatingsBusiness;
        protected readonly ILogger<AgeRatingsController> _logger;

        public AgeRatingsController(ILogger<AgeRatingsController> logger, IAgeRatingsBusiness ageRatingsBusiness)
        {
            _logger = logger;
            _ageRatingsBusiness = ageRatingsBusiness;
        }

        [HttpGet]
        public async Task<List<AgeRatingViewModel>> GetAgeRatings() => (await _ageRatingsBusiness.GetAgeRatings()).Adapt<List<AgeRatingViewModel>>();

        [HttpGet("{id}")]
        public async Task<AgeRatingViewModel> GetAgeRating([FromRoute] int id) => _ageRatingsBusiness.GetAgeRating(id).Adapt<AgeRatingViewModel>();

        [HttpPost]
        public async Task<IResult> InsertAgeRating([FromBody] AgeRatingViewModel ageRatingVM)
        {
            var created = _ageRatingsBusiness.InsertAgeRating(ageRatingVM.Adapt<AgeRatingDto>());
            return Results.Created($"/age-ratings/{created.Id}", created);
        }

        [HttpPut("{id}")]
        public async Task<IResult> UpdateAgeRating([FromRoute] int id, [FromBody] AgeRatingViewModel ageRatingVM)
        {
            _ageRatingsBusiness.UpdateAgeRating(id, ageRatingVM.Adapt<AgeRatingDto>());
            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteAgeRating([FromRoute] int id)
        {
            _ageRatingsBusiness.DeleteAgeRating(id);
            return Results.Ok();
        }

    }

}
