using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_login
{
    public class UserModel
    {
        private string userName { get; set; }
        private string passWord { get; set; }
        private string type { get; set; }
        private string email { get; set; }
        public UserModel(string user, string pass, string mail, string type)
        {
            this.userName = user;
            this.passWord = pass;
            this.email = mail;
            this.type = type;
        }

        private User toUser() => new User() { Username = userName, UserPassword = passWord, Email = email, UserType = type };
        public int UserInsert (string userName, string pass, string mail)
        {
            TutoringDataContext tut = new TutoringDataContext();
            string encrpass = EncryptionMachine.Encrypt(pass);

            UserModel newUser = new UserModel(userName, encrpass, mail, this.type);

            tut.Users.InsertOnSubmit(newUser.toUser());
            tut.SubmitChanges();

            // After submission, the newUser object will have the generated ID_User
            int id_user = tut.Users.Where(u => u.Username == userName).Select(u => u.ID_User).FirstOrDefault();

            return id_user;
        }
    }
}
