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
            string password = "12345678";

            // Connection String.
            String connString = "Server=" + host + ";Database=" + database
                + ";port=" + port + ";User Id=" + username + ";password=" + password;

            MySqlConnection conn = new MySqlConnection(connString);

            conn.Open();

            ORM.conn = conn;
        }

        public static User Search(string email)
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

        public static Int32 Create(User user)
        {
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"INSERT INTO users(email, password) VALUES('{user.Email}', '{user.Password}')";
            int id = cmd.ExecuteNonQuery();

            return id;
        }
    }
}
