using VideoTheque.DTOs;
using VideoTheque.ViewModels;

namespace VideoTheque.Businesses.Emprunts
{
    public interface IEmpruntsServerBusiness
    {
        Task<List<EmpruntableDto>> GetFilmsEmpruntables();

        EmpruntDto GetEmprunt(int idFilm);

        void DeleteEmprunt(string titre);
    }
}
