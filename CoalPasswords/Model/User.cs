using System.Collections.Generic;

namespace CoalPasswords
{
    /// <summary>
    /// Зарегистрированный пользователь
    /// </summary>
    class User
    {
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
        }
        public User(string login, string password, IList<IRecord> repository)
        {
            Login = login;
            Password = password;
            PasswordRecords = new Repository<IRecord>(repository);
        }
    }
}
