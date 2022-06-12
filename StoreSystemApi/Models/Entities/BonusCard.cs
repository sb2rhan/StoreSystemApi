using StoreSystemApi.Models.Auth;
using System.Text.Json.Serialization;

namespace StoreSystemApi.Models.Entities
{
    public class BonusCard : Entity
    {
        public DateTime? IssueDate { get; set; }
        public bool IsActive { get; set; }

        public Guid? OwnerId { get; set; }
        [JsonIgnore] public User? Owner { get; set; }
        [JsonIgnore] public List<Purchase>? Purchases { get; set; }
    }
}
