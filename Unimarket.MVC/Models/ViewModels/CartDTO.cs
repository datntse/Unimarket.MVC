namespace Unimarket.MVC.Models.ViewModels
{
	public class ResponseCartVM
	{
		public int Total { get; set; }
		public List<CartDTO> Data { get; set; }
		public int CurrentPage { get; set; }
	}

	public class CartDTO
	{
		public Guid Id { get; set; }
		public Guid ItemId { get; set; }
		public string userId { get; set; }
		public DateTime CreateAt { get; set; }
		public DateTime? UpdateAt { get; set; }
		public int Quantity { get; set; }
		public ItemDetails Item { get; set; }
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
}
