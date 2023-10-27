using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace UserMaster.Core.ViewModels
{
    public class TaskViewModel
    {
        [Required(ErrorMessage = "Please enter the UserName")]
        public string UserName { get; set; }


        public int TaskID { get; set; }



        public int IdUser { get; set; }

        [Required(ErrorMessage = "Please enter the TaskTitle")]
        public string TaskTitle { get; set; }


        [Required(ErrorMessage = "Please enter the TaskDescription")]
        public string TaskDescription { get; set; }


        [Required(ErrorMessage = "Please enter the  Priority")]
         public string Priority { get; set; }


        [Required(ErrorMessage = "Please enter the EstimatedHours")]
         public double EstimatedHours { get; set; }


        [Required(ErrorMessage = "Please enter the Status")]
         public string Status { get; set; }



        public int AssignToUserId { get; set; }
    }


}
public enum TaskStatus
{
    New,
    Open,
    Assigned,
    InProgress,
    Completed,
    Closed
}