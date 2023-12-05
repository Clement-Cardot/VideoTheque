using Mapster;
using System.IO;
using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.AgeRating;
using VideoTheque.Repositories.Films;
using VideoTheque.Repositories.Genres;
using VideoTheque.Repositories.Personnes;
using VideoTheque.Repositories.Supports;
using VideoTheque.ViewModels;

namespace VideoTheque.Businesses.Films
{
    public class FilmsBusiness : IFilmsBusiness
    {
        private readonly IFilmsRepository _filmDao;
        private readonly IPersonnesRepository _personneDao;
        private readonly IGenresRepository _genreDao;
        private readonly IAgeRatingsRepository _ageRatingDao;
        private readonly ISupportsRepository _supportDao;

        public FilmsBusiness(
            IFilmsRepository filmDao,
            IPersonnesRepository personnesRepository,
            IGenresRepository genresRepository,
            IAgeRatingsRepository ageRatingsRepository,
            ISupportsRepository supportsRepository
            )
        {
            _filmDao = filmDao;
            _personneDao = personnesRepository;
            _genreDao = genresRepository;
            _ageRatingDao = ageRatingsRepository;
            _supportDao = supportsRepository;

            TypeAdapterConfig<FilmDto, FilmViewModel>.NewConfig()
                .Map(dest => dest.Director, src => src.Director.FirstName + " " + src.Director.LastName)
                .Map(dest => dest.Scenarist, src => src.Scenarist.FirstName + " " + src.Scenarist.LastName)
                .Map(dest => dest.FirstActor, src => src.FirstActor.FirstName + " " + src.FirstActor.LastName)
                .Map(dest => dest.Genre, src => src.Genre.Name)
                .Map(dest => dest.AgeRating, src => src.AgeRating.Name)
                .Map(dest => dest.Support, src => src.Support.Name);

            TypeAdapterConfig<FilmViewModel, FilmDto>.NewConfig()
                .Map(dest => dest.Director, src => _personneDao.GetPersonneByFullName(src.Director))
                .Map(dest => dest.Scenarist, src => _personneDao.GetPersonneByFullName(src.Scenarist))
                .Map(dest => dest.FirstActor, src => _personneDao.GetPersonneByFullName(src.FirstActor))
                .Map(dest => dest.Genre, src => _genreDao.GetGenreByName(src.Genre))
                .Map(dest => dest.AgeRating, src => _ageRatingDao.GetAgeRatingByName(src.AgeRating))
                .Map(dest => dest.Support, src => src.Support);
        }

        public Task<List<FilmDto>> GetFilms()
        {
            return Task.Run(() =>
            {
                List<FilmDto> films = new List<FilmDto>();
                List<BluRayDto> blurays = _filmDao.GetFilms().Result;
                foreach(BluRayDto bluray in blurays)
                {
                    films.Add(this.ConvertToFilm(bluray));
                }
                return films;
            }
            );
        }

        public FilmDto GetFilm(int id)
        {
            var bluray = _filmDao.GetFilm(id).Result;

            if (bluray == null)
            {
                throw new NotFoundException($"Film '{id}' non trouvé");
            }

            return this.ConvertToFilm(bluray);
        }

        public FilmDto InsertFilm(FilmDto film)
        {
            if (_filmDao.InsertFilm(this.ConvertToBluray(film)).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de l'insertion du film {film.Title}");
            }

            return film;
        }

        public void UpdateFilm(int id, FilmDto film)
        {
            if (_filmDao.UpdateFilm(id, this.ConvertToBluray(film)).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la modification du film {film.Title}");
            }
        }

        public void DeleteFilm(int id)
        {
            if (_filmDao.DeleteFilm(id).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la suppression du film d'identifiant {id}");
            }
        }

        private FilmDto ConvertToFilm(BluRayDto bluray)
        {
            FilmDto film = new FilmDto
            {
                Id = bluray.Id,
                Title = bluray.Title,
                Duration = bluray.Duration,
                IsAvailable = bluray.IsAvailable,
                Genre = _genreDao.GetGenre(bluray.IdGenre).Result,
                AgeRating = _ageRatingDao.GetAgeRating(bluray.IdAgeRating).Result,
                Director = _personneDao.GetPersonne(bluray.IdDirector).Result,
                FirstActor = _personneDao.GetPersonne(bluray.IdFirstActor).Result,
                Scenarist = _personneDao.GetPersonne(bluray.IdScenarist).Result,
                Owner = _personneDao.GetPersonne((int)bluray.IdOwner).Result,
                Support = _supportDao.GetSupport(1).Result
            };

            return film;
        }

        private BluRayDto ConvertToBluray(FilmDto film)
        {
            BluRayDto bluray = new BluRayDto
            {
                Id = film.Id,
                Title = film.Title,
                Duration = film.Duration,
                IsAvailable = film.IsAvailable,
                IdGenre = film.Genre.Id,
                IdAgeRating = film.AgeRating.Id,
                IdDirector = film.Director.Id,
                IdFirstActor = film.FirstActor.Id,
                IdScenarist = film.Scenarist.Id,
                IdOwner = film.Owner.Id
            };

            return bluray;
        }
    }
}
