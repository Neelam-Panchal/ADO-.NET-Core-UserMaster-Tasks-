using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UserMaster.Infrastructure.Interfaces;
using UserMaster.Core.DBContext;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;
using UserMaster.Core.ViewModels;
using UserMaster.ViewModels;

namespace UserMaster.Infrastructure.Services
{
    public class UserMasterRepository : IUserMasterRepository
    {

        private readonly SQLDBHelper _dbHelper;
        private readonly IConfiguration _configuration;

        public object WindowsIdentity { get; private set; }

        public UserMasterRepository(SQLDBHelper dbHelper, IConfiguration configuration)
        {
            _dbHelper = dbHelper;
            _configuration = configuration;
        }

        public DataTable GetDataTable(string sqlQuery, List<SqlParameter> mySqlParams)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                try
                {
                    DataTable _dataTable = new DataTable();
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    if (mySqlParams != null && mySqlParams.Count > 0)
                    {
                        foreach (SqlParameter param in mySqlParams)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }
                    SqlDataAdapter _da = new SqlDataAdapter(cmd);
                    _da.Fill(_dataTable);
                    return _dataTable;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public int ExecuteQuery(string sqlQuery, List<SqlParameter> mySqlParams)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    int nRowsAffected = 0;
                    if (mySqlParams != null && mySqlParams.Count > 0)
                    {
                        foreach (SqlParameter param in mySqlParams)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }
                    nRowsAffected = cmd.ExecuteNonQuery();
                    return nRowsAffected;
                }
                catch (Exception ex)
                {
                    // Log or report the exception
                    Console.WriteLine($"An error occurred: {ex.Message}");

                    // Rethrow the exception to propagate it up the call stack
                    throw;
                }
            }
        }

        public List<SelectListItem> PopulateNationalities()
        {

            List<SelectListItem> nationalities = new List<SelectListItem>();
            string query = "SELECT IdCountry, CountryName FROM CountryMaster";

            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            nationalities.Add(new SelectListItem
                            {
                                Value = reader["IdCountry"].ToString(),
                                Text = reader["CountryName"].ToString()
                            });
                        }

                        /*model.Nationalities = new SelectList(nationalities, "Value", "Text");*/
                    }
                }
            }
            return nationalities;
        }

        public bool RegisterUser(RegisterViewModel model)
        {
            try
            {
                string insertQuery = "INSERT INTO UserMaster_Nilam (UserName, Password, FirstName, MiddleName, LastName, Email, DateOfBirth, PhoneNumber, Nationality, CreatedBy, CreatedOn) " +
                                     "VALUES (@UserName, @Password, @FirstName, @MiddleName, @LastName, @Email, @DateOfBirth, @PhoneNumber, @Nationality, @CreatedBy, @CreatedOn)";

                int databaseUser = 1;
                DateTime createdOn = DateTime.Now;

                var mySqlParams = new List<SqlParameter>
                {
                    new SqlParameter("@UserName", model.UserName),
                    new SqlParameter("@Password", model.Password),
                    new SqlParameter("@FirstName", model.FirstName),
                    new SqlParameter("@LastName", model.LastName),
                    new SqlParameter("@Email", model.EmailAddress),
                    new SqlParameter("@DateOfBirth", model.DateOfBirth),
                    new SqlParameter("@PhoneNumber", model.PhoneNumber),
                    new SqlParameter("@Nationality", model.Nationality),
                    new SqlParameter("@CreatedBy", databaseUser),
                    new SqlParameter("@CreatedOn", createdOn)
                };

                if (model.MiddleName != null)
                {
                    mySqlParams.Add(new SqlParameter("@MiddleName", model.MiddleName));
                }
                else
                {
                    mySqlParams.Add(new SqlParameter("@MiddleName", DBNull.Value));
                }

                mySqlParams.Add(new SqlParameter("@SUBMIT", "INSERT"));

                int affectedRows = ExecuteQuery(insertQuery, mySqlParams);

                return affectedRows > 0;
            }
            catch (Exception)
            {
                // Handle the exception and log it if needed
                return false;
            }
        }

        public bool IsUsernameExists(string username)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM UserMaster_Nilam WHERE UserName = @UserName";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@UserName", username);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

public LoginViewModel LoginUser(LoginViewModel model)
{

            int userId = 0;
            string username = null;

            string query = "SELECT * FROM UserMaster_Nilam WHERE UserName = @UserName AND Password = @Password";
                var mySqlParams = new List<SqlParameter>
                {
                    new SqlParameter("@UserName", model.UserName),
                    new SqlParameter("@Password", model.Password),
                };
                DataTable result = GetDataTable(query, mySqlParams);


                if (result.Rows.Count > 0)
                {
                    userId = Convert.ToInt32(result.Rows[0]["IdUser"]);
                    username = result.Rows[0]["UserName"].ToString();
                    return new LoginViewModel
                    {
                        IdUser = userId,
                        UserName = username
                    };
            }
                else
                {
                return null;
                }

            }
        

            
            public bool AddTask(int userId, TaskViewModel task)
            {

                try
                {
                    string insertTaskQuery = "INSERT INTO UserTasks_Nilam(IdUser, TaskTitle, TaskDescription, Priority, EstimatedHours,Status) " +
                                     "VALUES (@IdUser, @TaskTitle, @TaskDescription, @Priority, @EstimatedHours,'New')";


                //string status = (userId == null) ? "New" : "Assigned";

                var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@IdUser", userId),
                        new SqlParameter("@TaskTitle", task.TaskTitle),
                        new SqlParameter("@TaskDescription", task.TaskDescription),
                        new SqlParameter("@Priority", task.Priority),
                        new SqlParameter("@EstimatedHours", task.EstimatedHours),
                        new SqlParameter("@Status",task.Status = TaskStatus.New.ToString()
),

                        //new SqlParameter("@Status", task.Status),
                       /* new SqlParameter("@AssignToUserId", task.AssignToUserId),*/
                    };
                    int Rows = ExecuteQuery(insertTaskQuery, parameters);

                    return Rows > 0;

                }
                catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions or log them as needed
                return false;
            }
                       
                   
            }


        public List<TaskViewModel> GetUserTasks(int IdUser)
        {
            try
            {
                string selectTasksQuery = "SELECT * FROM UserTasks_Nilam WHERE IdUser = @IdUser AND DeletedAt IS NULL";

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@IdUser", IdUser)
                };

                DataTable result = GetDataTable(selectTasksQuery, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    List<TaskViewModel> userTasks = new List<TaskViewModel>();

                    foreach (DataRow row in result.Rows)
                    {
                        if (row != null) // Check for null DataRow
                        {
                            TaskViewModel task = new TaskViewModel
                            {
                                TaskID = Convert.ToInt32(row["TaskID"]),
                                TaskTitle = row["TaskTitle"].ToString(),
                                TaskDescription = row["TaskDescription"].ToString(),
                                Priority = row["Priority"].ToString(),
                                EstimatedHours = Convert.ToDouble(row["EstimatedHours"]),
                                Status = row["Status"].ToString(),
                                AssignToUserId = (row["AssignToUserId"] != DBNull.Value) ? Convert.ToInt32(row["AssignToUserId"]) : 0 // You can use a default value here, e.g., 0

                            };

                            userTasks.Add(task);
                        }
                    }

                    return userTasks;
                }
                else
                {
                    return new List<TaskViewModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions or log them as needed
                return null;
            }
        }
        public List<TaskViewModel> GetAssignedTasks(int IdUser)
        {
            try
            {
                string selectTasksQuery = "SELECT * FROM UserTasks_Nilam WHERE AssignToUserId = @IdUser AND DeletedAt IS NULL";

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@IdUser", IdUser)
                };

                DataTable result = GetDataTable(selectTasksQuery, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    List<TaskViewModel> userTasks = new List<TaskViewModel>();

                    foreach (DataRow row in result.Rows)
                    {
                        if (row != null) // Check for null DataRow
                        {
                            TaskViewModel task = new TaskViewModel
                            {
                                TaskID = Convert.ToInt32(row["TaskID"]),
                                TaskTitle = row["TaskTitle"].ToString(),
                                TaskDescription = row["TaskDescription"].ToString(),
                                Priority = row["Priority"].ToString(),
                                EstimatedHours = Convert.ToDouble(row["EstimatedHours"]),
                                Status = row["Status"] != DBNull.Value ? row["Status"].ToString() : "Open",
                                AssignToUserId = (row["AssignToUserId"] != DBNull.Value) ? Convert.ToInt32(row["AssignToUserId"]) : 0 // You can use a default value here, e.g., 0

                            };

                            userTasks.Add(task);
                        }
                    }

                    return userTasks;
                }
                else
                {
                    return new List<TaskViewModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions or log them as needed
                return null;
            }
        }

        /*public List<TaskViewModel> GetTasksStatus(int IdUser,string taskstatus)
        {
            try
            {
                string selectTasksQuery = "SELECT * FROM UserTasks_Nilam WHERE AssignToUserId = @IdUser AND DeletedAt IS NULL";

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@IdUser", IdUser)
                };

                DataTable result = GetDataTable(selectTasksQuery, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    List<TaskViewModel> userTasks = new List<TaskViewModel>();

                    foreach (DataRow row in result.Rows)
                    {
                        if (row != null) // Check for null DataRow
                        {
                            TaskViewModel task = new TaskViewModel
                            {
                                TaskID = Convert.ToInt32(row["TaskID"]),
                                TaskTitle = row["TaskTitle"].ToString(),
                                TaskDescription = row["TaskDescription"].ToString(),
                                Priority = row["Priority"].ToString(),
                                EstimatedHours = Convert.ToDecimal(row["EstimatedHours"]),
                                Status = taskstatus,
                                AssignToUserId = (row["AssignToUserId"] != DBNull.Value) ? Convert.ToInt32(row["AssignToUserId"]) : 0 // You can use a default value here, e.g., 0

                            };

                            userTasks.Add(task);
                        }
                    }

                    return userTasks;
                }
                else
                {
                    return new List<TaskViewModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions or log them as needed
                return null;
            }
        }*/


        public bool SoftDeleteTask(int taskId)
        {
            try
            {
                string softDeleteTaskQuery = "UPDATE UserTasks_Nilam SET DeletedAt = GETDATE() WHERE TaskID = @TaskID AND Status = 'New'";

                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@TaskID", taskId)
                };

                int affectedRows = ExecuteQuery(softDeleteTaskQuery, parameters);

                return affectedRows > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<SelectListItem> GetAllUsers()
        {
            string query = "SELECT IdUser,UserName FROM UserMaster_Nilam";
            var mySqlParams = new List<SqlParameter>();

            DataTable result = GetDataTable(query, mySqlParams);

            List<SelectListItem> userSelectList = new List<SelectListItem>();

            foreach (DataRow row in result.Rows)
            {
                RegisterViewModel user = new RegisterViewModel
                {
                    IdUser = Convert.ToInt32(row["IdUser"]),
                    UserName = row["UserName"].ToString(),

                };

                userSelectList.Add(new SelectListItem
                {
                    Value = user.IdUser.ToString(),
                    Text = user.UserName
                });
            }
            return userSelectList;
        }

        public TaskViewModel GetTaskById(int taskid)
        {
            try
            {
                string query = "SELECT * FROM UserTasks_Nilam WHERE TaskID = @TaskID";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@TaskID", taskid)
                };

                DataTable result = GetDataTable(query, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    DataRow row = result.Rows[0];

                    TaskViewModel task = new TaskViewModel
                    {
                        TaskID = Convert.ToInt32(row["TaskID"]),
                        TaskTitle = row["TaskTitle"].ToString(),
                        TaskDescription = row["TaskDescription"].ToString(),
                        Priority = row["Priority"].ToString(),
                        EstimatedHours = Convert.ToDouble(row["EstimatedHours"]),
                        Status= row["Status"].ToString(),
                        AssignToUserId = (row["AssignToUserId"] != DBNull.Value) ? Convert.ToInt32(row["AssignToUserId"]) : 0
                    };

                    return task;
                }
                else
                {
                    // Handle the case where no rows were found for the given taskID
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Handle exceptions or log them as needed
                return null;
            }
        }


        public void UpdateTask(TaskViewModel task,string taskStatus)
        {
            
                string query = "UPDATE UserTasks_Nilam SET TaskTitle = @TaskTitle, TaskDescription = @TaskDescription, Priority = @Priority, EstimatedHours = @EstimatedHours, Status = @Status, AssignToUserId = @AssignToUserId WHERE TaskID = @TaskID";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@TaskID", task.TaskID),
                    new SqlParameter("@TaskTitle", task.TaskTitle),
                    new SqlParameter("@TaskDescription", task.TaskDescription),
                    new SqlParameter("@Priority", task.Priority),
                    new SqlParameter("@EstimatedHours", task.EstimatedHours),
                    new SqlParameter("@Status", string.IsNullOrWhiteSpace(task.Status) ? (object)"Open" : task.Status),
                    new SqlParameter("@AssignToUserId", task.AssignToUserId),

                };
                int affectedRows = ExecuteQuery(query, parameters);

                if (affectedRows > 0)
                {
                    Console.WriteLine("Task updated successfully.");
                }
                else
                {
                    Console.WriteLine("Task update failed.");
                }
        }



        /*// Update an existing task
        public void UpdateTask(int taskID, TaskViewModel task)
        {
            // Add your SQL query to update the task in the UserTasks table
            string updateTaskQuery = @"
        UPDATE UserTasks
        SET TaskTitle = @TaskTitle, TaskDescription = @TaskDescription,
            Priority = @Priority, EstimatedHours = @EstimatedHours
        WHERE TaskID = @TaskID;
    ";

            var parameters = new
            {
                TaskID = taskID,
                task.TaskTitle,
                task.TaskDescription,
                task.Priority,
                task.EstimatedHours
            };

            _connection.Execute(updateTaskQuery, parameters);
        }
        */
        // Delete a task
        /* public void DeleteTask(int taskID)
         {
             // Add your SQL query to delete the task from the UserTasks table
             string deleteTaskQuery = "DELETE FROM UserTasks WHERE TaskID = @TaskID;";
             _connection.Execute(deleteTaskQuery, new { TaskID = taskID });
         }

         // Retrieve tasks for a specific user
         public IEnumerable<TaskViewModel> GetTasksForUser(int userID)
         {
             // Add your SQL query to select tasks for the given user
             string selectTasksQuery = "SELECT * FROM UserTasks WHERE UserID = @UserID;";
             return _connection.Query<TaskViewModel>(selectTasksQuery, new { UserID = userID });
         }*/


    }
}
