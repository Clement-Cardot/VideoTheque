using System.Text.RegularExpressions;
using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.Hosts;

namespace VideoTheque.Businesses.Hosts
{
    public class HostsBusiness : IHostsBusiness
    {
        private readonly IHostsRepository _hostDao;

        public HostsBusiness(IHostsRepository hostDao)
        {
            _hostDao = hostDao;
        }

        public Task<List<HostDto>> GetHosts() => _hostDao.GetHosts();

        public HostDto GetHost(int id)
        {
            var host = _hostDao.GetHost(id).Result;

            if (host == null)
            {
                throw new NotFoundException($"Host '{id}' non trouvé");
            }

            return host;
        }

        public HostDto InsertHost(HostDto host)
        {
            const string pattern = "^(((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)\\.?\\b){4})\\:(\\d{1,5})$";
            if (!Regex.IsMatch(host.Url, pattern))
            {
                throw new InternalErrorException($"L'url entrée n'est pas valide : {host.Url}");
            }

            if (_hostDao.InsertHost(host).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de l'insertion du host {host.Name}");
            }

            return host;
        }

        public void UpdateHost(int id, HostDto host)
        {
            if (_hostDao.UpdateHost(id, host).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la modification du host {host.Name}");
            }
        }


        public void DeleteHost(int id)
        {
            if (_hostDao.DeleteHost(id).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la suppression du host d'identifiant {id}");
            }
        }
    }
}
