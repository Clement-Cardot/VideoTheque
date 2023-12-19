using Mapster;
using Microsoft.AspNetCore.Mvc;
using VideoTheque.Businesses.Emprunts;
using VideoTheque.ViewModels;

namespace VideoTheque.Controllers.Emprunts
{
    [ApiController]
    [Route("emprunts/server")]
    public class EmpruntsServerController : ControllerBase
    {
        private readonly IEmpruntsServerBusiness _empruntsServerBusiness;

        protected readonly ILogger<EmpruntsServerController> _logger;

        public EmpruntsServerController(ILogger<EmpruntsServerController> logger, IEmpruntsServerBusiness empruntsServerBusiness)
        {
            _logger = logger;
            _empruntsServerBusiness = empruntsServerBusiness;
        }

        [HttpGet]
        public async Task<List<EmpruntableViewModel>> GetFilmsEmpruntables() => (await _empruntsServerBusiness.GetFilmsEmpruntables()).Adapt<List<EmpruntableViewModel>>();

        [HttpPost("{id}")]
        public EmpruntViewModel GetEmprunt([FromRoute] int id) => _empruntsServerBusiness.GetEmprunt(id).Adapt<EmpruntViewModel>();

        [HttpDelete("{titre}")]
        public async Task<IResult> DeleteEmprunt([FromRoute] string titre)
        {
            _empruntsServerBusiness.DeleteEmprunt(titre);
            return Results.Ok();
        }
    }
}
