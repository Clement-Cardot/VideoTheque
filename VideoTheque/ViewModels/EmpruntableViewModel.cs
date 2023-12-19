using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VideoTheque.ViewModels
{
    public class EmpruntableViewModel
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("titre")]
        [Required]
        public string Titre { get; set; }
    }
}
