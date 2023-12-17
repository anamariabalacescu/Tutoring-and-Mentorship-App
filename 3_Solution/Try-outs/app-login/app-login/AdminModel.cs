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

        public int insertAdmin(string sname, string fname, int id)
        {
            using (TutoringDataContext tut = new TutoringDataContext())
            {
                // Assuming you have an EncryptionMachine class for password encryption
                // string encrPass = EncryptionMachine.Encrypt(pass);
                AdminModel newAdmin = new AdminModel(sname, fname, id);

                try
                {
                    // Insert the newStudentModel into the StudentModels table
                    tut.Admins.InsertOnSubmit(newAdmin.toAdmin());
                    tut.SubmitChanges();

                    // Check if the insertion was successful
                    if (newAdmin.toAdmin().ID_Admin > 0)
                    {
                        return 1; // Successfully inserted the new student
                    }
                    else
                    {
                        return 0; // Failed to insert student
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