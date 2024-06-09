using System.ComponentModel.DataAnnotations;

namespace Unimarket.MVC.Models.ViewModels
{
    public class RegisterVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
		public string UserName { get; set; }
        public string Password { get; set; }
		public String MSSV { get; set; }
		public String DOB { get; set; }
		public String Phone { get; set; }
        public bool Gender { get; set; }
    }

	public class RegisterDTO
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
        public String? DOB { get; set; }
        public String? PhoneNumber { get; set; }
        public String? CCCDNumber { get; set; }
        public String StudentId { get; set; }
        public String Avatar { get; set; }
        public bool Gender { get; set; }
        public int Status { get; set; } = 1;
    }
}
