namespace  AdminPanel.Models
{
    public class FamilyDetails
    {
        public string? sys_code { get; set; } // UDD_Emp_Sys_Code primary key
        public string? Family_Sys_Code { get; set; } // UDD_Emp_Sys_Code primary key
        public string? Relationship_Type { get; set; } // VARCHAR(50)
        public string? Relationship_Name { get; set; } // VARCHAR(50)
        public string? Relationship_Date_Of_Birth { get; set; } // DATE
    }
}
