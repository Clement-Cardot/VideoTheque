using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.AgeRating;
using VideoTheque.Repositories.Films;
using VideoTheque.Repositories.Genres;
using VideoTheque.Repositories.Personnes;
using VideoTheque.Repositories.Supports;
using VideoTheque.ViewModels;

namespace VideoTheque.Businesses.Emprunts
{
    public class EmpruntsServerBusiness : IEmpruntsServerBusiness
    {

        private readonly IFilmsRepository _filmDao;
        private readonly IPersonnesRepository _personneDao;
        private readonly IGenresRepository _genreDao;
        private readonly IAgeRatingsRepository _ageRatingDao;
        private readonly ISupportsRepository _supportDao;

        public EmpruntsServerBusiness(
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

        public Task<List<EmpruntableDto>> GetFilmsEmpruntables()
        {
            return Task.Run(() =>
            {
                List<EmpruntableDto> empruntables = new List<EmpruntableDto>();
                List<BluRayDto> blurays = _filmDao.GetFilmsEmpruntables();
                foreach (BluRayDto bluray in blurays)
                {
                    empruntables.Add(
                        new EmpruntableDto
                        {
                            Id = bluray.Id,
                            Titre = bluray.Title
                        }
                    );
                }
                return empruntables;
            }
            );
        }

        public EmpruntDto GetEmprunt(int id)
        {
            var bluray = _filmDao.GetFilm(id).Result;

            if (bluray == null)
            {
                throw new NotFoundException($"Film '{id}' non trouvé");
            }

            if (bluray.IsAvailable == false)
            {
                throw new NotFoundException($"Film '{id}' n'est pas disponible");
            }

            if (bluray.IdOwner != null)
            {
                throw new NotFoundException($"Film '{id}' ne m'appartient pas");
            }

            SupportDto support = _supportDao.GetSupport(0).Result;
            AgeRatingDto ageRating = _ageRatingDao.GetAgeRating(bluray.IdAgeRating).Result;
            GenreDto genre = _genreDao.GetGenre(bluray.IdGenre).Result;
            PersonneDto director = _personneDao.GetPersonne(bluray.IdDirector).Result;
            PersonneDto firstActor = _personneDao.GetPersonne(bluray.IdFirstActor).Result;
            PersonneDto scenarist = _personneDao.GetPersonne(bluray.IdScenarist).Result;

            EmpruntDto emprunt = new EmpruntDto
            {
                Titre = bluray.Title,
                Duree = (int) bluray.Duration,
                Support = support.Name,
                AgeRating = ageRating,
                Genre = genre,
                Director = director,
                FirstActor = firstActor,
                Scenarist = scenarist
            };

            bluray.IsAvailable = false;
            _filmDao.UpdateFilm(id, bluray);

            return emprunt;
        }

        public void DeleteEmprunt(string title)
        {
            var bluray = _filmDao.GetFilmByName(title);

            if (bluray == null)
            {
                throw new NotFoundException($"Film '{title}' non trouvé");
            }

            if (bluray.IsAvailable == true)
            {
                throw new NotFoundException($"Film '{title}' est déjà disponible");
            }

            bluray.IsAvailable = true;
            _filmDao.UpdateFilm(bluray.Id, bluray);
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
                Duree = (int)filmDto.Duration,
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
