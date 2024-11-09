namespace  AdminPanel.Models
{
    public class UserLoginSession
    {
        public string? Sys_Code { get; set; }
        public string? UserId { get; set; }
        public string? Device_OS { get; set; }
        public string? Browser_Detail { get; set; }
        public string? Device_Location { get; set; }
        public string? LoginTime { get; set; }
        public string? LogOutTime { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }

        public int? Code { get; set; }
        public dynamic? Message { get; set; }
    }
}
