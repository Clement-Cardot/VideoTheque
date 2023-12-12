using Microsoft.AspNetCore.Mvc;
using VideoTheque.Businesses.Emprunts;
using VideoTheque.Businesses.Genres;
using VideoTheque.DTOs;
using VideoTheque.ViewModels;

namespace VideoTheque.Controllers.Emprunts
{
    [ApiController]
    [Route("emprunts/client")]
    public class EmpruntsClientController : ControllerBase, IEmpruntsClientController
    {
        private readonly IEmpruntsClientBusiness _empruntsClientBusiness;
        protected readonly ILogger<EmpruntsClientController> _logger;
        

        public EmpruntsClientController(ILogger<EmpruntsClientController> logger, IEmpruntsClientBusiness empruntsClientBusiness)
        {
            _logger = logger;
            _empruntsClientBusiness = empruntsClientBusiness;
        }

        public Task<List<FilmViewModel>> GetFilmsEmpruntablesFromHost([FromRoute] int id)
        {
            return _empruntsClientBusiness.GetFilmsEmpruntablesFromHost(id);
        }

        public void GetEmprunt([FromRoute] int idHost, [FromRoute] int idFilm)
        {
            _empruntsClientBusiness.EmpruntFilm(idHost, idFilm);
        }
    }
}
