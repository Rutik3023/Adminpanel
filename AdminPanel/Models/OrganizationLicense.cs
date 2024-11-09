namespace  AdminPanel.Models
{
    public class OrganizationLicense
    {
        public string? Sys_Code { get; set; }
        public string? OrgCode { get; set; }
        public string? AuthKey { get; set; }
        public DateTime? Valid_From { get; set; }
        public DateTime? Valid_Till { get; set; }
    }
}
