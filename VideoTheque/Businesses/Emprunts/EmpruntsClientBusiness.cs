using VideoTheque.Repositories.AgeRating;
using VideoTheque.Repositories.Films;
using VideoTheque.Repositories.Genres;
using VideoTheque.Repositories.Personnes;
using VideoTheque.Repositories.Supports;
using VideoTheque.Repositories.Hosts;
using VideoTheque.ViewModels;
using VideoTheque.DTOs;
using Mapster;
using System.Collections.Generic;

namespace VideoTheque.Businesses.Emprunts
{
    public class EmpruntsClientBusiness : IEmpruntsClientBusiness
    {
        private static HttpClient httpClient = new();

        private readonly IFilmsRepository _filmDao;
        private readonly IPersonnesRepository _personneDao;
        private readonly IGenresRepository _genreDao;
        private readonly IAgeRatingsRepository _ageRatingDao;
        private readonly ISupportsRepository _supportDao;
        private readonly IHostsRepository _hostDao;

        public EmpruntsClientBusiness(
            IFilmsRepository filmDao,
            IPersonnesRepository personnesRepository,
            IGenresRepository genresRepository,
            IAgeRatingsRepository ageRatingsRepository,
            ISupportsRepository supportsRepository,
            IHostsRepository hostDao
            )
        {
            _filmDao = filmDao;
            _personneDao = personnesRepository;
            _genreDao = genresRepository;
            _ageRatingDao = ageRatingsRepository;
            _supportDao = supportsRepository;
            _hostDao = hostDao;
        }

        public Task<List<EmpruntableViewModel>> GetFilmsEmpruntablesFromHost(int idHost)
        {
            HostDto host = _hostDao.GetHost(idHost).Result;

            Task<List<EmpruntableViewModel>> empruntables = httpClient.GetAsync(host.Url + "/films/empruntables").Adapt<Task<List<EmpruntableViewModel>>>();

            return empruntables;
        }

        public void EmpruntFilm(int idHost, int idFilm)
        {
            HostDto host = _hostDao.GetHost(idHost).Result;

            EmpruntViewModel emprunt = httpClient.PostAsync(host.Url + "/films/empruntables/" + idFilm, null).Result.Adapt<EmpruntViewModel>();

            SupportDto support = _supportDao.GetSupportByName(emprunt.Support).Result;
            AgeRatingDto ageRating = _ageRatingDao.GetAgeRatingByName(emprunt.AgeRating.Name).Result;
            GenreDto genre = _genreDao.GetGenreByName(emprunt.Genre.Name).Result;
            PersonneDto director = _personneDao.GetPersonneByFullName(emprunt.Director.FullName).Result;
            PersonneDto firstActor = _personneDao.GetPersonneByFullName(emprunt.FirstActor.FullName).Result;
            PersonneDto scenarist = _personneDao.GetPersonneByFullName(emprunt.Scenarist.FullName).Result;
            
            if (ageRating == null)
            {
                AgeRatingDto newAgeRating = emprunt.AgeRating.Adapt<AgeRatingDto>();
                _ageRatingDao.InsertAgeRating(newAgeRating);
                ageRating = _ageRatingDao.GetAgeRatingByName(emprunt.AgeRating.Name).Result;
            }

            if (genre == null)
            {
                GenreDto newGenre = emprunt.Genre.Adapt<GenreDto>();
                _genreDao.InsertGenre(newGenre);
                genre = _genreDao.GetGenreByName(emprunt.Genre.Name).Result;
            }

            if (director == null)
            {
                PersonneDto newDirector = emprunt.Director.Adapt<PersonneDto>();
                _personneDao.InsertPersonne(newDirector);
                director = _personneDao.GetPersonneByFullName(emprunt.Director.FullName).Result;
            }

            if (firstActor == null)
            {
                PersonneDto newFirstActor = emprunt.FirstActor.Adapt<PersonneDto>();
                _personneDao.InsertPersonne(newFirstActor);
                firstActor = _personneDao.GetPersonneByFullName(emprunt.FirstActor.FullName).Result;
            }

            if (scenarist == null)
            {
                PersonneDto newScenarist = emprunt.Scenarist.Adapt<PersonneDto>();
                _personneDao.InsertPersonne(newScenarist);
                scenarist = _personneDao.GetPersonneByFullName(emprunt.Scenarist.FullName).Result;
            }

            FilmDto newFilm = new FilmDto
            {
                Title = emprunt.Titre,
                Duration = emprunt.Duree,
                Support = support,
                AgeRating = ageRating,
                Genre = genre,
                Director = director,
                FirstActor = firstActor,
                Scenarist = scenarist,
                Owner = host,
                IsAvailable = true
            };

            _filmDao.InsertFilm(ConvertToBluray(newFilm));
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
                Support = _supportDao.GetSupport(1).Result,
                Owner = _hostDao.GetHost(bluray.IdOwner ?? default(int)).Result
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
