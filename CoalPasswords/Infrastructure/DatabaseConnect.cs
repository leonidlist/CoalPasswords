using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CoalPasswords
{
    class DatabaseConnect
    {
        public string DataSource { get; set; }
        public DatabaseConnect(string dataSource)
        {
            DataSource = dataSource;
        }
        public IRepository<User> GetUsers(string query = @"SELECT * FROM Users")
        {
            Repository<User> data = new Repository<User>();
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DataSource}"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    User tmp = new User(reader["Login"] as string, reader["Password"] as string);
                    tmp.Id = (long)reader["Id"];
                    tmp.Name = reader["Name"] as string;
                    tmp.LastName = reader["LastName"] as string;
                    tmp.PasswordRecords = GetRecords($"SELECT * FROM PasswordRecords WHERE UserId = {reader["Id"]}");
                    data.Add(tmp);
                }
            }
            return data;
        }
        private IRepository<IRecord> GetRecords(string query)
        {
            Random rnd = new Random();
            Repository<IRecord> repository = new Repository<IRecord>();
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DataSource}"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PasswordRecord tmp = new PasswordRecord
                    {
                        Title = reader["Title"].ToString(),
                        Username = reader["Username"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                        Website = reader["Website"].ToString(),
                        Category = reader["Category"].ToString()
                    };
                    tmp.CardColor = Pallete.Colors[rnd.Next(Pallete.Colors.Count)];
                    tmp.ImageUrl = $"http://{tmp.Website}/favicon.ico";
                    repository.Add(tmp);
                }
            }
            return repository;
        }

        public bool AddPasswordRecord(IRecord record, User current)
        {
            string query = $"INSERT INTO PasswordRecords(UserId, Title, Username, Email, Password, Website, Category) VALUES ('{current.Id}','{record.Title}','{record.Username}','{record.Email}','{record.Password}','{record.Website}','{record.Category}')";
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DataSource}"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
            }
            return true;
        }

        public bool AddUser(string name, string lastname, string login, string password)
        {
            string query = $"INSERT INTO Users(Name, LastName, Login, Password) VALUES ('{name}','{lastname}','{login}','{password}')";
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DataSource}"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
            }
            return true;
        }
    }
}
