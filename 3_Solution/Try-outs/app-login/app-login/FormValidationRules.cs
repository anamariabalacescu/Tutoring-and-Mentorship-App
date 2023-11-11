using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public static bool IsValidUsername(string username, SqlConnection conn)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Students WHERE Username = @Username";
            cmd.Parameters.AddWithValue("@Username", username);

            int count = (int)cmd.ExecuteScalar();
            
            return count > 0;
        }
    }
}
