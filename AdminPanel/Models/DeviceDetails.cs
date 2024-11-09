namespace  AdminPanel.Models
{
    public class DeviceDetails
    {
        public string? LogedinUser { get; set; }
        public string? Sys_Code { get; set; }
        public string? UserCode { get; set; }
        public string? BiosSerialNo { get; set; }
        public string? MotherboardSerialNo { get; set; }
        public string? AuthCode { get; set; }
        public string? Device_Flag { get; set; }
        public string? Sys_Created_By { get; set; }
        public DateTime Sys_Created_On { get; set; }
        public string? DeviceUserName { get; set; }

        public bool? DeviceFlagBool { get; set; }
        public string? OrgCode { get; set; }
        public string? OrgName { get; set; }

        public string? Deviceuniquecode { get; set; }


        public string? OS { get; set; }

        public string? Browser { get; set; }

        public string? DuniqueIdentifier { get; set; }

        public string? Location { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
