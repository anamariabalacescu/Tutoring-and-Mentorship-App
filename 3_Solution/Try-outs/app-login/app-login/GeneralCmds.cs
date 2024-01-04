using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Markup;

namespace app_login
{
    public class GeneralCmds
    {
        TutoringDataContext tu = new TutoringDataContext();
        public string getUsername(int id)
        {
            var user = tu.Users.Where(u => u.ID_User == id).FirstOrDefault();
            return user.Username;
        }
        public string getUserType(int id)
        {
            var user = tu.Users.Where(u => u.ID_User == id).FirstOrDefault();
            return user.UserType;
        }
        public string getPassword(int id)
        {
            var user = tu.Users.Where(u => u.ID_User == id).FirstOrDefault();
            return user.UserPassword;
        }
        public string getEmail (int id)
        {
            var user = tu.Users.Where(u => u.ID_User == id).FirstOrDefault();
            return user.Email;
        }
        public string getSurname (int id)
        {
            string type = getUserType(id);
            string nume;
            if (type == "profesor")
            {
                var user = tu.Profesors.Where(p => p.ID_User == id).FirstOrDefault();
                nume = user.Nume;
            } else if (type == "student"){
                var user = tu.Students.Where(s => s.ID_User==id).FirstOrDefault();
                nume = user.Nume;
            } else
            {
                var user = tu.Admins.Where(a => a.ID_User == id).FirstOrDefault();
                nume = user.Nume;
            }
            return nume;
        }
        public string getFirstname(int id)
        {
            string type = getUserType(id);
            string prenume;
            if (type == "profesor")
            {
                var user = tu.Profesors.Where(p => p.ID_User == id).FirstOrDefault();
                prenume = user.Prenume;
            }
            else if (type == "student")
            {
                var user = tu.Students.Where(s => s.ID_User == id).FirstOrDefault();
                prenume = user.Prenume;
            }
            else
            {
                var user = tu.Admins.Where(a => a.ID_User == id).FirstOrDefault();
                prenume = user.Prenume;
            }
            return prenume;
        }
        public string getProfJob(int id)
        {
            var user = tu.Profesors.Where(p => p.ID_User== id).FirstOrDefault();
            return user.Profesie_de_baza;
        }
        public string getUniversity(int id)
        {
            var user = tu.Students.Where(s => s.ID_User == id).FirstOrDefault();
            return user.Universitate;
        }
        public bool userExists(string username)
        {
            TutoringDataContext tut = new TutoringDataContext();

            // Check if any user has the same username
            return tut.Users.Any(u => u.Username == username);
        }

    }
}
