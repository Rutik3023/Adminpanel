namespace AdminPanel.Models 
{ 
    public class Employee_GL_Account_Map
    {
        public string sys_Code_Gl { get; set; } // UDD_EMP_GL_ACT_MAPING_Sys_Code
        public string OrgCode { get; set; } // UDD_OrgCode
        public string EmpCode { get; set; } // UDD_Emp_Sys_Code
        public string BranchLocationCode { get; set; } // UDD_BRNSysCode
        public string GLAccountCode { get; set; } // UDD, assuming it to be string as data type is not specified
        public string CurrencyCode { get; set; } // UDD_CURRENCY_Sys_Code
        public string Status { get; set; } // Status
        public string Note { get; set; } // Note
        public string Sys_Created_By { get; set; } // UDD_UserID
        public string Sys_Created_On { get; set; } // Sys_Created_On
        public string Sys_Modified_By { get; set; } // UDD_UserID
        public string Sys_Modified_On { get; set; } // Sys_Modified_On
    }
}
