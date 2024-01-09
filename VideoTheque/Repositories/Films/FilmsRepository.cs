using Microsoft.EntityFrameworkCore;
using VideoTheque.Context;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.Films
{
    public class FilmsRepository : IFilmsRepository
    {
        private readonly VideothequeDb _db;

        public FilmsRepository(VideothequeDb db)
        {
            _db = db;
        }

        public Task<List<BluRayDto>> GetFilms() => _db.BluRays.ToListAsync();

        public List<BluRayDto> GetFilmsEmpruntables() => _db.BluRays.Where(f => f.IsAvailable == true && f.IdOwner == null).ToList();

        public ValueTask<BluRayDto?> GetFilm(int id) => _db.BluRays.FindAsync(id);

        public BluRayDto GetFilmByName(string title) => (BluRayDto) _db.BluRays.Where(f => f.Title == title);

        public Task InsertFilm(BluRayDto film)
        {
            film.IsAvailable = true;
            _db.BluRays.AddAsync(film);
            return _db.SaveChangesAsync();
        }

        public Task UpdateFilm(int id, BluRayDto film)
        {
            var filmToUpdate = _db.BluRays.FindAsync(id).Result;

            if (filmToUpdate is null)
            {
                throw new KeyNotFoundException($"Film '{id}' non trouvé");
            }

            filmToUpdate.Title = film.Title;
            filmToUpdate.Duration = film.Duration;
            filmToUpdate.IdFirstActor = film.IdFirstActor;
            filmToUpdate.IdDirector = film.IdDirector;
            filmToUpdate.IdScenarist = film.IdScenarist;
            filmToUpdate.IdAgeRating = film.IdAgeRating;
            filmToUpdate.IdGenre = film.IdGenre;
            filmToUpdate.IsAvailable = film.IsAvailable;
            filmToUpdate.IdOwner = film.IdOwner;

            return _db.SaveChangesAsync();
        }

        public Task DeleteFilm(int id)
        {
            var filmToDelete = _db.BluRays.FindAsync(id).Result;

            if (filmToDelete is null)
            {
                throw new KeyNotFoundException($"Film '{id}' non trouvé");
            }

            _db.BluRays.Remove(filmToDelete);
            return _db.SaveChangesAsync();
        }
    }
}
