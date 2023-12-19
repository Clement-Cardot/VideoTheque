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
        }

        public Task<List<FilmViewModel>> GetFilms()
        {
            return Task.Run(() =>
            {
                List<FilmViewModel> films = new List<FilmViewModel>();
                List<BluRayDto> blurays = _filmDao.GetFilms().Result;
                foreach(BluRayDto bluray in blurays)
                {
                    films.Add(this.convertToModelView(this.ConvertToFilm(bluray)));
                }
                return films;
            }
            );
        }

        public FilmViewModel GetFilm(int id)
        {
            var bluray = _filmDao.GetFilm(id).Result;

            if (bluray == null)
            {
                throw new NotFoundException($"Film '{id}' non trouvé");
            }

            return this.convertToModelView(this.ConvertToFilm(bluray));
        }

        public FilmViewModel InsertFilm(FilmViewModel filmMV)
        {
            FilmDto film = this.convertToDto(filmMV);
            if (_filmDao.InsertFilm(this.ConvertToBluray(film)).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de l'insertion du film {film.Title}");
            }

            return this.convertToModelView(film);
        }

        public void UpdateFilm(int id, FilmViewModel filmMV)
        {
            FilmDto film = this.convertToDto(filmMV);
            
            FilmDto actualfilm = this.ConvertToFilm(_filmDao.GetFilm(id).Result);
            if (actualfilm.IsAvailable == false || actualfilm.Owner != null)
            {
                throw new InternalErrorException($"Le film {film.Title} n'est pas modifiable pour le moment");
            }

            if (_filmDao.UpdateFilm(id, this.ConvertToBluray(film)).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la modification du film {film.Title}");
            }
        }

        public void DeleteFilm(int id)
        {
            FilmDto actualfilm = this.ConvertToFilm(_filmDao.GetFilm(id).Result);
            if (actualfilm.IsAvailable == false || actualfilm.Owner != null)
            {
                throw new InternalErrorException($"Le film {actualfilm.Title} ne peut pas être supprimer pour le moment");
            }

            if (_filmDao.DeleteFilm(id).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la suppression du film d'identifiant {id}");
            }
        }

        private FilmDto ConvertToFilm(BluRayDto bluray)
        {
            return new FilmDto
            {
                Id = bluray.Id,
                Title = bluray.Title,
                Duration = bluray.Duration,
                Genre = _genreDao.GetGenre(bluray.IdGenre).Result,
                AgeRating = _ageRatingDao.GetAgeRating(bluray.IdAgeRating).Result,
                Director = _personneDao.GetPersonne(bluray.IdDirector).Result,
                FirstActor = _personneDao.GetPersonne(bluray.IdFirstActor).Result,
                Scenarist = _personneDao.GetPersonne(bluray.IdScenarist).Result,
                Support = _supportDao.GetSupport(1).Result
            };
        }

        private BluRayDto ConvertToBluray(FilmDto film)
        {
            return new BluRayDto
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
                IdOwner = film.Owner?.Id
            };
        }

        private FilmViewModel convertToModelView(FilmDto filmDto)
        {
            return new FilmViewModel
            {
                Id = filmDto.Id,
                Titre = filmDto.Title,
                Duree = (int) filmDto.Duration,
                AgeRating = filmDto.AgeRating.Name,
                Genre = filmDto.Genre.Name,
                Support = filmDto.Support.Name,
                Director = filmDto.Director.FirstName + " " + filmDto.Director.LastName,
                Scenarist = filmDto.Scenarist.FirstName + " " + filmDto.Scenarist.LastName,
                FirstActor = filmDto.FirstActor.FirstName + " " + filmDto.FirstActor.LastName
            };
        }

        private FilmDto convertToDto(FilmViewModel filmVM)
        {
            AgeRatingDto? ageRating = _ageRatingDao.GetAgeRatingByName(filmVM.AgeRating).Result;
            SupportDto? support = _supportDao.GetSupportByName(filmVM.Support).Result;
            GenreDto? genre = _genreDao.GetGenreByName(filmVM.Genre).Result;
            PersonneDto? director = _personneDao.GetPersonneByFullName(filmVM.Director).Result;
            PersonneDto? scenarist = _personneDao.GetPersonneByFullName(filmVM.Scenarist).Result;
            PersonneDto? firstActor = _personneDao.GetPersonneByFullName(filmVM.FirstActor).Result;


            FilmDto film = new FilmDto
            {
                Id = filmVM.Id,
                Title = filmVM.Titre,
                Duration = filmVM.Duree,
                AgeRating = ageRating,
                Support = support,
                Genre = genre,
                Director = director,
                Scenarist = scenarist,
                FirstActor = firstActor
            };

            return film;
        }
    }
}
