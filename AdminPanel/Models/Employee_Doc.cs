namespace  AdminPanel.Models
{
    public class Employee_Doc
    {
        public string sys_code { get; set; }
        public string OrgCode { get; set; }
        public string EmpCode { get; set; }
        public byte[] Document { get; set; }
        public string document_Name { get; set; }
        public string Document_Type { get; set; }

        public string empdocumentBase64{ get; set; }
        //public string DocumentBase64 { get; set; }
        public string Sys_Created_By { get; set; }
        public string Sys_Created_On { get; set; }
        public string Sys_Modified_By { get; set; }
        public string Sys_Modified_On { get; set; }
    }

}
