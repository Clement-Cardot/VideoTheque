namespace VideoTheque.DTOs
{
    public class FilmDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public long Duration { get; set; }
        public PersonneDto FirstActor { get; set; }
        public PersonneDto Director { get; set; }
        public PersonneDto Scenarist { get; set; }
        public AgeRatingDto AgeRating { get; set; }
        public SupportDto Support { get; set; }
        public GenreDto Genre { get; set; }
        public bool IsAvailable { get; set; }
        public PersonneDto? Owner { get; set; }
    }
}
