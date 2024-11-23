namespace Unimarket.MVC.Models.ViewModels
{
    public class ResponseOrder
    {
        public int status { get; set; }
        public DataResponse data { get; set; }
        public string message { get; set; }
    }

    public class ResponseReveunue
    {
        public int status { get; set; }
        public RevenuResponse data { get; set; }
        public string message { get; set; }
    }

    public class RevenuResponse
    {
        public int month { get; set; }
        public int year { get; set; }
        public decimal revenueAmount { get; set; }
    }

    public class DataResponse
    {
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public int pageSize { get; set; }
        public int totalElements { get; set; }
        public List<OrderResponse> data { get; set; }

    }
    public class OrderResponse
    {
        public int id { get; set; }
        public string? orderDate { get; set; }
        public decimal? totalPrice { get; set; }
        public string? orderStatus { get; set; }
        public int? couponId { get; set; }
        public int? userId { get; set; }
        public string userName { get; set; }
        public decimal discountAmount { get; set; }
        public int? addressId { get; set; }
        public string? shippingFee { get; set; }
        public string? orderDetails { get; set; }
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
        public string Note { get; set; }
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
