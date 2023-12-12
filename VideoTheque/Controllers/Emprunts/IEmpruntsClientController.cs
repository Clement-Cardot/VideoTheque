using Microsoft.AspNetCore.Mvc;
using VideoTheque.ViewModels;

namespace VideoTheque.Controllers.Emprunts
{
    public interface IEmpruntsClientController
    {
        [HttpGet("{id}")]
        public Task<List<FilmViewModel>> GetFilmsEmpruntablesFromHost([FromRoute] int id);

        [HttpGet("{idHost}/{idFilm}")]
        public void GetEmprunt([FromRoute] int idHost, [FromRoute] int idFilm);
    }
}
