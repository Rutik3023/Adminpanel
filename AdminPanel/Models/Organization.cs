using System.ComponentModel.DataAnnotations;

namespace  AdminPanel.Models
{
    public class Organization_model
    {
        public string? Sys_Code { get; set; }

        public string? Organization_Name { get; set; }

        public string? Organization_Code { get; set; }

        public string? Address { get; set; }

        public string? CityId { get; set; }

        public string? StateId { get; set; }

        public string? CountryId { get; set; }
        public string? CurrencyId { get; set; }

        public string? PinCode { get; set; }

        public string? Email { get; set; }

        public string? Telephone_No { get; set; }

        public string? Contact_Person_Name { get; set; }

        public string? Contact_Person_No { get; set; }

        public string? GSTNO { get; set; }

        public string? PANNO { get; set; }

        public string? Password { get; set; }

        public byte[]? Logo { get; set; }



        [Range(typeof(bool), "true", "true", ErrorMessage = "You need to agree to the Terms and Conditions")]
        public bool is_Ocean { get; set; }


        [Range(typeof(bool), "true", "true", ErrorMessage = "You need to agree to the Terms and Conditions")]
        public bool is_Land { get; set; }


        [Range(typeof(bool), "true", "true", ErrorMessage = "You need to agree to the Terms and Conditions")]
        public bool is_Air { get; set; }

        public string Status { get; set; }
        public string? ModelId { get; set; }
        public string Note { get; set; }

        public string Sys_Created_On { get; set; }
        public string Sys_Created_By { get; set; }
        public string sys_Updated_On { get; set; }
        public string sys_Updated_By { get; set; }
        public string? LogoImg { get; set; }
        public string? MimeType { get; set; }
    }
}
