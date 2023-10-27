using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMaster.Core.ViewModels;
using UserMaster.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UserMaster.Controllers
{
    public class TaskController : Controller
    {

        private readonly IUserMasterService _userMasterService;

        public TaskController(IUserMasterService userMasterService)
        {
            _userMasterService = userMasterService;
        }

        [HttpGet]
        public IActionResult Task()
        {
            return View();
        }

        //[HttpGet]
        //public IActionResult GetTask()
        //{
        //    int userId = HttpContext.Session.GetInt32("IdUser") ?? -1;

        //    if (userId >= 0)
        //    {
        //        // Retrieve the user's tasks
        //        var userTasks = _userMasterService.GetUserTasks(userId);


        //        var AssignedTasks = _userMasterService.GetAssignedTasks(userId);



        //        var combinedTasks = userTasks.Concat(AssignedTasks).ToList();

        //        ViewBag.TaskStatus = combinedTasks.FirstOrDefault()?.Status;
        //        // Store the combined list in the ViewBag
        //        ViewBag.Tasks = combinedTasks;

        //        var users = _userMasterService.GetAllUsers();
        //        ViewBag.Users = users;



        //        return View();

        //    }
        //    else
        //    {
        //        return RedirectToAction("Login");
        //    }
        //}

        [HttpPost]
        public IActionResult Task(TaskViewModel task)
        {

            

                try
                {
                    int userId = HttpContext.Session.GetInt32("IdUser") ?? -1;




                    if (userId >= 0)
                    {
                        bool taskAdded = _userMasterService.AddTask(userId, task);

                        if (taskAdded)
                        {
                            task.Status = TaskStatus.New.ToString();
                            return RedirectToAction("GetTask1");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Failed to create the task.");
                            return View(task);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                }



                catch (Exception ex)
                {
                    // Handle any exceptions or errors, and optionally log them
                    Console.WriteLine("Error: " + ex.Message);
                    return RedirectToAction("ErrorPage");
                }

               
            
          
        }
        [HttpPost]
        public IActionResult SDeleteTask(int taskId)
        {
            int userId = HttpContext.Session.GetInt32("IdUser") ?? -1;

            if (userId >= 0)
            {
                bool taskDeleted = _userMasterService.SoftDeleteTask(taskId);

                if (taskDeleted)
                {
                    // Successfully soft deleted the task
                    return RedirectToAction("GetTask1");
                }
                else
                {
                    
                    // Handle the case where the task couldn't be soft deleted
                    return RedirectToAction("GetTask1", new { error = "Failed to delete the task." });
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

       /* [HttpGet]
        public IActionResult gettaskid(int taskId,string taskstatus)
        {
            var task = _userMasterService.GetTaskById(taskId);
            return Ok(task);
        }*/

        [HttpPost]
        public IActionResult AssignToTask(TaskViewModel model)
        {
            /*int userId = HttpContext.Session.GetInt32("IdUser") ?? -1;
            var userTasks = _userMasterService.GetUserTasks(userId);
            ViewBag.Tasks = userTasks;*/
            // Validate taskID and assignToUserId here as needed

            try
            {
                // Retrieve the task using taskID
                TaskViewModel task = _userMasterService.GetTaskById(model.TaskID);

                int userId = HttpContext.Session.GetInt32("IdUser") ?? -1;
                /*List<TaskViewModel> AssignedTasks = _userMasterService.GetTasksStatus(userId,taskStatus);*/

                if (task != null)
                {
                    // Update the AssignToUserId for the task
                    if(model.AssignToUserId > 0) 
                    { 
                        task.AssignToUserId = model.AssignToUserId; 
                    }
                    
                    task.Status = model.Status;
                    //task.TaskID = taskID;

                    // Call the UpdateTask method to save the changes to the database
                    _userMasterService.UpdateTask(task, model.Status);

                    ViewBag.TaskID = model.TaskID;
                    ViewBag.AssignToUserId = model.AssignToUserId;

                    // Redirect to a success page or return a JSON response
                    return RedirectToAction("GetTask1", "Task");
                }
                else
                {
                    // Handle the case where the task is not found
                    return RedirectToAction("TaskNotFound");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions or errors, and optionally log them
                Console.WriteLine("Error: " + ex.Message);
                return RedirectToAction("ErrorPage");
            }
        }




        public IActionResult GetTask1()
        {
            int userId = HttpContext.Session.GetInt32("IdUser") ?? -1;

            string username = HttpContext.Session.GetString("UserName");

            ViewBag.UserName = username;

            if (userId >= 0)
            {
                // Retrieve the user's tasks
                var userTasks = _userMasterService.GetUserTasks(userId);


                var AssignedTasks = _userMasterService.GetAssignedTasks(userId);



                var combinedTasks = userTasks.Concat(AssignedTasks).ToList();

                ViewBag.TaskStatus = combinedTasks.FirstOrDefault()?.Status;



                // Store the combined list in the ViewBag
                ViewBag.Tasks = combinedTasks;

                var users = _userMasterService.GetAllUsers();
                ViewBag.Users = users;
                return View();
            }
            else
            {
                return View("Login");
            }
        }




    }
}
