namespace MoviesDB.Models
{
    public class MovieSummary
    {
        public List<Movie> Movies;

        public MovieSummary()
        {
            Movies = new List<Movie>();
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            if (Movies.Count == 0)
            {
                Movies.AddRange(await InitialMoviesAsync());
            }
            return new List<Movie>(Movies);
        }

        private async Task<List<Movie>> InitialMoviesAsync()
        {
            Database database = new Database();
            return await database.GetAllMoviesAsync();
        }
    }
}
