namespace Unimarket.MVC.Models.ViewModels
{
	public class ResponseCartVM
	{
		public int Total { get; set; }
		public List<CartDTO> Data { get; set; }
		public int CurrentPage { get; set; }
	}

	public class UserCartResponse
	{
		public int status { get; set; }
		public CartDTO Data { get; set; }
		public string message { get;set; }
	}

	public class CartDTO
	{
		public int id { get; set; }
		public float totalPrice { get; set; }

        public string orderDate { get; set; }
		public string orderStatus { get; set; }
		public string couponId { get; set; }
		public string userId { get; set; }
		public string userName { get; set; }
		public string addressId { get; set; }
		public float shippingFee { get; set; }
		public float discountAmount { get; set; }	
		public List<OrderDetails> orderDetails { get; set; }
    }

	public class OrderDetails
	{
		public string id { get; set; }
		public string productName { get; set;}
		public int quantity { get; set; }
		public float price { get; set; }
		public string image { get; set; }
	}
	public class ItemDetails
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string ImageUrl { get; set; }
	}
	public class AddToCart
	{
		public string UserId { get; set; }
		public Guid ItemId { get; set; }
	}
	public class UpdateItemQuantityDTO
	{
		public string UserId { get; set; }
		public string ItemId { get; set; }
		public int Quantity { get; set; }
	}
	public class AddToCarts
	{
		public string itemId { get; set; }
		public int quantity { get; set; }
	}

    public class UpdateOrderModel
    {
        public int orderId { get; set; }
        public int type { get; set; }
    }

}
