using VideoTheque.DTOs;
using VideoTheque.ViewModels;

namespace VideoTheque.Businesses.Films
{
    public interface IFilmsBusiness
    {
        Task<List<FilmViewModel>> GetFilms();

        FilmViewModel GetFilm(int id);

        FilmViewModel InsertFilm(FilmViewModel film);

        void UpdateFilm(int id, FilmViewModel film);

        void DeleteFilm(int id);
    }
}
