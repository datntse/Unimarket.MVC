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
        public int total { get; set; }
        public int currentPage { get; set; }
		public List<ProductDTO> data { get; set; }
  
    }

    public class ProductDTO
    {
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Quantity { get; set; }
		public int Price { get; set; }
		public int Status { get; set; }
		public string ImageUrl { get; set; }
		public List<string> CategoryName { get; set; }
		public List<string> SubImageUrl { get; set; }
	}
}
