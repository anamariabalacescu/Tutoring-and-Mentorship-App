using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_login
{
    internal class Professor
    {
        public string nume { get; set; }
        public string prenume { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string profesie { get; set; }
        public string email { get; set; }

        public Professor() { }
        public Professor(string sname, string fname, string prof, string userName, string userPass, string mail)
        {
            nume = sname;
            prenume = fname;
            profesie = prof;
            username = userName;
            password = userPass;
            email = mail;
        }
        public int insertProfesor(string sname, string fname, string prof, string user, string pass, string mail, SqlConnection conn)
        {
            SqlCommand insertCmd = conn.CreateCommand();
            string encrPass = EncryptionMachine.Encrypt(pass);
            insertCmd.CommandText = $"INSERT INTO Profesors (Nume, Prenume, Profesie_de_baza, Username, ProfPassword, Email) VALUES ('{sname}','{fname}','{prof}','{user}','{encrPass}','{mail}');";
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
