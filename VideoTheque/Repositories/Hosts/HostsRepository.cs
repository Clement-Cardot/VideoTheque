using Microsoft.EntityFrameworkCore;
using VideoTheque.Context;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Hosts
{
    public class HostsRepository : IHostsRepository
    {
        private readonly VideothequeDb _db;

        public HostsRepository(VideothequeDb db)
        {
            _db = db;
        }

        public Task<List<HostDto>> GetHosts() => _db.Hosts.ToListAsync();

        public ValueTask<HostDto?> GetHost(int id) => _db.Hosts.FindAsync(id);

        public Task InsertHost(HostDto host)
        {
            _db.Hosts.AddAsync(host);
            return _db.SaveChangesAsync();
        }

        public Task UpdateHost(int id, HostDto host)
        {
            var hostToUpdate = _db.Hosts.FindAsync(id).Result;

            if (hostToUpdate is null)
            {
                throw new KeyNotFoundException($"Host '{id}' non trouvé");
            }

            hostToUpdate.Name = host.Name;
            hostToUpdate.Url = host.Url;
            return _db.SaveChangesAsync();
        }

        public Task DeleteHost(int id)
        {
            var hostToDelete = _db.Hosts.FindAsync(id).Result;

            if (hostToDelete is null)
            {
                throw new KeyNotFoundException($"Host '{id}' non trouvé");
            }

            _db.Hosts.Remove(hostToDelete);
            return _db.SaveChangesAsync();
        }
    }
}
