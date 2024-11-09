using System.ComponentModel.DataAnnotations;

namespace  AdminPanel.Models
{
    public class VMRoleMaser
    {
        public string Code { get; set; }
     
        public string OrgCode { get; set; }

        public DateTime? creationdate { get; set; }
        public string RoleName { get; set; }

        public string RoleDescription { get; set; }
        public string Status { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string LastModificationDate { get; set; }

        public string LastModifiedBy { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "You need to agree to the Terms and Conditions")]
        public bool isLocked { get; set; }
        public bool isChecker { get; set; }

        //[Range(typeof(bool), "true", "true", ErrorMessage = "You need to agree to the Terms and Conditions")]
        //public bool isNotLocked { get; set; }

    }
}
