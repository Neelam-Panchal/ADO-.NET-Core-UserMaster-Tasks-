using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace UserMaster.Core.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter the FirstName")]
        public string FirstName { get; set; }

        public int IdUser { get; set; }

        [Required(ErrorMessage = "Please enter the MiddleName")]
        public string MiddleName { get; set; }


        [Required(ErrorMessage = "Please enter the LastName")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Please enter the EmailAddress")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string EmailAddress { get; set; }



        [Required(ErrorMessage = "Please enter the Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string DateOfBirth { get; set; }



        [Required(ErrorMessage = "Please enter the Phone Number")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Phone Number must contain only numbers.")]
        [StringLength(10)]

        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Nationality is required.")]
        public int Nationality { get; set; }

        /*public IEnumerable<System.Web.Mvc.SelectListItem> Nationalities { get; set; }*/



        [Required(ErrorMessage = "Please enter the User Name")]
        public string UserName { get; set; }



        [Required(ErrorMessage = "Please enter the Password")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$", ErrorMessage = "Password must be alphanumeric.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }



        [Required(ErrorMessage = "Please enter the Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        

    }
}