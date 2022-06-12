using StoreSystemApi.Models.Auth;
using System.Text.Json.Serialization;

namespace StoreSystemApi.Models.Entities
{
    public class Purchase : Entity
    {
        public decimal Total { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string? PurchaseType { get; set; }
        public decimal? TaxRate { get; set; }
        
        public Guid CashierId { get; set; }
        public Guid? BonusCardId { get; set; }
        [JsonIgnore] public User? Cashier { get; set; }
        [JsonIgnore] public BonusCard? BonusCard { get; set; }
        // many-to-many
        [JsonIgnore] public List<PurchaseProduct>? PurchaseProducts { get; set; }
    }
}
