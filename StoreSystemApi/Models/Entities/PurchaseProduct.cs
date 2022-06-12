using System.Text.Json.Serialization;

namespace StoreSystemApi.Models.Entities
{
    public class PurchaseProduct
    {
        public Guid ProductId { get; set; }
        public Guid PurchaseId { get; set; }
        public int PurchasedAmount { get; set; }
        [JsonIgnore] public Product? Product { get; set; }
        [JsonIgnore] public Purchase? Purchase { get; set; }
    }
}
