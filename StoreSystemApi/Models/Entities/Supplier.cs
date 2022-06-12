using System.Text.Json.Serialization;

namespace StoreSystemApi.Models.Entities
{
    public class Supplier : Entity
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        [JsonIgnore] public List<Product>? Products { get; set; }
    }
}
