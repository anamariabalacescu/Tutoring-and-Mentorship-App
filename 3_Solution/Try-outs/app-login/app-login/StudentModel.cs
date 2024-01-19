using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_login
{
    public class StudentModel
    {
        public int id { get; set; }
        public string nume { get; set; }
        public string prenume { get; set; }
        public string universitate { get; set; }
        public StudentModel() { }
        public StudentModel(int id_std, string sname, string fname, string uni)
        {
            id = id_std;
            nume = sname;
            prenume = fname;
            universitate = uni;
        }

        private Student toStudent() => new Student() { Nume = this.nume, Prenume = this.prenume, Universitate = this.universitate, ID_User = id };

        public int InsertStudent(int id, string sname, string fname, string univ)
        {
            using (TutoringEntities tut = new TutoringEntities())
            {
                // Assuming you have an EncryptionMachine class for password encryption
                // string encrPass = EncryptionMachine.Encrypt(pass);
                StudentModel newStudent = new StudentModel(id, sname, fname, univ);

                try
                {
                    // Insert the newStudentModel into the Students table
                    tut.Students.Add(newStudent.toStudent());
                    int changes = tut.SaveChanges();

                    // Check if any entities were inserted
                    if (changes > 0)
                    {
                        return 1; // Successfully inserted the new student
                    }
                    else
                    {
                        return 0; // No entities were inserted
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
