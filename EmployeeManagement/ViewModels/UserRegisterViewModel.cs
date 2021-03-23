using EmployeeManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    //this class use to bring the user input data from View to pass to Controller
    public class UserRegisterViewModel
    {
        //Model Validation: []
        [Required]
        [DisplayName("Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "UserAccount")]
        [ValidEmailDomain(allowedDomain: "tonetone.com",
            ErrorMessage = "Email Domain must be 'tonetone.com'")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password do not match")]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }
    }
}
