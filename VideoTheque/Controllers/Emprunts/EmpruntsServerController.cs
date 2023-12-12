using Microsoft.AspNetCore.Mvc;
using VideoTheque.Businesses.Emprunts;
using VideoTheque.Businesses.Films;
using VideoTheque.ViewModels;

namespace VideoTheque.Controllers.Emprunts
{
    [ApiController]
    [Route("emprunts/server")]
    public class EmpruntsServerController : ControllerBase, IEmpruntsServerController
    {
        private readonly IEmpruntsServerBusiness _empruntsServerBusiness;

        protected readonly ILogger<EmpruntsServerController> _logger;

        public EmpruntsServerController(ILogger<EmpruntsServerController> logger, IEmpruntsServerBusiness empruntsServerBusiness)
        {
            _logger = logger;
            _empruntsServerBusiness = empruntsServerBusiness;
        }

        public async Task<List<FilmViewModel>> GetFilmsEmpruntables() => await _empruntsServerBusiness.GetFilmsEmpruntables();

        public FilmViewModel GetEmprunt([FromRoute] int id) => _empruntsServerBusiness.GetEmprunt(id);

        public async Task<IResult> DeleteEmprunt([FromRoute] string title)
        {
            _empruntsServerBusiness.DeleteEmprunt(title);
            return Results.Ok();
        }
    }
}
