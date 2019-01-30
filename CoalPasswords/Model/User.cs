using System;
using System.Collections.Generic;

namespace CoalPasswords
{
    /// <summary>
    /// Зарегистрированный пользователь
    /// </summary>
    [Serializable]
    class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullNameString { get => Name + " " + LastName; private set { FullNameString = value; } }
        public string Login { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// Коллекция паролей пользователя
        /// </summary>
        public IRepository<IRecord> PasswordRecords { get; set; }
        public User(string login, string password)
        {
            Login = login;
            Password = password;
            PasswordRecords = new Repository<IRecord>();
            Name = "Bill";
            LastName = "Gates";
        }
        public User(string login, string password, IList<IRecord> repository)
        {
            Login = login;
            Password = password;
            PasswordRecords = new Repository<IRecord>(repository);
        }
    }
}
