using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace app_login
{
    internal class FormValidationRules
    {
        public static bool IsValidEmail(string email)
        {
            // Define a regular expression for a basic email validation
            string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

            // Check if the email matches the pattern
            return Regex.IsMatch(email, emailPattern);
        }
        public static bool IsValidPassword(string password)
        {
            // Min len() = 8 chs
            if (password.Length < 8)
                return false;

            // 1 uppercase 
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return false;

            // 1 lowercase 
            if (!Regex.IsMatch(password, @"[a-z]"))
                return false;

            // 1 digit
            if (!Regex.IsMatch(password, @"\d"))
                return false;

            return true;
        }
        public static bool IsValidUser(string user, string pass)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
                return false;

            using (TutoringDataContext tut = new TutoringDataContext())
            {
                // Check if there is a user with the provided username and password
                bool isValidUser = tut.Users.Any(u => u.Username == user && u.UserPassword == pass);

                return isValidUser;
            }
        }
        public static bool IsValidUsername(string username)
        {
            TutoringDataContext tut = new TutoringDataContext();

            var count = tut.Users.Count(s => s.Username == username);

            return count > 0;
        }
    }
}
