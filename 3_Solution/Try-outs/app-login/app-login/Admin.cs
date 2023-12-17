using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_login
{
    internal class Admin
    {
        public string nume { get; set; }
        public string prenume { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }

        public Admin() { }
        public Admin(string sname, string fname, string userName, string userPass, string mail)
        {
            nume = sname;
            prenume = fname;
            username = userName;
            password = userPass;
            email = mail;
        }
        public int insertAdmin(string sname, string fname, string user, string pass, string mail, SqlConnection conn)
        {
            SqlCommand insertCmd = conn.CreateCommand();
            string encrPass = EncryptionMachine.Encrypt(pass);
            insertCmd.CommandText = $"INSERT INTO Admins (Nume, Prenume, Username, AdminPassword, Email) VALUES ('{sname}','{fname}','{user}','{encrPass}','{mail}');";
            //insertCmd.Parameters.AddWithValue("@Username", username.Text);
            //insertCmd.Parameters.AddWithValue("@Password", password.Text);
            //insertCmd.Parameters.AddWithValue("@Nume", surname.Text);
            //insertCmd.Parameters.AddWithValue("@Prenume", firstname.Text);.
            int rowsAffected = insertCmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                return 1;
                // Successfully inserted the new user
                //Console.WriteLine("User inserted successfully");
            }
            else
            {
                return 0;
                // Failed to insert user
                //Console.WriteLine("Failed to insert user");
            }
        }
    }
}