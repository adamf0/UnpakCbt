using System.ComponentModel.DataAnnotations.Schema;

namespace UnpakCbt.Modules.Account.Application.Account.GetAccount
{
    public sealed record AccountResponse
    {
        public string Uuid { get; set; }
        public string Username { get; set; }
        public string Level { get; set; }
        public string Status { get; set; }
    }
}
