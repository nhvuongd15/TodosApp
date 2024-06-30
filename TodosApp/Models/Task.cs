using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodosApp.Models
{
    public class Task
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = null;
        public DateTime? DueDate { get; set; } = null;
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedDate { get; set; } = null;

        public Task(int userId, string title, string? description = null, DateTime? dueDate = null)
        {
            UserId = userId;
            Title = title;
            Description = description;
            DueDate = dueDate;
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
            CompletedDate = DateTime.Now;
        }

        public void MarkAsUncompleted()
        {
            IsCompleted = false;
            CompletedDate = null;
        }

        public bool Save()
        {
            if (Id == null)
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO tasks (user_id, title, description, due_date, completed, completed_date) VALUES (@user_id, @title, @description, @due_date, @completed, @completed_date) SELECT SCOPE_IDENTITY()", Db.DBConnection.GetInstance()))
                {
                    cmd.Parameters.AddWithValue("@user_id", UserId);
                    cmd.Parameters.AddWithValue("@title", Title);
                    if (Description != null)
                    {
                        cmd.Parameters.AddWithValue("@description", Description);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@description", DBNull.Value);
                    }
                    if (DueDate != null)
                    {
                        cmd.Parameters.AddWithValue("@due_date", DueDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@due_date", DBNull.Value);
                    }
                    cmd.Parameters.AddWithValue("@completed", IsCompleted);
                    cmd.Parameters.AddWithValue("@completed_date", DBNull.Value);

                    try
                    {
                        int id = Convert.ToInt32(cmd.ExecuteScalar());
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
                using (SqlCommand cmd = new SqlCommand("UPDATE tasks SET user_id = @user_id, title = @title, description = @description, due_date = @due_date, completed = @completed, completed_date = @completed_date WHERE id = @id", Db.DBConnection.GetInstance()))
                {
                    cmd.Parameters.AddWithValue("@user_id", UserId);
                    cmd.Parameters.AddWithValue("@title", Title);
                    if (Description != null)
                    {
                        cmd.Parameters.AddWithValue("@description", Description);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@description", DBNull.Value);
                    }
                    if (DueDate != null)
                    {
                        cmd.Parameters.AddWithValue("@due_date", DueDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@due_date", DBNull.Value);
                    }
                    cmd.Parameters.AddWithValue("@completed", IsCompleted);
                    if (CompletedDate != null)
                    {
                        cmd.Parameters.AddWithValue("@completed_date", CompletedDate);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@completed_date", DBNull.Value);
                    }
                    cmd.Parameters.AddWithValue("@id", Id);

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
        }

        public static List<Task> GetAllTasksOfUserId(int userId, int isCompleted = -1)
        {
            if (isCompleted == -1)
            {
                List<Task> tasks = new List<Task>();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE user_id = @user_id", Db.DBConnection.GetInstance()))
                {
                    cmd.Parameters.AddWithValue("@user_id", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task t = new Task(userId, reader["title"].ToString()!)
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Description = reader["description"] == DBNull.Value ? null : reader["description"].ToString(),
                                DueDate = reader["due_date"] == DBNull.Value ? null : (DateTime?)reader["due_date"],
                                IsCompleted = Convert.ToBoolean(reader["completed"]),
                                CompletedDate = reader["completed_date"] == DBNull.Value ? null : (DateTime?)reader["completed_date"]
                            };
                            tasks.Add(t);
                        }
                    }
                }
                return tasks;
            } else
            {
                List<Task> tasks = new List<Task>();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE user_id = @user_id AND completed = @completed", Db.DBConnection.GetInstance()))
                {
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    cmd.Parameters.AddWithValue("@completed", isCompleted);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task t = new Task(userId, reader["title"].ToString()!)
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Description = reader["description"] == DBNull.Value ? null : reader["description"].ToString(),
                                DueDate = reader["due_date"] == DBNull.Value ? null : (DateTime?)reader["due_date"],
                                IsCompleted = Convert.ToBoolean(reader["completed"]),
                                CompletedDate = reader["completed_date"] == DBNull.Value ? null : (DateTime?)reader["completed_date"]
                            };
                            tasks.Add(t);
                        }
                    }
                }
                return tasks;
            }
        }

        public override string ToString()
        {
            return $"Id: {Id}, UserId: {UserId}, Title: {Title}, Description: {Description}, DueDate: {DueDate}, IsCompleted: {IsCompleted}, CompletedDate: {CompletedDate}";
        }
    }
}
