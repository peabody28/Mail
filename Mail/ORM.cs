using MySql.Data.MySqlClient;
using System;
using Mail.Models;
using System.Data.Common;

namespace Mail
{
    public static class ORM
    {
        public static MySqlConnection conn;

        static ORM()
        {
            string host = "127.0.0.1";
            int port = 3306;
            string database = "mail";
            string username = "root";
            string password = "1234";

            // Connection String.
            String connString = "Server=" + host + ";Database=" + database
                + ";port=" + port + ";User Id=" + username + ";password=" + password;

            MySqlConnection conn = new MySqlConnection(connString);

            conn.Open();

            ORM.conn = conn;
        }

        public static User SearchUser(string email)
        {
            MySqlCommand cmd = conn.CreateCommand();

            // Set Command Text
            cmd.CommandText = $"SELECT * FROM users WHERE email = '{MySqlHelper.EscapeString(email)}'";

            User tmp = new();
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    tmp.Id = reader.GetInt32(0);
                    tmp.Email = reader.GetString(1);
                    tmp.Password = reader.GetString(2);
                }
                else
                    return null;
            }

            return tmp;
        }

        public static Int32 CreateUser(User user)
        {
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"INSERT INTO users(email, password) VALUES('{user.Email}', '{user.Password}')";
            int id = cmd.ExecuteNonQuery();

            return id;
        }

        public static Int32 CreateMail(MailModel model)
        {
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"INSERT INTO mails(author, target, text) VALUES('{model.AuthorId}','{model.TargetId}','{model.Text}')";
            int id = cmd.ExecuteNonQuery();

            return id;
        }
    }
}
