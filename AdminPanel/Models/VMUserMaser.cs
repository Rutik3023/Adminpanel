using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections;

namespace  AdminPanel.Models
{
    public class VMUserMaser
    {
        
        public int Code { get; set; }

      
        public string OrgCode { get; set; }

   
        public string UserID { get; set; }

        public string UserName { get; set; }


        public string EmpCode { get; set; }
         public string FullName { get; set; }


        public string MobileNo { get; set; }

     
        public string EmailID { get; set; }

      
        public string UserLoginType { get; set; }

      
        public string UserTypeCode { get; set; }

       
        public bool? IsAdmin { get; set; }

       
        public int? LoginAttempts { get; set; } = 0;

        public bool? UserLockedStatus { get; set; }

         public DateTime? LastLoginDate { get; set; }

      
        public string Password { get; set; }

      
        public bool? IsADUser { get; set; } 
       
        public byte[] ImageName { get; set; }

         public int? ExperienceLevel { get; set; }
        public int? MultipleDevice { get; set; }

        public string? SecurityQuestion { get; set; }

      
        public string SecurityAnswer { get; set; }

        public bool? IsAdministrator { get; set; }




        public bool? IsEdit { get; set; } 

    
        public bool? IsDelete { get; set; } 

  
        public string? Status { get; set; }

        public DateTime CreationDate { get; set; }



    }

   
  
}





    

