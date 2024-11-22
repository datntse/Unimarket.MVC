using Unimarket.MVC.Models.ViewModels;

namespace Unimarket.MVC.Models.CreateModels
{
    public class CheckOutCM
    {
        public String Note { get; set; }    
        public String Address { get; set; }
    }

    public class CheckoutAddress
    {
        public string street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Citi { get; set; }
    }

    public class CheckOutDTO
    {
        public String UserId { get; set; }
        public String Note { get; set; }
        public String Address { get; set; }
        public String PaymentType { get; set; }
    }

    public class AddressResponse
    {
        public int status { get; set; }
        public int data { get; set; }
        public string message { get; set; }
    }
}
