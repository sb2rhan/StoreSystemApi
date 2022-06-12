using System.Text.Json.Serialization;

namespace StoreSystemApi.Models.Entities
{
    public class Category : Entity
    {
        public string? Name { get; set; }

        [JsonIgnore] public List<Product>? Products { get; set; }
        // self relate this entity
    }
}
