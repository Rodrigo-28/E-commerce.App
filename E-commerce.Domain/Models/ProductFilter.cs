namespace E_commerce.Domain.Models
{
    public class ProductFilter
    {
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool InStockOnly { get; set; }
        public string? SearchTerm { get; set; }
    }
}
