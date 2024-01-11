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
        private string status { get; set; }
        public UserModel(string user, string pass, string mail, string type)
        {
            this.userName = user;
            this.passWord = pass;
            this.email = mail;
            this.type = type;
            this.status = "active";
        }
        public UserModel(int userId)
        {
            TutoringDataContext tut = new TutoringDataContext();

            // Find the user by ID
            User existingUser = tut.Users.SingleOrDefault(u => u.ID_User == userId);

            // Return the user or null if not found
            this.userName = existingUser.Username;
            this.passWord = existingUser.UserPassword;
            this.email = existingUser.Email;
            this.type = existingUser.UserType;
        }
        private User toUser() => new User() { Username = userName, UserPassword = passWord, Email = email, UserType = type, UserStatus = status };
        public int UserInsert(string userName, string pass, string mail)
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
        private bool VerifyOldPassword(int userId, string inputOldPassword)
        {
            TutoringDataContext tut = new TutoringDataContext();

            // Find the user by ID
            User existingUser = tut.Users.SingleOrDefault(u => u.ID_User == userId);

            if (existingUser != null)
            {
                // Encrypt the provided old password
                string encryptedInputOldPassword = EncryptionMachine.Encrypt(inputOldPassword);

                // Compare the encrypted old password with the stored password
                return existingUser.UserPassword == encryptedInputOldPassword;
            }

            // Handle the case where the user with the provided ID was not found
            return false;
        }

        public void UpdateUserPassword(int userId, string oldPassword, string newPassword)
        {
            TutoringDataContext tut = new TutoringDataContext();

            // Verify the old password
            if (VerifyOldPassword(userId, oldPassword))
            {
                // Find the user by ID
                User existingUser = tut.Users.SingleOrDefault(u => u.ID_User == userId);

                if (existingUser != null)
                {
                    // Update the password
                    existingUser.UserPassword = EncryptionMachine.Encrypt(newPassword);
                    try
                    {
                        // Submit changes to the database
                        tut.SubmitChanges();

                        // If no exception occurred, changes were successfully submitted
                        Done d = new Done();
                        d.SuccessMessage = "Password changed successfully!";
                        d.Show();
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions if any occur during the submission
                        Error er = new Error();
                        er.ErrorMessage = "Error submitting password changes";
                    }
                }
                // Handle the case where the user with the provided ID was not found
            }

        }
        private bool IsValidEmail(int userId, string newEmail)
        {
            TutoringDataContext tut = new TutoringDataContext();

            // Find the user by ID
            User existingUser = tut.Users.SingleOrDefault(u => u.ID_User == userId);

            if (existingUser != null)
            {
                // Verify the email for the given user ID
                return existingUser.Email == newEmail;
            }
            return false;
        }
        public void ChangeEmail(int userId, string oldEmail, string newEmail, string password)
        {
            TutoringDataContext tut = new TutoringDataContext();

            // Find the user by ID
            User existingUser = tut.Users.SingleOrDefault(u => u.ID_User == userId);

            if (existingUser != null)
            {
                // Verify the old email and password
                if (VerifyOldPassword(userId, password))
                {
                    // Check the validity of the new email
                    if (IsValidEmail(userId, oldEmail))
                    {
                        // Update the email
                        if (FormValidationRules.IsValidEmail(newEmail))
                        {
                            existingUser.Email = newEmail;
                            try
                            {
                                // Submit changes to the database
                                tut.SubmitChanges();

                                // If no exception occurred, changes were successfully submitted
                                Done d = new Done();
                                d.SuccessMessage = "Email changed successfully!";
                                d.Show();
                            }
                            catch (Exception ex)
                            {
                                // Handle exceptions if any occur during the submission
                                Error er = new Error();
                                er.ErrorMessage = "Error submitting email changes";
                                er.Show();
                            }
                        }
                        else
                        {
                            // Handle the case where the new email is not valid
                            Error er = new Error();
                            er.ErrorMessage = "New email is not valid";
                            er.Show();
                        }
                    }
                    else
                    {
                        // Handle the case where the old email is not valid
                        Error er  = new Error();
                        er.ErrorMessage = "Current email doesn't match";
                        er.Show();
                    }
                }
                else
                {
                    // Handle the case where the password is incorrect
                    Error er  = new Error();
                    er.ErrorMessage = "Wrong password!";
                    er.Show();
                }
            }
            else
            {
                // Handle the case where the user with the provided ID was not found
                throw new ArgumentException("User not found.");
            }
        }
        public void UpdateUserStatus(int userId, string status)
        {
            TutoringDataContext tut = new TutoringDataContext();

        // Find the user by ID
            User existingUser = tut.Users.SingleOrDefault(u => u.ID_User == userId);

            if (existingUser != null)
            {
                // Update the password
                existingUser.UserStatus = status;
                try
                {
                    // Submit changes to the database
                    tut.SubmitChanges();

                    // If no exception occurred, changes were successfully submitted
                    Done d = new Done();
                    d.SuccessMessage = "Password changed successfully!";
                    d.Show();
                }
                catch (Exception ex)
                {
                    // Handle exceptions if any occur during the submission
                    Error er = new Error();
                    er.ErrorMessage = "Error submitting password changes";
                }
            }
            // Handle the case where the user with the provided ID was not found

        }
        public void UpdateUsername(int userId, string oldUsername, string newUsername, string password)
        {
            TutoringDataContext tut = new TutoringDataContext();

            // Find the user by ID
            User existingUser = tut.Users.SingleOrDefault(u => u.ID_User == userId);

            if (existingUser != null)
            {
                if (oldUsername != newUsername)
                {
                    // Verify the old username and password
                    if (VerifyOldPassword(userId, password))
                    {
                        GeneralCmds gen = new GeneralCmds();
                        // Check the validity of the new username
                        if (gen.userExists(newUsername) == false)
                        {
                            // Update the username
                            existingUser.Username = newUsername;

                            // Submit changes to the database
                            tut.SubmitChanges();

                            // If no exception occurred, changes were successfully submitted
                            try
                            {
                                // Submit changes to the database
                                tut.SubmitChanges();

                                // If no exception occurred, changes were successfully submitted
                                Done d = new Done();
                                d.SuccessMessage = "Username changed successfully!";
                                d.Show();
                            }
                            catch (Exception ex)
                            {
                                // Handle exceptions if any occur during the submission
                                Error er = new Error();
                                er.ErrorMessage = "Error submitting username changes";
                                er.Show();
                            }
                        }
                        else
                        {
                            // Handle the case where the new username already exists
                            Error er = new Error();
                            er.ErrorMessage = "Username is taken";
                            er.Show();
                        }
                    }
                    else
                    {
                        // Handle the case where the old username or password is incorrect
                        Error er = new Error();
                        er.ErrorMessage = "Password is not valid";
                        er.Show();
                    }
                }
                else
                {
                    Error er = new Error();
                    er.ErrorMessage = "Username is the same";
                    er.Show();
                }
            }
            else
            {
                // Handle the case where the user with the provided ID was not found
                Error er = new Error();
                er.ErrorMessage = "User not found";
                er.Show();
            }
        }
    }
}
