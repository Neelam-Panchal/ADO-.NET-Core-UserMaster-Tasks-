using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMaster.Core.ViewModels;
using UserMaster.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UserMaster.Infrastructure.Interfaces
{
    public interface IUserMasterRepository
    {
        DataTable GetDataTable(string sqlQuery, List<SqlParameter> mySqlParams);
        int ExecuteQuery(string sqlQuery, List<SqlParameter> mySqlParams);

        bool RegisterUser(RegisterViewModel model);

        public bool IsUsernameExists(string username);

        List<SelectListItem> PopulateNationalities();

        public LoginViewModel LoginUser(LoginViewModel model);

        public bool AddTask(int userId, TaskViewModel task);

        public List<TaskViewModel> GetUserTasks(int userId);


        public bool SoftDeleteTask(int taskId);

        public List<SelectListItem> GetAllUsers();

        public TaskViewModel GetTaskById(int taskid);

        public void UpdateTask(TaskViewModel task, string taskStatus);

        public List<TaskViewModel> GetAssignedTasks(int IdUser);

       /* public List<TaskViewModel> GetTasksStatus(int IdUser, string taskstatus);*/

    }
}

