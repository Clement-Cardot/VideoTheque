using Microsoft.AspNetCore.Mvc;
using VideoTheque.Businesses.Emprunts;
using VideoTheque.Businesses.Genres;
using VideoTheque.DTOs;
using VideoTheque.ViewModels;

namespace VideoTheque.Controllers
{
    [ApiController]
    [Route("emprunts")]
    public class EmpruntsController : ControllerBase
    {
        private readonly IEmpruntsClientBusiness _empruntsClientBusiness;
        protected readonly ILogger<EmpruntsController> _logger;


        public EmpruntsController(ILogger<EmpruntsController> logger, IEmpruntsClientBusiness empruntsClientBusiness)
        {
            _logger = logger;
            _empruntsClientBusiness = empruntsClientBusiness;
        }

        [HttpGet("{idHost}")]
        public Task<List<EmpruntableViewModel>> GetFilmsEmpruntablesFromHost([FromRoute] int idHost)
        {
            return _empruntsClientBusiness.GetFilmsEmpruntablesFromHost(idHost);
        }

        [HttpPost("{idHost}/{idFilm}")]
        public void GetEmprunt([FromRoute] int idHost, [FromRoute] int idFilm)
        {
            _empruntsClientBusiness.EmpruntFilm(idHost, idFilm);
        }
    }
}
