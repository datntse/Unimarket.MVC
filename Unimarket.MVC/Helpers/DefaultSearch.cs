namespace Unimarket.MVC.Helpers
{
    public class DefaultSearch
    {
        public int perPage { get; set; } = 10;
        public int currentPage { get; set; } = 0;
        public String? sortBy { get; set; }
        public bool isAscending { get; set; } = true;
        public List<string> categoryNames { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
    }
}
