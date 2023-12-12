using VideoTheque.DTOs;

namespace VideoTheque.Businesses.Hosts
{
    public interface IHostsBusiness
    {
        Task<List<HostDto>> GetHosts();

        HostDto GetHost(int id);

        HostDto InsertHost(HostDto host);

        void UpdateHost(int id, HostDto host);

        void DeleteHost(int id);
    }
}
