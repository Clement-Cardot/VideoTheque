using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.Supports;

namespace VideoTheque.Businesses.Supports
{
    public class SupportsBusiness : ISupportsBusiness
    {
        private readonly ISupportsRepository _supportDao;

        public SupportsBusiness(ISupportsRepository supportDao)
        {
            _supportDao = supportDao;
        }

        public Task<List<SupportDto>> GetSupports() => _supportDao.GetSupports();

        public SupportDto GetSupport(int id)
        {
            var support = _supportDao.GetSupport(id).Result;

            if (support == null)
            {
                throw new NotFoundException($"support '{id}' non trouvé");
            }

            return support;
        }        

        public SupportDto InsertSupport(SupportDto support)
        {
            if (_supportDao.InsertSupport(support).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de l'insertion du support {support.Name}");
            }

            return support;
        }

        public void UpdateSupport(int id, SupportDto support)
        {
            if (_supportDao.UpdateSupport(id, support).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la modification du support {support.Name}");
            }
        }

        public void DeleteSupport(int id)
        {
            if (_supportDao.DeleteSupport(id).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la suppression du support d'identifiant {id}");
            }
        }
    }
}
