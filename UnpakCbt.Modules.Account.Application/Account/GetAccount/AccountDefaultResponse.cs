namespace UnpakCbt.Modules.Account.Application.Account.GetAccount
{
    public sealed record AccountDefaultResponse
    {
        public string Id { get; set; }
        public string Uuid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Level { get; set; }
        public string Status { get; set; }
    }
}
