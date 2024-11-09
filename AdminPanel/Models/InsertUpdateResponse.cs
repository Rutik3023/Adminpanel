namespace  AdminPanel.Models
{
    public class InsertUpdateResponse
    {


        public List<Employee_Signature> SignaturesList { get; set; }
        public int ResponseCode { get; set; }

        public InsertUpdateResponse()
        {
            SignaturesList = new List<Employee_Signature>();

        }
    }
}
