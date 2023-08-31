using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CPW219_eCommerceSite.Models
{
    public class Users
    {
        public int UsersID { get; set; }        
        [Required(ErrorMessage = "Please Enter Username")]
        [Display(Name = "Username:")]        
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Password:")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please Enter Email")]
        [Display(Name = "Email:")]
        public string Email { get; set; }
    }
}
