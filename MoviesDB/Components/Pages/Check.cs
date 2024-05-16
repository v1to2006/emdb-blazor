using MoviesDB.Models;
using System.Text.RegularExpressions;

namespace MoviesDB.Components
{
    public class Check
    {
        public bool CheckFields(Movie _movie)
        {
            bool fields;
            if (_movie.Nimi != string.Empty && _movie.Ohjaaja != string.Empty &&
               _movie.Julkaistu >= 1888 && _movie.Pituus > 0 && _movie.Genre != string.Empty &&
               _movie.Päänäyttelijät != string.Empty)
            {
                { fields = true; }
            }
            else
            {
                { fields = false; }
            }
            return fields;
        }

        public bool CheckLenghts(Movie _movie)
        {
            bool lenghts;
            if (_movie.Nimi.Length < 200 && _movie.Ohjaaja.Length < 100 && _movie.Genre.Length < 50 && _movie.Päänäyttelijät.Length < 50)
            { lenghts = true; }

            else
            { lenghts = false; }
            return lenghts;
        }
        public bool CheckPassword(User _user)
        {
            bool isOK = false;

            if (_user.Password.Length >= 6 &&
        _user.Password.Any(char.IsUpper) &&
        !_user.Password.Contains("!#?&%$€£@") &&
        _user.Password.Any(c => !char.IsLetterOrDigit(c)))
            {
                isOK = true;
            }
            return isOK;
        }

        public bool CheckEmail(User _user)
        {
            string regex = @"^[\w\.-]+@[\w\.-]+\.\w{2,}$";

            return Regex.IsMatch(_user.Email, regex, RegexOptions.IgnoreCase);
        }
    }
}