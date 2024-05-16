namespace MoviesDB.Models
{
    public class MovieSummary
    {
        private readonly List<Movie> _movies;

        public MovieSummary()
        {
            _movies = InitialMovies();
        }

        public List<Movie> GetMovies()
        {
            return new List<Movie>(_movies);
        }

        private List<Movie> InitialMovies()
        {
            Database database = new Database();

            return database.GetAllMovies();
        }
    }
}
