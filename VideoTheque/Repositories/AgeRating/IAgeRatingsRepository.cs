using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoTheque.DTOs;

namespace VideoTheque.Repositories.AgeRating
{
    public interface IAgeRatingsRepository
    {
        Task<List<AgeRatingDto>> GetAgeRatings();

        ValueTask<AgeRatingDto?> GetAgeRating(int id);

        ValueTask<AgeRatingDto?> GetAgeRatingByName(string name);

        ValueTask<AgeRatingDto?> GetAgeRatingByAbreviation(string abreviation);

        Task InsertAgeRating(AgeRatingDto AgeRating);

        Task UpdateAgeRating(int id, AgeRatingDto AgeRating);

        Task DeleteAgeRating(int id);
    }
}
