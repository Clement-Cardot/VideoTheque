using Microsoft.AspNetCore.Mvc;
using VideoTheque.ViewModels;

namespace VideoTheque.Controllers.Emprunts
{
    public interface IEmpruntsServerController
    {
        [HttpGet]
        public Task<List<FilmViewModel>> GetFilmsEmpruntables();

        [HttpGet("{id}")]
        public FilmViewModel GetEmprunt([FromRoute] int id);

        [HttpDelete("{id}")]
        public Task<IResult> DeleteEmprunt([FromRoute] string name);
    }
}
