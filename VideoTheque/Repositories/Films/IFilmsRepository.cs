using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Films
{
    public interface IFilmsRepository
    {
        Task<List<BluRayDto>> GetFilms();

        ValueTask<BluRayDto?> GetFilm(int id);

        Task InsertFilm(BluRayDto film);

        Task UpdateFilm(int id, BluRayDto film);

        Task DeleteFilm(int id);
    }
}
