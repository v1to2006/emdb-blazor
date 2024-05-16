using MoviesDB.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Collections.Generic;
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
            _user = "app";
            _password = "databaseApp!";
            _port = 3306;

            string connectionQuery = $"Server={_host};Port={_port};Database={_database};Uid={_user};Pwd={_password};";

            _connection = new MySqlConnection(connectionQuery);
        }

        public List<Movie> GetAllMovies()
        {
            List<Movie> movies = new List<Movie>();

            string query = "SELECT * FROM movies;";

            OpenConnection();
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


        public void InsertMovie(Movie movie)
        {
            double rounded = Math.Round(movie.Arvio, 1);
            string Arvio = rounded.ToString().Replace(',','.');

            string query = $"INSERT INTO movies (Name, Director, ReleaseYear, Length, Rating, Genres, MainActors, Image) VALUES (" +
                $"\"{movie.Nimi}\"," +
                $"\"{movie.Ohjaaja}\"," +
                $"{movie.Julkaistu}," +
                $"{movie.Pituus}," +
                $"\"{Arvio}\"," +
                $"\"{movie.Genre}\"," +
                $"\"{movie.Päänäyttelijät}\"," +
                $"\"{movie.Image}\"" +
                $");";

            OpenConnection();

            MySqlCommand mySqlCommand = new MySqlCommand(query, _connection);
            mySqlCommand.ExecuteNonQuery();
            CloseConnection();
        }

        public void EditMovie(Movie movie)
        {
            string query = $"UPDATE movies SET Name = '{movie.Nimi}', Length = {movie.Pituus}, ReleaseYear = {movie.Julkaistu}, Genres = '{movie.Genre}', MainActors = '{movie.Päänäyttelijät}', Director = '{movie.Ohjaaja}', Rating = {movie.Arvio}, Image = '{movie.Image}' WHERE idMovies = {movie.IdElokuvat}";

            OpenConnection();

            MySqlCommand mySqlCommand = new MySqlCommand(query, _connection);
            mySqlCommand.ExecuteNonQuery();
            CloseConnection();
        }

        public bool CheckMovies(Movie movie)
        {
            string query = "SELECT * FROM movies WHERE Name = @MovieName";

            OpenConnection();

            MySqlCommand mySqlCommand = new MySqlCommand(query, _connection);
            mySqlCommand.Parameters.AddWithValue("@MovieName", movie.Nimi);

            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            bool movieExists = mySqlDataReader.HasRows;

            mySqlDataReader.Close();
            CloseConnection();

            return movieExists;
        }
        public void InsertUser(User user)
        {
                string query = $"INSERT INTO usertable (username, email, password) VALUES (" +
                $"\"{user.Name}\"," +
                $"\"{user.Email}\"," +
                $"\"{user.Password}\"" +
                $");";

                OpenConnection();

                MySqlCommand mySqlCommand = new MySqlCommand(query, _connection);
                mySqlCommand.ExecuteNonQuery();
                CloseConnection();
        }

        public bool CheckUserExistence(User user)
        {
            bool userExists;
            long userWow;
            using (MySqlCommand sqlCommand = new MySqlCommand("SELECT COUNT(*) from usertable where username like @username OR email like @email", _connection))
            {
                _connection.Open();
                sqlCommand.Parameters.AddWithValue("@username", user.Name);
                sqlCommand.Parameters.AddWithValue("@email", user.Email);
                long userCount = (long)sqlCommand.ExecuteScalar();
                userWow = userCount;
                CloseConnection();
            }

            if (userWow == 0)
            {
                userExists = false;
            }
            else
            {
                userExists = true;
            }

            return userExists;
        }

        public bool CheckUserPassword(User user)
        {
            string username = user.Name;
            bool passCorr;

            using (MySqlCommand sqlCommand = new MySqlCommand("SELECT usertable.password from usertable WHERE username='"+username+"'", _connection))
            {
                _connection.Open();
                
                string pass = (string)sqlCommand.ExecuteScalar();
                
                CloseConnection();

                if (pass == user.Password)
                {
                    passCorr = true;
                }

                else
                {
                    passCorr = false;
                }

            }

            return passCorr;

        }

        private void OpenConnection()
        {
            _connection.Open();
        }

        private void CloseConnection()
        {
            _connection.Close();
        }
    }
}