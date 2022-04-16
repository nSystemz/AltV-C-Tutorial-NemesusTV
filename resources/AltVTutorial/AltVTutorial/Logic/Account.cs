using AltV.Net;
using AltVTutorial.Database;
using AltVTutorial.Database.Models;
using System;
using System.Linq;

namespace AltVTutorial.Logic
{
    class Account
    {
        private string _username;
        private ConnectionContext _db;

        /// <summary>
        /// Gets the last inserted id. (Default -1)
        /// </summary>
        public int LastInsertedId { get; private set; }

        /// <summary>
        /// Creates a new instance of the given user
        /// </summary>
        /// <param name="username">Username to work with</param>
        public Account(string username)
        {
            this._username = username;
            _db = new ConnectionContext();
            LastInsertedId = -1;

        }

        /// <summary>
        /// Checks if the given username exists. If no parameter is provided, the instance username will be used
        /// </summary>
        /// <param name="username">OPTIONAL: Username to check against the database</param>
        /// <returns>Retruns true if the given username exists</returns>
        public bool DoesUserExist(string username = "")
        {
            string userToCheck = (username == string.Empty) ? username : this._username;
            User result = this._db.User.Where(user => user.Username == userToCheck).FirstOrDefault();
            return result != null;
        }

        /// <summary>
        /// Create an account with the given password
        /// </summary>
        /// <param name="password">Plain text password, which will be hased and inserted into the database</param>
        /// <returns>Returns true if the registration was successfull</returns>
        public bool Register(string password)
        {
            string saltedPassword = BCrypt.HashPassword(password, BCrypt.GenerateSalt());
            User newUserAccount = new User();
            newUserAccount.Username = _username;
            newUserAccount.Password = saltedPassword;
            this._db.Add(newUserAccount);
            try
            {
                this._db.SaveChanges();
                LastInsertedId = newUserAccount.Id;
                return true;
            }
            catch (Exception ex)
            {
                Alt.LogError(ex.Message);
            }
            return false;
        }
    }
}
