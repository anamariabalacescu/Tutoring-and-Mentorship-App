using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_login
{
    public class AdminModel
    {
        public string nume { get; set; }
        public string prenume { get; set; }
        public int iduser { get; set; }

        public AdminModel() { }
        public AdminModel(string sname, string fname, int id)
        {
            nume = sname;
            prenume = fname;
            iduser = id;
        }

        private Admin toAdmin() => new Admin() { Nume = this.nume, Prenume = this.prenume, ID_User = this.iduser };

        public int InsertAdmin(string sname, string fname, int id)
        {
            using (var tut = new TutoringEntities())
            {
                AdminModel newAdmin = new AdminModel(sname, fname, id);

                try
                {
                    tut.Admins.Add(newAdmin.toAdmin()); // Assuming ToAdmin() returns an Admin instance
                    tut.SaveChanges();

                    // Check if the insertion was successful
                    if (newAdmin.toAdmin().ID_Admin > 0)
                    {
                        return 1; // Successfully inserted the new admin
                    }
                    else
                    {
                        return 0; // Failed to insert admin
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, log, or throw as needed
                    Error er = new Error();
                    er.ErrorMessage = ex.Message;
                    er.Show();
                    return 0; // Return 0 to indicate failure
                }
            }
        }

    }
}