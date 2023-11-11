using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_login
{
    internal class Student
    {
        public string nume { get; set; }
        public string prenume { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string universitate { get; set; }
        public string email { get; set; }
        public Student() { }
        public Student(string sname, string fname, string uni, string userName, string userPass, string mail)
        {
            nume = sname;
            prenume = fname;
            universitate = uni;
            username = userName;
            password = userPass;
            email = mail;
        }
        public int insertStudent(string sname, string fname, string univ, string user, string pass, string email, SqlConnection conn)
        {
            SqlCommand insertCmd = conn.CreateCommand();
            string encrPass = EncryptionMachine.Encrypt(pass);
            insertCmd.CommandText = $"INSERT INTO Students (Nume, Prenume, Universitate, Username, StdPassword, Email) VALUES ('{sname}','{fname}','{univ}','{user}','{encrPass}','{email}');";
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
