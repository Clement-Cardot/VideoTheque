using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Personnes
{
    public interface IPersonnesRepository
    {
        Task<List<PersonneDto>> GetPersonnes();

        ValueTask<PersonneDto?> GetPersonne(int id);

        ValueTask<PersonneDto?> GetPersonneByFullName(string fullName);

        Task InsertPersonne(PersonneDto personne);

        Task UpdatePersonne(int id, PersonneDto personne);

        Task DeletePersonne(int id);
    }
}
