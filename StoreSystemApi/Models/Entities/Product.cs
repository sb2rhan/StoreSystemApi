using System.Text.Json.Serialization;

namespace StoreSystemApi.Models.Entities
{
    public class Product : Entity
    {
        public string? Barcode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int StockAmount { get; set; }
        public decimal? Price { get; set; }
        public decimal DiscountRate { get; set; } = 0m;

        public Guid SupplierId { get; set; }
        public Guid? CategoryId { get; set; }
        [JsonIgnore] public Supplier? Supplier { get; set; }
        [JsonIgnore] public Category? Category { get; set; }
        // many-to-many
        [JsonIgnore] public List<PurchaseProduct>? PurchaseProducts { get; set; }
    }
}
