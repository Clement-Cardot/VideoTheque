using VideoTheque.ViewModels;

namespace VideoTheque.Businesses.Emprunts
{
    public interface IEmpruntsServerBusiness
    {
        Task<List<FilmViewModel>> GetFilmsEmpruntables();

        FilmViewModel GetEmprunt(int idFilm);

        void DeleteEmprunt(string title);
    }
}
