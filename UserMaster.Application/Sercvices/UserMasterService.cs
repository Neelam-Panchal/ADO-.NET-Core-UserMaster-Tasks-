using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMaster.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using UserMaster.Core.ViewModels;
using UserMaster.ViewModels;

namespace UserMaster.Application.Services
{
    public class UserMasterService : IUserMasterService
    {
        private readonly IUserMasterRepository _repository;

        public UserMasterService(IUserMasterRepository repository)
        {
            _repository = repository;
        }

        public DataTable GetDataFromDatabase(string sqlQuery, List<SqlParameter> parameters)
        {
            return _repository.GetDataTable(sqlQuery, parameters);
        }

        public int ExecuteQuery(string sqlQuery, List<SqlParameter> parameters)
        {
            return _repository.ExecuteQuery(sqlQuery, parameters);
        }

        public bool RegisterUser(RegisterViewModel model)
        {
            return _repository.RegisterUser(model);
        }
        public List<SelectListItem> PopulateNationalities()
        {
            return _repository.PopulateNationalities();
        }
        public bool IsUsernameExists(string username)
        {
            return _repository.IsUsernameExists(username);
        }
        public LoginViewModel LoginUser(LoginViewModel model)
        {
            return _repository.LoginUser(model);

        }
        public bool AddTask(int userId, TaskViewModel task)
        {
            return _repository.AddTask(userId,task);
        }

        public List<TaskViewModel> GetUserTasks(int userId)
        {
            return _repository.GetUserTasks(userId);
        }
        public List<TaskViewModel> GetAssignedTasks(int IdUser)
        {
            return _repository.GetAssignedTasks(IdUser);
        }

       /* public List<TaskViewModel> GetTasksStatus(int IdUser, string taskstatus)
        {
            return _repository.GetTasksStatus(IdUser, taskstatus);
        }
*/
        public bool SoftDeleteTask(int taskId)
        {
            return _repository.SoftDeleteTask(taskId);
        }
        public List<SelectListItem> GetAllUsers()
        {
            return _repository.GetAllUsers();
        }
        public TaskViewModel GetTaskById(int taskid)
        {
            return _repository.GetTaskById(taskid);
        }
        public void UpdateTask(TaskViewModel task, string taskStatus)
        {
             _repository.UpdateTask(task,taskStatus);
        }

    }
}

