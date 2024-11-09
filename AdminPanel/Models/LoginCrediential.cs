namespace  AdminPanel.Models
{
    public class LoginCrediential
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Roles { get; set; }

        public bool? isChecker { get; set; }
        public string? OrgCode { get; set; }
        public string? OrgName { get; set; }
        public string? Email { get; set; }
        public string? wrongloginattempts { get; set; }
        public int? Code { get; set; }
        public dynamic? Message { get; set; }
        public string? SessionId { get; set; }
    }
}
