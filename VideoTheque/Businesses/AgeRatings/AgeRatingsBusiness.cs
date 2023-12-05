using VideoTheque.Core;
using VideoTheque.DTOs;
using VideoTheque.Repositories.AgeRating;

namespace VideoTheque.Businesses.AgeRating
{
    public class AgeRatingsBusiness : IAgeRatingsBusiness
    {
        private readonly IAgeRatingsRepository _ageRatingDao;

        public AgeRatingsBusiness(IAgeRatingsRepository ageRatingDao)
        {
            _ageRatingDao = ageRatingDao;
        }

        public Task<List<AgeRatingDto>> GetAgeRatings() => _ageRatingDao.GetAgeRatings();

        public AgeRatingDto GetAgeRating(int id)
        {
            var ageRating = _ageRatingDao.GetAgeRating(id).Result;

            if (ageRating == null)
            {
                throw new NotFoundException($"ageRating '{id}' non trouvé");
            }

            return ageRating;
        }

        public AgeRatingDto InsertAgeRating(AgeRatingDto ageRating)
        {
            if (_ageRatingDao.InsertAgeRating(ageRating).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de l'insertion du ageRating {ageRating.Name}");
            }

            return ageRating;
        }

        public void UpdateAgeRating(int id, AgeRatingDto ageRating)
        {
            if (_ageRatingDao.UpdateAgeRating(id, ageRating).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la modification du ageRating {ageRating.Name}");
            }
        }


        public void DeleteAgeRating(int id)
        {
            if (_ageRatingDao.DeleteAgeRating(id).IsFaulted)
            {
                throw new InternalErrorException($"Erreur lors de la suppression du ageRating d'identifiant {id}");
            }
        }
    }
}
