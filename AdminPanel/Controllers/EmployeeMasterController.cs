


using AdminPanel.CommonRepo;
using AdminPanel.Models;
using AdminPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AdminPanel.Controllers
{
   
    [Authorize]
    [AuthorizeUser]
    public class EmployeeMasterController : Controller
    {
        IGeneral general;
        IEmployee Employee;
        IUserMaster userMaster;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmployeeMasterController(IEmployee _Employee, IGeneral _general, IUserMaster userMaster, IHttpContextAccessor httpContextAccessor)
        {
            Employee = _Employee;
            general = _general;
            this.userMaster = userMaster;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        public IActionResult Employee_Form(string Id = "")
        {   // Master Form
            try
            {
                Employee d= new Employee(); 
                ViewBag.YesNoDropDown = CommonDropdown.GetYesNoDropdown();
                ViewBag.Status = CommonDropdown.GetStatusDropdown();
                ViewBag.EmpStatus = CommonDropdown.Get_EmployeeStatusDropdown();
                //Tabs details
                ViewBag.BranchLocation = Employee.Get_Branch_Location_Drop();
                ViewBag.Department = Employee.Get_Department_Drop();
                ViewBag.Reporting_To = Employee.Get_Reportin_To_Drop();
                ViewBag.Department_Head = Employee.Get_Department_Head_Code_Drop();
                ViewBag.Designation = Employee.Get_Designation_Drop();
                ViewBag.Currency = Employee.Get_CurrencyList_For_Cureency_Drop();
                // Personal
                ViewBag.NationalityDrop = Employee.Get_CountryList_For_Nationality_Drop(2);
                ViewBag.ORG = userMaster.GetOrganizationDropDownbyorgid();

                if (Id != "")
                {
                    Employee obj = Employee.GetEmployeeById(Id);

                    var photoBase64 = Convert.ToBase64String(obj.Photo);
                    var photoBase64Url = $"data:{"data:image/png"};base64,{photoBase64}";
                    ViewBag.EmpPhoto = photoBase64Url;
                    //GlAccount Details
                    ViewBag.GetGlMappingDetails = Employee.GetEmployee_GL_MappingBy_EmpCode(Id);

                    // Family Details 
                    ViewBag.GetFamilyDetails = Employee.Get_Family_Details_By_Emp_SysCode(Id);

                    //Employee Documents 
                    ViewBag.GetEmpDocDetails = Employee.GetEmp_Documents_By_EmpCode(Id);

                    return View(obj);
                }
                var user = _httpContextAccessor.HttpContext.User;

                string orgCode = user.FindFirst("OrgCode")?.Value;
                if (orgCode == "" || orgCode == null)
                {
                    return View();
                }
                d.OrgCode = orgCode;
                return View(d);

            }
            catch (Exception ex)
            {

                general.Write_Log_Into_Txt(ex.Message, "EmployeeMasterController");
                general.InsertExcepstionlog(ex.Message, "EmployeeMasterController");
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult Employee_Form(Employee obj, IFormFile Emp_photoCode)
        {
            try
            {
                ViewBag.ORG = userMaster.GetOrganizationDropDownbyorgid();
                ViewBag.YesNoDropDown = CommonDropdown.GetYesNoDropdown();
                ViewBag.Status = CommonDropdown.GetStatusDropdown();
                ViewBag.EmpStatus = CommonDropdown.Get_EmployeeStatusDropdown();
                //Tabs details
                ViewBag.BranchLocation = Employee.Get_Branch_Location_Drop();
                ViewBag.Department = Employee.Get_Department_Drop();
                ViewBag.Reporting_To = Employee.Get_Reportin_To_Drop();
                ViewBag.Department_Head = Employee.Get_Department_Head_Code_Drop();
                ViewBag.Currency = Employee.Get_CurrencyList_For_Cureency_Drop();
                ViewBag.Designation = Employee.Get_Designation_Drop();
                ViewBag.NationalityDrop = Employee.Get_CountryList_For_Nationality_Drop(2);
                ResponceMessage RespMsg = Employee.InsertUpadte_Employee(obj, Emp_photoCode);
                if (RespMsg==null)
                {
                    Employee d= new Employee();
                    var user = _httpContextAccessor.HttpContext.User;

                    string orgCode = user.FindFirst("OrgCode")?.Value;
                    if (orgCode == "" || orgCode == null)
                    {
                        return View();
                    }
                    d.OrgCode = orgCode;
                    return View(d);

                    
                }
                if(RespMsg.code==0)
                {
                    TempData["Output"] = "Record submitted successfully.";
                }
                if (RespMsg.code == 1)
                {
                    TempData["Output"] = "Record updated successfully";
                }
                if (RespMsg.code == 2)
                {
                    TempData["Output"] = "Co-Email id already Exits";
                    return View(obj);
                }
                else if (RespMsg.code == -1)
                {
                    TempData["Output"] = "Something went wrong. Please try again.";
                }

                var photo = RespMsg.Message.Photo;
               // int EmpId = Convert.ToInt32( RespMsg.Message.Sys_Code);
                //GlAccount Details
                ViewBag.GetGlMappingDetails = Employee.GetEmployee_GL_MappingBy_EmpCode(RespMsg.Message.Sys_Code);

                // Family Details 
                ViewBag.GetFamilyDetails = Employee.Get_Family_Details_By_Emp_SysCode(RespMsg.Message.Sys_Code);

                //Employee Documents 
                ViewBag.GetEmpDocDetails = Employee.GetEmp_Documents_By_EmpCode(RespMsg.Message.Sys_Code);
                if (photo != null && photo.Length > 0)
                {
                    
                        var photoBase64 = Convert.ToBase64String(photo);
                        var photoBase64Url = $"data:{"data:image/png"};base64,{photoBase64}";
                        ViewBag.EmpPhoto = photoBase64Url;
                    
                }
                else
                {
                    ViewBag.EmpPhoto = null;
                }
                return View(RespMsg.Message);

            }
            catch (Exception ex)
            {

                general.Write_Log_Into_Txt(ex.Message, "EmployeeMasterController");
                general.InsertExcepstionlog(ex.Message, "EmployeeMasterController");
                return View("Error");
            }

        }


        [HttpGet]
        public IActionResult Employee_List()
        {
            try
            {
                ViewBag.EmployeeList = Employee.GetEmployeeList();
                return View();
            }
            catch (Exception ex)
            {

                general.Write_Log_Into_Txt(ex.Message, "EmployeeMasterController");
                general.InsertExcepstionlog(ex.Message, "EmployeeMasterController");
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                bool respo = Employee.Delete(id);
                if (respo)
                {
                    TempData["Output"] = "Deleted successfully.";
                }

                return Json(respo);
            }
            catch (Exception er)
            {
                general.Write_Log_Into_Txt(er.Message, "EmployeeMasterController");
                general.InsertExcepstionlog(er.Message, "EmployeeMasterController");
                return View("Error");
            }

        }

        [HttpGet]
        public IActionResult Employee_Professional()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Employee_Personal()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Employee_Accounts()
        {
            try
            {
                ViewBag.Status = CommonDropdown.GetStatusDropdown();
                ViewBag.BranchLocation = Employee.Get_Branch_Location_Drop();

                return View();
            }
            catch (Exception ex)
            {

                general.Write_Log_Into_Txt(ex.Message, "EmployeeMasterController");
                general.InsertExcepstionlog(ex.Message, "EmployeeMasterController");
                return View("Error");
            }
        }


        //Employee Documents
        #region Employee Documents
        [HttpGet]
        public IActionResult Employee_Documents()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Employee_Documents(IFormFile Emp_documents1, string document_type, string Sys_Code_Doc, string Sys_Code)
        {
            if (Emp_documents1 != null)
            {
                List<Employee_Doc> EmpDocList = Employee.InsertUpadte_Emp_Documents(Emp_documents1, document_type, Sys_Code_Doc, Sys_Code);
                return Json(EmpDocList);
            }

            return View();
        }

        public IActionResult Employee_Documents_Check(string Sys_Code, string document_type)
        {
            if (Sys_Code != null && document_type != null)
            {
                bool response = Employee.Check_DocumentType(Sys_Code, document_type);
                return Json(response);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Delete_Employee_Documents(string Sys_Code)
        {
            if (Sys_Code != null)
            {
                List<Employee_Doc> response = Employee.Delete_Emp_Documents(Sys_Code);
                return Json(response);
            }

            return View();
        } 
        #endregion

        
        //Emp Family Details 
        #region Employee Family Details
        [HttpGet]
        public IActionResult Employee_Family_Details()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Edit_Employee_Family_Details(int FamilySysCode)
        {

            if (FamilySysCode > 0)
            {
                var FamModel = Employee.Get_Family_Details_By_Fam_SysCode(FamilySysCode);
                return Json(FamModel);
            }
            return Ok("No record found");
        }

        [HttpPost]
        public IActionResult SaveFamilyDetails([FromBody] FamilyDetails obj)
        {
            var resp = Employee.Insert_Family_Details(obj);
            return Json(resp);
        }


        [HttpGet]
        public IActionResult Delete_FamilyDetails(string Sys_Code)
        {
            if (Sys_Code!=null)
            {
                var resp = Employee.Delete_Emp_familyDetails(Sys_Code);
                return Json(resp);
            }
            return Json(null);
        }
        #endregion

        #region Employee Gl Account Mapping
        [HttpPost]
        public IActionResult Save_GL_Mapping([FromBody] Employee_GL_Account_Map obj)
        {
            if (obj != null)
            {
                List<Employee_GL_Account_Map> data = Employee.InsertUpadte_GL_Mapping_Get_GL(obj);

                return Json(data);
            }

            return Json(null);
        }

        [HttpGet]
        public IActionResult Get_Employee_GL_Mapping(int Id)
        {
            if (Id > 0)
            {
                var resp = Employee.GetEmployee_GL_MappingById(Id);
                return Json(resp);
            }
            else
            {
                return Json(null);
            }
        }
        #endregion

        //Employee Beneficiary Details

        [HttpGet]
        public IActionResult Employee_Beneficiary_Details()
        {
            return View();
        }




        [HttpGet]
        public IActionResult Get_Dept_Head(string syscode)
        {
            var resp = Employee.Get_Department_Head_By_Dept_Id(syscode);
            return Json(resp);
        }

        [HttpPost]
        public IActionResult SaveData([FromBody] Employee obj)
        {
            var resp = Employee.Update_Emp_General_Details(obj);
            return Json(resp);
        }

        [HttpGet]
        public IActionResult Employee_Access_Details()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Employee_Entitlement_Details()
        {
            return View();
        }







    }
}