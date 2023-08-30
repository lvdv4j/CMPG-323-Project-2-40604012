namespace cmpg323_project.DTO
{
    public class ProductsDTO
    {
        public short ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductDescription { get; set; }
        public int? UnitsInStock { get; set; }
    }
}
