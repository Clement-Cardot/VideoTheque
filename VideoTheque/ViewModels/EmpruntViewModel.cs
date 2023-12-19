using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VideoTheque.ViewModels
{
    public class EmpruntViewModel
    {
        [JsonPropertyName("titre")]
        [Required]
        public string Titre { get; set; }

        [JsonPropertyName("duree")]
        [Required]
        public int Duree { get; set; }

        [JsonPropertyName("support")]
        [Required]
        public string Support { get; set; }

        [JsonPropertyName("age-rating")]
        [Required]
        public AgeRatingViewModel AgeRating { get; set; }

        [JsonPropertyName("genre")]
        [Required]
        public GenreViewModel Genre { get; set; }

        [JsonPropertyName("realisateur")]
        [Required]
        public PersonneViewModel Director { get; set; }

        [JsonPropertyName("scenariste")]
        [Required]
        public PersonneViewModel Scenarist { get; set; }

        [JsonPropertyName("acteur-principal")]
        [Required]
        public PersonneViewModel FirstActor { get; set; }
    }
}
