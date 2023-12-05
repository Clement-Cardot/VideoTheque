using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VideoTheque.DTOs;

namespace VideoTheque.ViewModels
{
    public class FilmViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("realisateur")]
        [Required]
        public string Director { get; set; }

        [JsonPropertyName("scenariste")]
        [Required]
        public string Scenarist { get; set; }

        [JsonPropertyName("duree")]
        [Required]
        public int Duree { get; set; }

        [JsonPropertyName("support")]
        [Required]
        public string Support = "BLURAY";

        [JsonPropertyName("age-rating")]
        [Required]
        public string AgeRating { get; set; }

        [JsonPropertyName("genre")]
        [Required]
        public string Genre { get; set; }

        [JsonPropertyName("titre")]
        [Required]
        public string Titre { get; set; }

        [JsonPropertyName("acteur-principal")]
        [Required]
        public string FirstActor { get; set; }


    }
}
