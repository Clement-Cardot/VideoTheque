using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Films
{
    public interface IFilmsRepository
    {
        Task<List<BluRayDto>> GetFilms();

        List<BluRayDto> GetFilmsEmpruntables();

        ValueTask<BluRayDto?> GetFilm(int id);

        BluRayDto GetFilmByName(string title);

        Task InsertFilm(BluRayDto film);

        Task UpdateFilm(int id, BluRayDto film);

        Task DeleteFilm(int id);
    }
}
