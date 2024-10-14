namespace Unimarket.MVC.Models.ViewModels
{
    public class ProductResponseApi
    {
        public int status { get; set; }
        public ResponseProductVM1 Data { get; set; }
        public string Message { get; set; }
    }

    public class ProductDetailResponseApi
    {
        public int status { get; set; }
        public ProductVM1 Data { get; set; }
        public string Message { get; set; }
    }


    public class ResponseProductVM1
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int totalElements { get; set; }
        public List<ProductVM1> Data { get; set; }
    }

    public class ProductVM1
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public List<ImageProductVM> Images { get; set; }
        public bool Deleted { get; set; }
    }

    public class ImageProductVM
    {
        public int Id { get; set; }
        public string imageUrl { get; set; }
    }
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
