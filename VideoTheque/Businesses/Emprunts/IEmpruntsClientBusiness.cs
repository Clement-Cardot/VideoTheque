using VideoTheque.ViewModels;

namespace VideoTheque.Businesses.Emprunts
{
    public interface IEmpruntsClientBusiness
    {
        Task<List<EmpruntableViewModel>> GetFilmsEmpruntablesFromHost(int idHost);

        void EmpruntFilm(int idHost, int idFilm);
    }
}
