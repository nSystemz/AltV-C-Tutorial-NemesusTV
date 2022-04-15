using AltV.Net;
using MySql.Data.MySqlClient;
using System;

namespace AltVTutorial.Logic
{
    class Account
    {
        private string _username;
        private MySqlConnection _connection; // ToDo: get 
        private MySqlCommand _command;

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
            _connection = Datenbank.Connection;
            _command = _connection.CreateCommand();
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
            _command.CommandText = "SELECT ID FROM users WHERE name=@name LIMIT 1";
            _command.Parameters.AddWithValue("@name", userToCheck);
            try
            {
                MySqlDataReader reader = _command.ExecuteReader();
                if (reader.HasRows)
                    return true;
            }
            catch (Exception ex)
            {
                Alt.LogError(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Create an account with the given password
        /// </summary>
        /// <param name="password">Plain text password, which will be hased and inserted into the database</param>
        /// <returns>Returns true if the registration was successfull</returns>
        public bool Register(string password)
        {
            string saltedPassword = BCrypt.HashPassword(password, BCrypt.GenerateSalt());
            try
            {
                MySqlCommand command = _connection.CreateCommand();
                command.CommandText = "INSERT INTO users (password, name) VALUES (@password, @name)";

                command.Parameters.AddWithValue("@password", saltedPassword);
                command.Parameters.AddWithValue("@name", _username);
                command.ExecuteNonQuery();
                LastInsertedId = Convert.ToInt32(command.LastInsertedId);
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
