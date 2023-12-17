using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_login
{
    internal class ProfessorModel
    {
        public int id {  get; set; }
        public string nume { get; set; }
        public string prenume { get; set; }
        public string profesie { get; set; }

        public ProfessorModel() { }
        public ProfessorModel(int id_prof, string sname, string fname, string prof)
        {
            id = id_prof;
            nume = sname;
            prenume = fname;
            profesie = prof;
        }
        private Profesor toProfesor() => new Profesor() { ID_User = this.id, Nume = this.nume, Prenume = this.prenume, Profesie_de_baza = this.profesie };
        public int InsertProfesor(int id_prof, string sname, string fname, string prof)
        {
            using (TutoringDataContext tut = new TutoringDataContext())
            {
                try
                {
                    ProfessorModel newProfesor = new ProfessorModel
                    {
                        id = id_prof,
                        nume = sname,
                        prenume = fname,
                        profesie = prof
                    };

                    // Insert the newProfesor into the Profesors table
                    tut.Profesors.InsertOnSubmit(newProfesor.toProfesor());
                    tut.SubmitChanges();

                    // Check if the insertion was successful
                    ChangeSet changes = tut.GetChangeSet();

                    // Check if any entities were inserted
                    if (changes.Inserts.Count > 0)
                    {
                        return 1; // Successfully inserted the new professor
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
                    er.ErrorMessage = "Cannot insert into table.\n";
                    return 0; // Return 0 to indicate failure
                }
            }
        }
    }
}
