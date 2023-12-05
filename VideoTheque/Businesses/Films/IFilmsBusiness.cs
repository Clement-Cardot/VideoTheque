using VideoTheque.DTOs;

namespace VideoTheque.Businesses.Films
{
    public interface IFilmsBusiness
    {
        Task<List<FilmDto>> GetFilms();

        FilmDto GetFilm(int id);

        FilmDto InsertFilm(FilmDto film);

        void UpdateFilm(int id, FilmDto film);

        void DeleteFilm(int id);
    }
}
