using System;
using System.Collections.Generic;

namespace CoalPasswords
{
    class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullNameString { get => Name + " " + LastName; private set { FullNameString = value; } }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Theme { get; set; }
        public string Language { get; set; }
        public string Image { get; set; }
        public IRepository<IRecord> PasswordRecords { get; set; }
        public User(long id, string login, string password)
        {
            Id = id;
            Login = login;
            Password = password;
            PasswordRecords = new Repository<IRecord>();
        }
        public User(long id, string login, string password, IList<IRecord> repository)
        {
            Id = id;
            Login = login;
            Password = password;
            PasswordRecords = new Repository<IRecord>(repository);
        }
    }
}
