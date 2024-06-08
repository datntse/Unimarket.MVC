using System.ComponentModel.DataAnnotations;

namespace Unimarket.MVC.Models.ViewModels
{
	public class UserVM
	{
		public String Id { get; set; }
		public DateTime? DOB { get; set; }
		public String FirstName { get; set; }
		public String LastName { get; set; }
		public String PhoneNumber { get; set; }
		public String CCCDNumber { get; set; }
		public String StudentId { get; set; }
		public String Email { get; set; }
		public String Avatar { get; set; }
		public bool Gender { get; set; }
		public int Status { get; set; }
	}
}
