namespace VideoTheque.Controllers
{
    using Mapster;
    using Microsoft.AspNetCore.Mvc;
    using VideoTheque.Businesses.AgeRating;
    using VideoTheque.Businesses.Emprunts;
    using VideoTheque.Businesses.Films;
    using VideoTheque.Businesses.Genres;
    using VideoTheque.Businesses.Personnes;
    using VideoTheque.DTOs;
    using VideoTheque.ViewModels;

    [ApiController]
    [Route("films")]
    public class FilmsController : ControllerBase
    {
        private readonly IFilmsBusiness _filmsBusiness;
        private readonly IEmpruntsServerBusiness _empruntsServerBusiness;
        protected readonly ILogger<FilmsController> _logger;

        public FilmsController(ILogger<FilmsController> logger, IFilmsBusiness filmsBusiness, IEmpruntsServerBusiness empruntsServerBusiness)
        {
            _logger = logger;
            _filmsBusiness = filmsBusiness;
            _empruntsServerBusiness = empruntsServerBusiness;
        }

        [HttpGet]
        public async Task<List<FilmViewModel>> GetFilms() => await _filmsBusiness.GetFilms();

        [HttpGet("{id}")]
        public async Task<FilmViewModel> GetFilm([FromRoute] int id) => _filmsBusiness.GetFilm(id);

        [HttpPost]
        public async Task<IResult> InsertFilm([FromBody] FilmViewModel filmVM)
        {
            var created = _filmsBusiness.InsertFilm(filmVM);
            return Results.Created($"/films/{created.Id}", created);
        }

        [HttpPut("{id}")]
        public async Task<IResult> UpdateFilm([FromRoute] int id, [FromBody] FilmViewModel filmVM)
        {
            _filmsBusiness.UpdateFilm(id, filmVM);
            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteFilm([FromRoute] int id)
        {
            _filmsBusiness.DeleteFilm(id);
            return Results.Ok();
        }

        // Empruntables
        [HttpGet("/empruntables")]
        public async Task<List<EmpruntableViewModel>> GetFilmsEmpruntables() => (await _empruntsServerBusiness.GetFilmsEmpruntables()).Adapt<List<EmpruntableViewModel>>();

        [HttpPost("/empruntables/{id}")]
        public EmpruntViewModel GetEmprunt([FromRoute] int id) => _empruntsServerBusiness.GetEmprunt(id).Adapt<EmpruntViewModel>();

        [HttpDelete("/empruntables/{titre}")]
        public async Task<IResult> DeleteEmprunt([FromRoute] string titre)
        {
            _empruntsServerBusiness.DeleteEmprunt(titre);
            return Results.Ok();
        }
    }
}
