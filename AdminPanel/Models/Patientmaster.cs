namespace AdminPanel.Models
{
    public class Patientmaster
    {
        public int syscode { get; set; }

        public string orgcode { get; set; }
        public String Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string AdhaarNo { get; set; }
        public int BloodGroup { get; set; }
        public char Gender { get; set; }
       
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }




    }
}
