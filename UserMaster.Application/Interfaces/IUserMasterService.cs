using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UserMaster.Core.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using UserMaster.ViewModels;

namespace UserMaster.Application.Services
{
    public interface IUserMasterService
    {
        DataTable GetDataFromDatabase(string sqlQuery, List<SqlParameter> parameters);
        int ExecuteQuery(string sqlQuery, List<SqlParameter> parameters);

        public bool RegisterUser(RegisterViewModel model);

        public bool IsUsernameExists(string username);

        public List<SelectListItem> PopulateNationalities();

        public LoginViewModel LoginUser(LoginViewModel model);

        public bool AddTask(int userId, TaskViewModel task);

        public List<TaskViewModel> GetUserTasks(int userId);

        public bool SoftDeleteTask(int taskId);

        public List<SelectListItem> GetAllUsers();

        public TaskViewModel GetTaskById(int taskid);

        public void UpdateTask(TaskViewModel task, string taskStatus);

        public List<TaskViewModel> GetAssignedTasks(int IdUser);

        //public List<TaskViewModel> GetTasksStatus(int IdUser, string taskstatus);



    }
}
