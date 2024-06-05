namespace Unimarket.MVC.Models.ViewModels
{
    public class ResponseProductVM
    {
        public int Total { get; set; }
        public List<ProductVM> Data { get; set; }
        public int CurrentPage { get; set; }
    }

    public class ProductVM
    {
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string ProductDetail { get; set; }
        public int Quantity { get; set; }
		public int Price { get; set; }
		public int Status { get; set; }
		public string ImageUrl { get; set; }
		public List<string> CategoryName { get; set; }
		public List<string> SubImageUrl { get; set; }
	}

	public class ProductManageVM
	{
		public ResponseProductVM Product { get; set; }
		public List<CategoryVM> Categories { get; set; }
	}

	public class ProductUM 
    {
		public ProductVM Product { get; set; }
        public List<CategoryVM> Categories { get; set; }
    }
}
