namespace E_commerce.application.Dtos.Requests
{
    public class ProductQueryDto
    {
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? InStockOnly { get; set; }
        public string? SearchTerm { get; set; }
    }
}
