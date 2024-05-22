using MoviesDB.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace MoviesDB
{
    public class Database
    {
        private readonly string _host;
        private readonly string _database;
        private readonly string _user;
        private readonly string _password;
        private readonly int _port;

        private MySqlConnection _connection;

        public Database()
        {
            _host = "mc.koudata.fi";
            _database = "moviedb";
            _user = "dbuser";
            _password = "Nakkikastike123!";
            _port = 3306;

            string connectionQuery = $"Server={_host};Port={_port};Database={_database};Uid={_user};Pwd={_password};";

            _connection = new MySqlConnection(connectionQuery);
        }

        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            List<Movie> movies = new List<Movie>();

            string query = "SELECT * FROM movies ORDER BY idMovies DESC;";

            await OpenConnectionAsync();
            MySqlCommand mySqlCommand = new MySqlCommand(query, _connection);

            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);

            DataTable dataTable = new DataTable();

            mySqlDataAdapter.Fill(dataTable);
            CloseConnection();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                try
                {
                    Movie movie = new Movie(
                        Convert.ToString(dataRow["Name"]),
                        Convert.ToInt32(dataRow["idMovies"]),
                        Convert.ToString(dataRow["Director"]),
                        Convert.ToInt32(dataRow["ReleaseYear"]),
                        Convert.ToInt32(dataRow["Length"]),
                        Convert.ToDouble(dataRow["Rating"]),
                        Convert.ToString(dataRow["Genres"]),
                        Convert.ToString(dataRow["MainActors"]),
                        Convert.ToString(dataRow["Image"])
                    );

                    movies.Add(movie);
                }
                catch (FormatException ex)
                {
                    // Log or handle the exception appropriately
                    Console.WriteLine($"Error converting DataRow to Movie: {ex.Message}");
                }
            }

            return movies;
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            Movie movie = null;

            string query = "SELECT * FROM movies WHERE idMovies = @id;";

            await OpenConnectionAsync();
            MySqlCommand mySqlCommand = new MySqlCommand(query, _connection);
            mySqlCommand.Parameters.AddWithValue("@id", id);

            try
            {
                MySqlDataReader reader = (MySqlDataReader)await mySqlCommand.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    movie = new Movie(
                        Convert.ToString(reader["Name"]),
                        Convert.ToInt32(reader["idMovies"]),
                        Convert.ToString(reader["Director"]),
                        Convert.ToInt32(reader["ReleaseYear"]),
                        Convert.ToInt32(reader["Length"]),
                        Convert.ToDouble(reader["Rating"]),
                        Convert.ToString(reader["Genres"]),
                        Convert.ToString(reader["MainActors"]),
                        Convert.ToString(reader["Image"])
                    );
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching movie by ID: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }

            return movie;
        }

        public async Task InsertMovieAsync(Movie movie)
        {
            double rounded = Math.Round(movie.Arvio, 1);
            string Arvio = rounded.ToString().Replace(',', '.');

            string query = "INSERT INTO movies (Name, Director, ReleaseYear, Length, Rating, Genres, MainActors, Image) VALUES (@Name, @Director, @ReleaseYear, @Length, @Rating, @Genres, @MainActors, @Image)";

            await OpenConnectionAsync();

            MySqlCommand mySqlCommand = new MySqlCommand(query, _connection);
            mySqlCommand.Parameters.AddWithValue("@Name", movie.Nimi);
            mySqlCommand.Parameters.AddWithValue("@Director", movie.Ohjaaja);
            mySqlCommand.Parameters.AddWithValue("@ReleaseYear", movie.Julkaistu);
            mySqlCommand.Parameters.AddWithValue("@Length", movie.Pituus);
            mySqlCommand.Parameters.AddWithValue("@Rating", Arvio);
            mySqlCommand.Parameters.AddWithValue("@Genres", movie.Genre);
            mySqlCommand.Parameters.AddWithValue("@MainActors", movie.Päänäyttelijät);
            mySqlCommand.Parameters.AddWithValue("@Image", movie.Image);

            await mySqlCommand.ExecuteNonQueryAsync();
            CloseConnection();
        }

        public async Task DeleteMovieAsync(int id)
        {
            string query = "DELETE FROM moviedb.movies WHERE idMovies = @Id";

            await OpenConnectionAsync();

            MySqlCommand mySqlCommand = new MySqlCommand(query, _connection);
            mySqlCommand.Parameters.AddWithValue("@Id", id);

            await mySqlCommand.ExecuteNonQueryAsync();
            CloseConnection();
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            string query = "UPDATE movies SET Name = @Name, Length = @Length, ReleaseYear = @ReleaseYear, Genres = @Genres, MainActors = @MainActors, Director = @Director, Rating = @Rating, Image = @Image WHERE idMovies = @idMovies";

            await OpenConnectionAsync();

            MySqlCommand mySqlCommand = new MySqlCommand(query, _connection);
            mySqlCommand.Parameters.AddWithValue("@Name", movie.Nimi);
            mySqlCommand.Parameters.AddWithValue("@Length", movie.Pituus);
            mySqlCommand.Parameters.AddWithValue("@ReleaseYear", movie.Julkaistu);
            mySqlCommand.Parameters.AddWithValue("@Genres", movie.Genre);
            mySqlCommand.Parameters.AddWithValue("@MainActors", movie.Päänäyttelijät);
            mySqlCommand.Parameters.AddWithValue("@Director", movie.Ohjaaja);
            mySqlCommand.Parameters.AddWithValue("@Rating", movie.Arvio);
            mySqlCommand.Parameters.AddWithValue("@Image", movie.Image);
            mySqlCommand.Parameters.AddWithValue("@idMovies", movie.IdElokuvat);

            await mySqlCommand.ExecuteNonQueryAsync();
            CloseConnection();
        }

        public async Task<bool> CheckMovieExistenceAsync(Movie movie)
        {
            string query = "SELECT COUNT(*) FROM movies WHERE Name = @MovieName";

            await OpenConnectionAsync();

            MySqlCommand mySqlCommand = new MySqlCommand(query, _connection);
            mySqlCommand.Parameters.AddWithValue("@MovieName", movie.Nimi);

            long movieCount = (long)await mySqlCommand.ExecuteScalarAsync();

            CloseConnection();

            return movieCount > 0;
        }

        public async Task InsertUserAsync(User user)
        {
            string query = "INSERT INTO usertable (username, email, password) VALUES (@Name, @Email, @Password)";

            await OpenConnectionAsync();

            MySqlCommand mySqlCommand = new MySqlCommand(query, _connection);
            mySqlCommand.Parameters.AddWithValue("@Name", user.Name);
            mySqlCommand.Parameters.AddWithValue("@Email", user.Email);
            mySqlCommand.Parameters.AddWithValue("@Password", user.Password);

            await mySqlCommand.ExecuteNonQueryAsync();
            CloseConnection();
        }

        public async Task<bool> CheckUserExistenceAsync(User user)
        {
            string query = "SELECT COUNT(*) FROM usertable WHERE username = @username OR email = @Email";

            await OpenConnectionAsync();

            MySqlCommand mySqlCommand = new MySqlCommand(query, _connection);
            mySqlCommand.Parameters.AddWithValue("@username", user.Name);
            mySqlCommand.Parameters.AddWithValue("@Email", user.Email);

            long userCount = (long)await mySqlCommand.ExecuteScalarAsync();

            CloseConnection();

            return userCount > 0;
        }

        public async Task<bool> CheckUserPasswordAsync(User user)
        {
            string query = "SELECT password FROM usertable WHERE username = @username";

            await OpenConnectionAsync();

            MySqlCommand mySqlCommand = new MySqlCommand(query, _connection);
            mySqlCommand.Parameters.AddWithValue("@username", user.Name);

            string storedPassword = (string)await mySqlCommand.ExecuteScalarAsync();

            CloseConnection();

            return storedPassword == user.Password;
        }

        private async Task OpenConnectionAsync()
        {
            await _connection.OpenAsync();
        }

        private void CloseConnection()
        {
            _connection.Close();
        }
    }
}
