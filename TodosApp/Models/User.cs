using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodosApp.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        public User(string username, string password, string fullName)
        {
            Username = username;
            Password = password;
            FullName = fullName;
        }

        public static User? getUserByUsername(string username)
        {
            using (SqlCommand command = new SqlCommand("SELECT * FROM users WHERE username = @username", Db.DBConnection.GetInstance()))
            {
                command.Parameters.AddWithValue("@username", username);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User(username, reader["password"].ToString()!, reader["full_name"].ToString()!)
                        {
                            Id = Convert.ToInt32(reader["id"])
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static bool IsLoginValid(string username, string password)
        {
            using (SqlCommand command = new SqlCommand("SELECT * FROM users WHERE username = @username AND password = @password", Db.DBConnection.GetInstance()))
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public bool Save()
        {
            if (Id == null)
            {
                using (SqlCommand command = new SqlCommand("INSERT INTO users (username, password, full_name) VALUES (@username, @password, @full_name)" +
                    "SELECT SCOPE_IDENTITY()", Db.DBConnection.GetInstance()))
                {
                    command.Parameters.AddWithValue("@username", Username);
                    command.Parameters.AddWithValue("@password", Password);
                    command.Parameters.AddWithValue("@full_name", FullName);
                    try
                    {
                        int id = Convert.ToInt32(command.ExecuteScalar());
                        Id = id;
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            else
            {
                using (SqlCommand command = new SqlCommand("UPDATE users SET username = @username, password = @password, full_name = @full_name WHERE id = @id", Db.DBConnection.GetInstance()))
                {
                    command.Parameters.AddWithValue("@username", Username);
                    command.Parameters.AddWithValue("@password", Password);
                    command.Parameters.AddWithValue("@full_name", FullName);
                    command.Parameters.AddWithValue("@id", Id);

                    try 
                    {
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
        }

        public List<Task> getAllTasks(int isCompleted = -1)
        {
            if (Id == null)
            {
                return new List<Task>();
            } else
            {
                return Task.GetAllTasksOfUserId(Id.Value, isCompleted);
            }
        }

        public override string ToString()
        {
            return $"User: {Id} {FullName} ({Username})";
        }
    }
}
