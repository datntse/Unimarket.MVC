using System.ComponentModel.DataAnnotations;

namespace Unimarket.MVC.Models.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Please enter your email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class LoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
