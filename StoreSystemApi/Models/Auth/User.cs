using Microsoft.AspNetCore.Identity;
using StoreSystemApi.Models.Entities;
using System.Text.Json.Serialization;

namespace StoreSystemApi.Models.Auth
{
    public class User : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BankCard { get; set; }
        [JsonIgnore] public BonusCard? BonusCard { get; set; }
        [JsonIgnore] public List<Purchase>? Purchases { get; set; }
    }
}
