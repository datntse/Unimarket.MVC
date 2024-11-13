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
        public string fullName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public String? phone { get; set; }
    }
}
