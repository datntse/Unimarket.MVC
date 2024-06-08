namespace Unimarket.MVC.Models.ViewModels
{
    public class ResponseOrder
    {
        public int Total { get; set; }
        public List<OrderDTO> Data { get; set; }
        public int CurrentPage { get; set; }
    }
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public String PaymentType { get; set; }
        public float TotalPrice { get; set; }
        public int Status { get; set; }
        public DateTime CreateAt { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Note {  get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public List<OrderdetailVM> OrderdetailVM { get; set; }
    }
    public class OrderdetailVM
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public float TotalPrice { get; set; }
        public ItemsVM ItemsVMs { get; set; }
    }

    public class ItemsVM
    {
        public String Name { get; set; }
        public float Price { get; set; }
        public String ImageUrl { get; set; }
    }

    public class UpdateOrder
    {
        public Guid OrderId { get; set; }
        public int Status { get; set; }
    }
}
