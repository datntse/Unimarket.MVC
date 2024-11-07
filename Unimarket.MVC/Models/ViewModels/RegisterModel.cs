using System.ComponentModel.DataAnnotations;

namespace Unimarket.MVC.Models.ViewModels
{
    public class RegisterVM
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
		public String Phone { get; set; }
    }

	public class RegisterDTO
	{
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public String? PhoneNumber { get; set; }
    }
}
