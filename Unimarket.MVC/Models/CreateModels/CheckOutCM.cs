namespace Unimarket.MVC.Models.CreateModels
{
    public class CheckOutCM
    {
        public String Note { get; set; }    
        public String Address { get; set; }
    }

    public class CheckOutDTO
    {
        public String UserId { get; set; }
        public String Note { get; set; }
        public String Address { get; set; }
        public String PaymentType { get; set; }
    }
}
