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
                    User tmp = new User((long)reader["UniqueId"], reader["Login"] as string, reader["Password"] as string);
                    tmp.Name = reader["Name"] as string;
                    tmp.LastName = reader["LastName"] as string;
                    tmp.Theme = reader["Theme"] as string;
                    tmp.Language = reader["Language"] as string;
                    tmp.Image = reader["Image"] as string;
                    tmp.PasswordRecords = GetRecords($"SELECT * FROM PasswordRecords WHERE UserId = {reader["UniqueId"]}");
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
                        Category = (int)reader.GetInt64(7),
                        UniqueId = reader.GetInt32(8)
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
            try
            {
                string query = $"INSERT INTO PasswordRecords(UserId, Title, Username, Email, Password, Website, Category, UniqueId) VALUES ('{current.Id}','{record.Title}','{record.Username}','{record.Email}','{record.Password}','{record.Website}','{record.Category}', '{record.UniqueId}')";
                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DataSource}"))
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    command.ExecuteNonQuery();
                }
                return true;
            } catch(Exception) { return false; }
        }

        public bool UpdateUserTheme(User current)
        {
            try
            {
                string query = $"UPDATE Users SET Theme = '{current.Theme}' WHERE UniqueId = {current.Id}";
                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DataSource}"))
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    command.ExecuteNonQuery();
                }
                return true;
            } catch(Exception) { return false; }
        }

        public bool UpdateUserLanguage(User current)
        {
            try
            {
                string query = $"UPDATE Users SET Language = '{current.Theme}' WHERE UniqueId = {current.Id}";
                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DataSource}"))
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool UpdatePasswordRecord(IRecord record)
        {
            try
            {
                string query = $"UPDATE PasswordRecords SET Title = '{record.Title}', Username = '{record.Username}', Email = '{record.Email}', Password = '{record.Password}', Website = '{record.Website}', Category = '{record.Category}' WHERE UniqueId = {record.UniqueId}";
                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DataSource}"))
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    command.ExecuteNonQuery();
                }
                return true;
            } catch(Exception) { return false; }
        }

        public bool AddUser(User user)
        {
            try
            {
                string query = $"INSERT INTO Users(Name, LastName, Login, Password, Theme, Language, Image, UniqueId) VALUES ('{user.Name}','{user.LastName}','{user.Login}','{user.Password}','{user.Theme}','{user.Language}','{user.Image}','{user.Id}')";
                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DataSource}"))
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    command.ExecuteNonQuery();
                }
                return true;
            } catch (Exception) { return false; }
        }

        public bool RemoveRecord(IRecord record, User current)
        {
            string query = $"DELETE FROM PasswordRecords WHERE UniqueId='{record.UniqueId}'";
            using(SQLiteConnection connection = new SQLiteConnection($"Data Source={DataSource}"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
            }
            return true;
        }
    }
}
