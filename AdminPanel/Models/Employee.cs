namespace  AdminPanel.Models
{ 
    public class Employee
    {
        public string? Sys_Code { get; set; } // UDD_Emp_Sys_Code primary key
        public string? EmployeeCode { get; set; } // UDD_EmployeeCode
        public string? OrgCode { get; set; } // UDD_OrgCode
        public string? First_Name { get; set; } // VARCHAR(30) NOT NULL
        public string? Middle_Name { get; set; } // VARCHAR(30)
        public string? Last_Name { get; set; } // VARCHAR(40) NOT NULL

        public string? Full_Name { get; set; } // VARCHAR(40) NOT NULL
        public string? Alice_Name { get; set; } // VARCHAR(40) NOT NULL
        public string? Emp_Status { get; set; } // VARCHAR(15) NOT NULL
        public string? Salesmen { get; set; } // VARCHAR(3) NOT NULL
        public string? Co_Email_ID { get; set; } // VARCHAR(100) NOT NULL
        public string? Mobile_No { get; set; } // INT NOT NULL
        public string? Reason_for_Resignation { get; set; } // VARCHAR(50)
        public string? Emp_photoCode { get; set; } // UDD_EMP_PHOTO_SIGNATURE_SYS_CODE
        public string? Status { get; set; } // CHAR(1) NOT NULL
        public string? Date_of_Joining { get; set; } // DATE
        public string? Probation_Period { get; set; } // VARCHAR(3)
        public string? Professional_Status { get; set; } // VARCHAR(15)
        public string? National_ID { get; set; } // VARCHAR(50)
        public string? Transfer_Date { get; set; } // DATE
        public string? Employment_End_Date { get; set; } // DATE
        public string? BranchCode { get; set; } // UDD_BRNSysCod
        public string? DepartmentCode { get; set; } // UDD_DEPCode
        public string? Department_Head_Code { get; set; } // UDD_Emp_Sys_Code
        public string? Reporting_To_Code { get; set; } // UDD_Emp_Sys_Code
        public string? Designation_Start_Date { get; set; }
        public string? DesignationCode { get; set; } // UDD_DESGCode
        public string? Designation_Qualifier { get; set; } // VARCHAR(100)
        public string? Job_Description { get; set; } // VARCHAR(2000)
        public string? Payroll_Employee_ID { get; set; } // VARCHAR(50)
        public string? Date_Of_Birth { get; set; } // DATE NOT NULL
        public string? Aadhar_No { get; set; } // VARCHAR(12) NOT NULL
        public string? PAN_No { get; set; } // VARCHAR(10) NOT NULL
        public string? Gender { get; set; } // VARCHAR(10) NOT NULL
        public string? UAN_No { get; set; } // VARCHAR(50)
        public string? Nationality { get; set; } // VARCHAR(50) NOT NULL
        public string? Religion { get; set; } // VARCHAR(50)
        public string? Marital_Status { get; set; } // VARCHAR(10)
        public string? Wedding_Annive { get; set; } // DATE
        public string? Office_Phone_No { get; set; } // VARCHAR(15)
        public string? Personal_Email_ID { get; set; } // VARCHAR(100) NOT NULL
        public string? Personal_Mobile_No { get; set; } // VARCHAR(10) NOT NULL
        public string? Home_Town { get; set; } // VARCHAR(50)
        public string? Airport_Name { get; set; } // VARCHAR(100)
        public string? Personal_Address { get; set; } // VARCHAR(500) NOT NULL
        public string? Phone_Number { get; set; } // VARCHAR(30)
        public string? Contact_Address { get; set; } // VARCHAR(500)
        public string? Emergency_Contact_Person { get; set; } // VARCHAR(100) NOT NULL
        public string? Emergency_Contact_Address { get; set; } // VARCHAR(500)
        public string? Emergency_Contact_No { get; set; } // VARCHAR(30) NOT NULL
        public string? Bank_AC_No { get; set; } // VARCHAR(50) NOT NULL
        public string? IFSC_Code { get; set; } // VARCHAR(11) NOT NULL
        public string? Bank_Name { get; set; } // VARCHAR(100) NOT NULL
        public string? Bank_Address { get; set; } // VARCHAR(500)
       
        public string? Sys_Created_By { get; set; } // UDD_UserID
        public string? Sys_Created_On { get; set; } // DATETIME
        public string? Sys_Modified_By { get; set; } // UDD_UserID
        public string? Sys_Modified_On { get; set; } // DATETIME

        public byte[] Photo { get; set; } 
    }
}
