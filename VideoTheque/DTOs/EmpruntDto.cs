using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VideoTheque.ViewModels;

namespace VideoTheque.DTOs
{
    public class EmpruntDto
    {
        public string Titre { get; set; }

        public int Duree { get; set; }

        public string Support { get; set; }

        public AgeRatingDto AgeRating { get; set; }

        public GenreDto Genre { get; set; }

        public PersonneDto Director { get; set; }

        public PersonneDto Scenarist { get; set; }

        public PersonneDto FirstActor { get; set; }
    }
}
