using Mapster;
using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.Personnes;
using VideoTheque.ViewModels;

namespace VideoTheque.Businesses.Personnes
{
    public class PersonnesBusiness : IPersonnesBusiness
    {
        private readonly IPersonnesRepository _personneDao;

        public PersonnesBusiness(IPersonnesRepository personneDao)
        {
            _personneDao = personneDao;
            TypeAdapterConfig<PersonneDto, PersonneViewModel>.NewConfig()
                .Map(dest => dest.FullName, src => src.FirstName + " " + src.LastName);
        }

        public Task<List<PersonneDto>> GetPersonnes() => _personneDao.GetPersonnes();

        public PersonneDto GetPersonne(int id)
        {
            var personne = _personneDao.GetPersonne(id).Result;

            if (personne == null)
            {
                throw new NotFoundException($"Personne '{id}' non trouvé");
            }

            return personne;
        }

        public PersonneDto InsertPersonne(PersonneDto personne)
        {
            if (_personneDao.InsertPersonne(personne).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de l'insertion du personne {personne.Id}");
            }

            return personne;
        }

        public void UpdatePersonne(int id, PersonneDto personne)
        {
            if (_personneDao.UpdatePersonne(id, personne).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la modification du personne {personne.Id}");
            }
        }


        public void DeletePersonne(int id)
        {
            if (_personneDao.DeletePersonne(id).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la suppression du personne d'identifiant {id}");
            }
        }
    }
}
