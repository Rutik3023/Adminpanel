using AdminPanel.CommonRepo;
using  AdminPanel.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;

namespace AdminPanel.Services
{
    public interface IEmployee
    {   //Employee Main Form
        public ResponceMessage InsertUpadte_Employee(Employee obj, IFormFile Document_);

        public List<Employee> GetEmployeeList();

        public Employee GetEmployeeById(string EmpCode);

        public bool Delete(int Code);

        //Tabs Details Form
        public List<SelectListItem> Get_Branch_Location_Drop();

        public List<SelectListItem> Get_Department_Drop();

        public List<SelectListItem> Get_Designation_Drop();

        public List<SelectListItem> Get_Reportin_To_Drop();

        public DepartmentMaster Get_Department_Head_By_Dept_Id(string syscode);

        public List<SelectListItem> Get_Department_Head_Code_Drop();

        public Employee Update_Emp_General_Details(Employee obj);

        public List<SelectListItem> Get_CountryList_For_Nationality_Drop(int Id);

        public List<SelectListItem> Get_CurrencyList_For_Cureency_Drop();


        //Gl Account Mapping
        public Employee_GL_Account_Map GetEmployee_GL_MappingById(int Id);
        public List<Employee_GL_Account_Map> InsertUpadte_GL_Mapping_Get_GL(Employee_GL_Account_Map obj);
        public List<Employee_GL_Account_Map> GetEmployee_GL_MappingBy_EmpCode(string Id);


        //Employee Documents
        public List<Employee_Doc> InsertUpadte_Emp_Documents(IFormFile Emp_documents, string document_type, string Sys_Code_Doc, string Emp_SysCode);
        public List<Employee_Doc> GetEmp_Documents_By_EmpCode(string Id);
        public bool Check_DocumentType(string SysCode, string DocumentType);
        public List<Employee_Doc> Delete_Emp_Documents(string Code);


        //Family Details

        public List<FamilyDetails> Insert_Family_Details(FamilyDetails obj);


        public List<FamilyDetails> Get_Family_Details_By_Emp_SysCode(string empcode);

        public List<FamilyDetails> Get_Family_Details_By_Fam_SysCode(int FamSysCode);

        public List<FamilyDetails> Delete_Emp_familyDetails(string Code);

        //Employee Signature
        public InsertUpdateResponse InsertUpdate_EmployeeSign(Employee_Signature Code);
        public List<Employee_Signature> Get_All_EmployeeSign();

        public List<Employee_Signature> Delete_Emp_Signature(string Code);
        public Employee_Signature Get__EmployeeSignBy_SysCode(string SysCode);
    }


    public class Employee_Services : IEmployee
    {
        IConfiguration configuration;
        IGeneral general;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Employee_Services(IConfiguration _configuration, IHttpContextAccessor httpContextAccessor, IGeneral _general)
        {
            configuration = _configuration;
            general = _general;
            _httpContextAccessor = httpContextAccessor;
        }


        //Employee Master Form
        #region Functiom To InsertUpadte Employee Master Details
        public ResponceMessage InsertUpadte_Employee(Employee obj, IFormFile Document_)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                var user = _httpContextAccessor.HttpContext.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }

                byte[] logoBytes = null;
                string PhotoName = "";
                if (Document_ != null && Document_.Length > 0)
                {
                    // Read the file into a byte array

                    using (var memoryStream = new MemoryStream())
                    {
                        Document_.CopyTo(memoryStream);
                        logoBytes = memoryStream.ToArray();
                    }

                    PhotoName = Path.GetFileName(Document_.FileName);


                }

                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);
                string sql = "USP_HRM_INSERTUPDATE_EMPLOYEEDETAILS";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                SqlParameter P_SYSCODE = new SqlParameter();
                P_SYSCODE.ParameterName = "P_SYSCODE";
                P_SYSCODE.SqlDbType = SqlDbType.Int;
                P_SYSCODE.Size = 20;
                P_SYSCODE.Value = obj.Sys_Code ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_SYSCODE);


                SqlParameter P_FIRST_NAME = new SqlParameter();
                P_FIRST_NAME.ParameterName = "P_FIRST_NAME";
                P_FIRST_NAME.SqlDbType = SqlDbType.VarChar;
                P_FIRST_NAME.Size = 50;
                P_FIRST_NAME.Value = obj.First_Name;
                db.cmd.Parameters.Add(P_FIRST_NAME);


                SqlParameter P_MIDDLE_NAME = new SqlParameter();
                P_MIDDLE_NAME.ParameterName = "P_MIDDLE_NAME";
                P_MIDDLE_NAME.SqlDbType = SqlDbType.VarChar;
                P_MIDDLE_NAME.Size = 50;
                P_MIDDLE_NAME.Value = obj.Middle_Name;
                db.cmd.Parameters.Add(P_MIDDLE_NAME);

                SqlParameter P_LAST_NAME = new SqlParameter();
                P_LAST_NAME.ParameterName = "P_LAST_NAME";
                P_LAST_NAME.SqlDbType = SqlDbType.VarChar;
                P_LAST_NAME.Size = 500;
                P_LAST_NAME.Value = obj.Last_Name;
                db.cmd.Parameters.Add(P_LAST_NAME);

                SqlParameter P_EMP_STATUS = new SqlParameter();
                P_EMP_STATUS.ParameterName = "P_EMP_STATUS";
                P_EMP_STATUS.SqlDbType = SqlDbType.VarChar;
                P_EMP_STATUS.Size = 50;
                P_EMP_STATUS.Value = obj.Emp_Status;
                db.cmd.Parameters.Add(P_EMP_STATUS);

                SqlParameter P_CO_EMAIL_ID = new SqlParameter();
                P_CO_EMAIL_ID.ParameterName = "P_CO_EMAIL_ID";
                P_CO_EMAIL_ID.SqlDbType = SqlDbType.VarChar;
                P_CO_EMAIL_ID.Size = 50;
                P_CO_EMAIL_ID.Value = obj.Co_Email_ID ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_CO_EMAIL_ID);

                SqlParameter P_MOBILE_NO = new SqlParameter();
                P_MOBILE_NO.ParameterName = "P_MOBILE_NO";
                P_MOBILE_NO.SqlDbType = SqlDbType.VarChar;
                P_MOBILE_NO.Size = 10;
                P_MOBILE_NO.Value = obj.Mobile_No ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_MOBILE_NO);

                SqlParameter P_RESIGNATION_REASON = new SqlParameter();
                P_RESIGNATION_REASON.ParameterName = "P_RESIGNATION_REASON";
                P_RESIGNATION_REASON.SqlDbType = SqlDbType.VarChar;
                P_RESIGNATION_REASON.Size = 4000;
                P_RESIGNATION_REASON.Value = obj.Reason_for_Resignation ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_RESIGNATION_REASON);

                SqlParameter P_SALESMAN = new SqlParameter();
                P_SALESMAN.ParameterName = "P_SALESMAN";
                P_SALESMAN.SqlDbType = SqlDbType.VarChar;
                P_SALESMAN.Size = 50;
                P_SALESMAN.Value = obj.Salesmen;
                db.cmd.Parameters.Add(P_SALESMAN);

                SqlParameter P_STATUS = new SqlParameter();
                P_STATUS.ParameterName = "P_Status";
                P_STATUS.SqlDbType = SqlDbType.Char;
                P_STATUS.Size = 1;
                P_STATUS.Value = obj.Status;
                db.cmd.Parameters.Add(P_STATUS);

                SqlParameter P_EMP_PHOTO = new SqlParameter();
                P_EMP_PHOTO.ParameterName = "P_EMP_PHOTO";
                P_EMP_PHOTO.SqlDbType = SqlDbType.VarBinary;
                P_EMP_PHOTO.Size = -1;
                P_EMP_PHOTO.Value = logoBytes ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_EMP_PHOTO);

                SqlParameter P_EMP_PHOTONAME = new SqlParameter();
                P_EMP_PHOTONAME.ParameterName = "P_EMP_PHOTONAME";
                P_EMP_PHOTONAME.SqlDbType = SqlDbType.VarChar;
                P_EMP_PHOTONAME.Size = 200;
                P_EMP_PHOTONAME.Value = PhotoName ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_EMP_PHOTONAME);

                SqlParameter P_ORGCODE = new SqlParameter();
                P_ORGCODE.ParameterName = "P_ORGCODE";
                P_ORGCODE.SqlDbType = SqlDbType.VarChar;
                P_ORGCODE.Size = 20;
                P_ORGCODE.Value = obj.OrgCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_ORGCODE);

                SqlParameter P_USERID = new SqlParameter();
                P_USERID.ParameterName = "P_USERID";
                P_USERID.SqlDbType = SqlDbType.VarChar;
                P_USERID.Size = 200;
                P_USERID.Value = userId;
                db.cmd.Parameters.Add(P_USERID);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_ID = new SqlParameter();
                P_ID.ParameterName = "P_ID";
                P_ID.SqlDbType = SqlDbType.Int;
                P_ID.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_ID);

                SqlParameter P_MESSAGE = new SqlParameter();
                P_MESSAGE.ParameterName = "P_MESSAGE";
                P_MESSAGE.SqlDbType = SqlDbType.VarChar;
                P_MESSAGE.Size = 200;
                P_MESSAGE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_MESSAGE);

                var dr = db.DataReader(sql);

                Employee data = new Employee();
                while (dr.Read())
                {
                    data.Sys_Code = dr["Sys_Code"].ToString();
                    data.EmployeeCode = dr["Code"].ToString();
                    data.OrgCode = dr["OrgCode"] != DBNull.Value ? dr["OrgCode"].ToString() : null;
                    data.First_Name = dr["First_Name"].ToString();
                    data.Middle_Name = dr["Middle_Name"].ToString();
                    data.Last_Name = dr["Last_Name"].ToString();
                    data.Emp_Status = dr["Employment_Status"].ToString();
                    data.Salesmen = dr["Salesmen"].ToString();
                    data.Co_Email_ID = dr["Co_Email_ID"].ToString();
                    data.Mobile_No = dr["Mobile_No"].ToString();
                    data.Reason_for_Resignation = dr["Reason_for_Resignation"] != DBNull.Value ? dr["Reason_for_Resignation"].ToString() : null;
                    data.Photo = dr["Photo"] as byte[];
                    data.Status = dr["Status"].ToString();
                    // Additional fields from the query
                    //string Doj = dr["Date_of_Joining"] != DBNull.Value ? dr["Date_of_Joining"].ToString() : null;
                    data.Date_of_Joining = dr["Date_of_Joining"] != DBNull.Value ? dr["Date_of_Joining"].ToString() : null;
                    data.Probation_Period = dr["Probation_Period"] != DBNull.Value ? dr["Probation_Period"].ToString() : null;
                    data.Professional_Status = dr["Professional_Status"] != DBNull.Value ? dr["Professional_Status"].ToString() : null;
                    data.National_ID = dr["National_ID"] != DBNull.Value ? dr["National_ID"].ToString() : null;
                    data.Transfer_Date = dr["Transfer_Date"] != DBNull.Value ? dr["Transfer_Date"].ToString() : null;
                    data.Employment_End_Date = dr["Employment_End_Date"] != DBNull.Value ? dr["Employment_End_Date"].ToString() : null;
                    data.BranchCode = dr["BranchCode"] != DBNull.Value ? dr["BranchCode"].ToString() : null;
                    data.DepartmentCode = dr["DepartmentCode"] != DBNull.Value ? dr["DepartmentCode"].ToString() : null;
                    data.Department_Head_Code = dr["Department_Head_Code"] != DBNull.Value ? dr["Department_Head_Code"].ToString() : null;
                    data.Reporting_To_Code = dr["Reporting_To_Code"] != DBNull.Value ? dr["Reporting_To_Code"].ToString() : null;
                    data.Designation_Start_Date = dr["Designation_Start_Date"] != DBNull.Value ? dr["Designation_Start_Date"].ToString() : null;
                    data.DesignationCode = dr["DesignationCode"] != DBNull.Value ? dr["DesignationCode"].ToString() : null;
                    data.Designation_Qualifier = dr["Designation_Qualifier"] != DBNull.Value ? dr["Designation_Qualifier"].ToString() : null;
                    data.Job_Description = dr["Job_Description"] != DBNull.Value ? dr["Job_Description"].ToString() : null;
                    data.Payroll_Employee_ID = dr["Payroll_Employee_ID"] != DBNull.Value ? dr["Payroll_Employee_ID"].ToString() : null;
                    data.Date_Of_Birth = dr["Date_Of_Birth"] != DBNull.Value ? dr["Date_Of_Birth"].ToString() : null;
                    data.PAN_No = dr["PAN_No"] != DBNull.Value ? dr["PAN_No"].ToString() : null;
                    data.Aadhar_No = dr["Aadhar_No"] != DBNull.Value ? dr["Aadhar_No"].ToString() : null;
                    data.Gender = dr["Gender"] != DBNull.Value ? dr["Gender"].ToString() : null;
                    data.UAN_No = dr["UAN_No"] != DBNull.Value ? dr["UAN_No"].ToString() : null;
                    data.Nationality = dr["Nationality"] != DBNull.Value ? dr["Nationality"].ToString() : null;
                    data.Religion = dr["Religion"] != DBNull.Value ? dr["Religion"].ToString() : null;
                    data.Marital_Status = dr["Marital_Status"] != DBNull.Value ? dr["Marital_Status"].ToString() : null;
                    data.Wedding_Annive = dr["Wedding_Annive"] != DBNull.Value ? dr["Wedding_Annive"].ToString() : null;
                    data.Office_Phone_No = dr["Office_Phone_No"] != DBNull.Value ? dr["Office_Phone_No"].ToString() : null;
                    data.Personal_Email_ID = dr["Personal_Email_ID"].ToString();
                    data.Personal_Mobile_No = dr["Personal_Mobile_No"].ToString();
                    data.Home_Town = dr["Home_Town"] != DBNull.Value ? dr["Home_Town"].ToString() : null;
                    data.Airport_Name = dr["Airport_Name"] != DBNull.Value ? dr["Airport_Name"].ToString() : null;
                    data.Personal_Address = dr["Personal_Address"].ToString();
                    data.Phone_Number = dr["Phone_Number"] != DBNull.Value ? dr["Phone_Number"].ToString() : null;
                    data.Contact_Address = dr["Contact_Address"] != DBNull.Value ? dr["Contact_Address"].ToString() : null;
                    data.Emergency_Contact_Person = dr["Emergency_Contact_Person"].ToString();
                    data.Emergency_Contact_Address = dr["Emergency_Contact_Address"] != DBNull.Value ? dr["Emergency_Contact_Address"].ToString() : null;
                    data.Emergency_Contact_No = dr["Emergency_Contact_No"].ToString();
                    data.Bank_AC_No = dr["Bank_AC_No"].ToString();
                    data.IFSC_Code = dr["IFSC_Code"].ToString();
                    data.Bank_Name = dr["Bank_Name"].ToString();
                    data.Bank_Address = dr["Bank_Address"] != DBNull.Value ? dr["Bank_Address"].ToString() : null;

                    data.Sys_Created_By = dr["Sys_Created_By"] != DBNull.Value ? dr["Sys_Created_By"].ToString() : null;
                    data.Sys_Created_On = (dr["Sys_Created_On"].ToString());
                    data.Sys_Modified_By = dr["Sys_Modified_By"] != DBNull.Value ? dr["Sys_Modified_By"].ToString() : null;
                    data.Sys_Modified_On = dr["Sys_Modified_On"] != DBNull.Value ? (dr["Sys_Modified_On"].ToString()) : null;
                }
                dr.Close();
                ResponceMessage OBJ = new ResponceMessage();

                string ResponseMsg = P_MESSAGE.Value.ToString();
                OBJ.Message = data;
                int RespCode = Convert.ToInt32(P_CODE.Value);
                OBJ.code = RespCode;


                db.CloseConnection();

                if (RespCode == 0 || RespCode == 1 || RespCode == 2)
                {
                    return OBJ;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region Functiom To Get  all Employee for grid
        public List<Employee> GetEmployeeList()
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                var user = _httpContextAccessor.HttpContext.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_GET_ALLEMP_BY_ORGCODE";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_ORGCODE = new SqlParameter();
                P_ORGCODE.ParameterName = "P_ORGCODE";
                P_ORGCODE.SqlDbType = SqlDbType.VarChar;
                P_ORGCODE.Size = 100;
                P_ORGCODE.Value = orgCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_ORGCODE);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);
                List<Employee> list = new List<Employee>();

                while (dr.Read())
                {
                    Employee data = new Employee();
                    data.Sys_Code = dr["Sys_Code"].ToString();
                    data.EmployeeCode = dr["Code"].ToString();
                    data.OrgCode = dr["OrgCode"] != DBNull.Value ? dr["OrgCode"].ToString() : null;
                    data.First_Name = dr["First_Name"].ToString();
                    data.Middle_Name = dr["Middle_Name"].ToString();
                    data.Last_Name = dr["Last_Name"].ToString();
                    data.Full_Name = dr["FullName"].ToString();
                    data.Emp_Status = dr["Employment_Status"].ToString();
                    data.Salesmen = dr["Salesmen"].ToString();
                    data.Co_Email_ID = dr["Co_Email_ID"].ToString();
                    data.Mobile_No = dr["Mobile_No"].ToString();
                    data.Reason_for_Resignation = dr["Reason_for_Resignation"] != DBNull.Value ? dr["Reason_for_Resignation"].ToString() : null;
                    data.Emp_photoCode = dr["Emp_photoCode"].ToString();
                    data.Status = dr["Status"].ToString();
                    // Additional fields from the query
                    //data.Date_of_Joining = dr["Date_of_Joining"] != DBNull.Value ? dr["Date_of_Joining"].ToString() : null;
                    data.Probation_Period = dr["Probation_Period"] != DBNull.Value ? dr["Probation_Period"].ToString() : null;
                    data.Professional_Status = dr["Professional_Status"] != DBNull.Value ? dr["Professional_Status"].ToString() : null;
                    data.National_ID = dr["National_ID"] != DBNull.Value ? dr["National_ID"].ToString() : null;
                    data.Transfer_Date = dr["Transfer_Date"] != DBNull.Value ? dr["Transfer_Date"].ToString() : null;
                    data.Employment_End_Date = dr["Employment_End_Date"] != DBNull.Value ? dr["Employment_End_Date"].ToString() : null;
                    data.BranchCode = dr["BranchCode"] != DBNull.Value ? dr["BranchCode"].ToString() : null;
                    data.DepartmentCode = dr["DepartmentCode"] != DBNull.Value ? dr["DepartmentCode"].ToString() : null;
                    data.Department_Head_Code = dr["Department_Head_Code"] != DBNull.Value ? dr["Department_Head_Code"].ToString() : null;
                    data.Reporting_To_Code = dr["Reporting_To_Code"] != DBNull.Value ? dr["Reporting_To_Code"].ToString() : null;
                    data.Designation_Start_Date = dr["Designation_Start_Date"] != DBNull.Value ? dr["Designation_Start_Date"].ToString() : null;
                    data.DesignationCode = dr["DesignationCode"] != DBNull.Value ? dr["DesignationCode"].ToString() : null;
                    data.Designation_Qualifier = dr["Designation_Qualifier"] != DBNull.Value ? dr["Designation_Qualifier"].ToString() : null;
                    data.Job_Description = dr["Job_Description"] != DBNull.Value ? dr["Job_Description"].ToString() : null;
                    data.Payroll_Employee_ID = dr["Payroll_Employee_ID"] != DBNull.Value ? dr["Payroll_Employee_ID"].ToString() : null;
                    data.Date_Of_Birth = dr["Date_Of_Birth"] != DBNull.Value ? dr["Date_Of_Birth"].ToString() : null;
                    data.PAN_No = dr["PAN_No"] != DBNull.Value ? dr["PAN_No"].ToString() : null;
                    data.Aadhar_No = dr["Aadhar_No"] != DBNull.Value ? dr["Aadhar_No"].ToString() : null;
                    data.Gender = dr["Gender"] != DBNull.Value ? dr["Gender"].ToString() : null;
                    data.UAN_No = dr["UAN_No"] != DBNull.Value ? dr["UAN_No"].ToString() : null;
                    data.Nationality = dr["Nationality"] != DBNull.Value ? dr["Nationality"].ToString() : null;
                    data.Religion = dr["Religion"] != DBNull.Value ? dr["Religion"].ToString() : null;
                    data.Marital_Status = dr["Marital_Status"] != DBNull.Value ? dr["Marital_Status"].ToString() : null;
                    data.Wedding_Annive = dr["Wedding_Annive"] != DBNull.Value ? dr["Wedding_Annive"].ToString() : null;
                    data.Office_Phone_No = dr["Office_Phone_No"] != DBNull.Value ? dr["Office_Phone_No"].ToString() : null;
                    data.Personal_Email_ID = dr["Personal_Email_ID"].ToString();
                    data.Personal_Mobile_No = dr["Personal_Mobile_No"].ToString();
                    data.Home_Town = dr["Home_Town"] != DBNull.Value ? dr["Home_Town"].ToString() : null;
                    data.Airport_Name = dr["Airport_Name"] != DBNull.Value ? dr["Airport_Name"].ToString() : null;
                    data.Personal_Address = dr["Personal_Address"].ToString();
                    data.Phone_Number = dr["Phone_Number"] != DBNull.Value ? dr["Phone_Number"].ToString() : null;
                    data.Contact_Address = dr["Contact_Address"] != DBNull.Value ? dr["Contact_Address"].ToString() : null;
                    data.Emergency_Contact_Person = dr["Emergency_Contact_Person"].ToString();
                    data.Emergency_Contact_Address = dr["Emergency_Contact_Address"] != DBNull.Value ? dr["Emergency_Contact_Address"].ToString() : null;
                    data.Emergency_Contact_No = dr["Emergency_Contact_No"].ToString();
                    data.Bank_AC_No = dr["Bank_AC_No"].ToString();
                    data.IFSC_Code = dr["IFSC_Code"].ToString();
                    data.Bank_Name = dr["Bank_Name"].ToString();
                    data.Bank_Address = dr["Bank_Address"] != DBNull.Value ? dr["Bank_Address"].ToString() : null;



                    list.Add(data);
                }

                dr.Close();
                db.CloseConnection();

                return list;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Functiom To Get Employee By Sys_code
        public Employee GetEmployeeById(string EmpCode)
        {
            try
            {
                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_HRM_GET_EMPLOYEEMASTER_BY_SYSCODE";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_ID = new SqlParameter();
                P_ID.ParameterName = "P_ID";
                P_ID.SqlDbType = SqlDbType.Int;
                P_ID.Value = EmpCode;
                db.cmd.Parameters.Add(P_ID);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);

                Employee data = new Employee();
                while (dr.Read())
                {
                    data.Sys_Code = dr["Sys_Code"].ToString();
                    data.EmployeeCode = dr["Code"].ToString();
                    data.OrgCode = dr["OrgCode"] != DBNull.Value ? dr["OrgCode"].ToString() : null;
                    data.First_Name = dr["First_Name"].ToString();
                    data.Middle_Name = dr["Middle_Name"].ToString();
                    data.Last_Name = dr["Last_Name"].ToString();
                    data.Emp_Status = dr["Employment_Status"].ToString();
                    data.Salesmen = dr["Salesmen"].ToString();
                    data.Co_Email_ID = dr["Co_Email_ID"].ToString();
                    data.Mobile_No = dr["Mobile_No"].ToString();
                    data.Reason_for_Resignation = dr["Reason_for_Resignation"] != DBNull.Value ? dr["Reason_for_Resignation"].ToString() : null;
                    data.Photo = dr["Photo"] as byte[];
                    data.Status = dr["Status"].ToString();
                    // Additional fields from the query
                    //string Doj = dr["Date_of_Joining"] != DBNull.Value ? dr["Date_of_Joining"].ToString() : null;
                    data.Date_of_Joining = dr["Date_of_Joining"] != DBNull.Value ? dr["Date_of_Joining"].ToString() : null;
                    data.Probation_Period = dr["Probation_Period"] != DBNull.Value ? dr["Probation_Period"].ToString() : null;
                    data.Professional_Status = dr["Professional_Status"] != DBNull.Value ? dr["Professional_Status"].ToString() : null;
                    data.National_ID = dr["National_ID"] != DBNull.Value ? dr["National_ID"].ToString() : null;
                    data.Transfer_Date = dr["Transfer_Date"] != DBNull.Value ? dr["Transfer_Date"].ToString() : null;
                    data.Employment_End_Date = dr["Employment_End_Date"] != DBNull.Value ? dr["Employment_End_Date"].ToString() : null;
                    data.BranchCode = dr["BranchCode"] != DBNull.Value ? dr["BranchCode"].ToString() : null;
                    data.DepartmentCode = dr["DepartmentCode"] != DBNull.Value ? dr["DepartmentCode"].ToString() : null;
                    data.Department_Head_Code = dr["Department_Head_Code"] != DBNull.Value ? dr["Department_Head_Code"].ToString() : null;
                    data.Reporting_To_Code = dr["Reporting_To_Code"] != DBNull.Value ? dr["Reporting_To_Code"].ToString() : null;
                    data.Designation_Start_Date = dr["Designation_Start_Date"] != DBNull.Value ? dr["Designation_Start_Date"].ToString() : null;
                    data.DesignationCode = dr["DesignationCode"] != DBNull.Value ? dr["DesignationCode"].ToString() : null;
                    data.Designation_Qualifier = dr["Designation_Qualifier"] != DBNull.Value ? dr["Designation_Qualifier"].ToString() : null;
                    data.Job_Description = dr["Job_Description"] != DBNull.Value ? dr["Job_Description"].ToString() : null;
                    data.Payroll_Employee_ID = dr["Payroll_Employee_ID"] != DBNull.Value ? dr["Payroll_Employee_ID"].ToString() : null;
                    data.Date_Of_Birth = dr["Date_Of_Birth"] != DBNull.Value ? dr["Date_Of_Birth"].ToString() : null;
                    data.PAN_No = dr["PAN_No"] != DBNull.Value ? dr["PAN_No"].ToString() : null;
                    data.Aadhar_No = dr["Aadhar_No"] != DBNull.Value ? dr["Aadhar_No"].ToString() : null;
                    data.Gender = dr["Gender"] != DBNull.Value ? dr["Gender"].ToString() : null;
                    data.UAN_No = dr["UAN_No"] != DBNull.Value ? dr["UAN_No"].ToString() : null;
                    data.Nationality = dr["Nationality"] != DBNull.Value ? dr["Nationality"].ToString() : null;
                    data.Religion = dr["Religion"] != DBNull.Value ? dr["Religion"].ToString() : null;
                    data.Marital_Status = dr["Marital_Status"] != DBNull.Value ? dr["Marital_Status"].ToString() : null;
                    data.Wedding_Annive = dr["Wedding_Annive"] != DBNull.Value ? dr["Wedding_Annive"].ToString() : null;
                    data.Office_Phone_No = dr["Office_Phone_No"] != DBNull.Value ? dr["Office_Phone_No"].ToString() : null;
                    data.Personal_Email_ID = dr["Personal_Email_ID"].ToString();
                    data.Personal_Mobile_No = dr["Personal_Mobile_No"].ToString();
                    data.Home_Town = dr["Home_Town"] != DBNull.Value ? dr["Home_Town"].ToString() : null;
                    data.Airport_Name = dr["Airport_Name"] != DBNull.Value ? dr["Airport_Name"].ToString() : null;
                    data.Personal_Address = dr["Personal_Address"].ToString();
                    data.Phone_Number = dr["Phone_Number"] != DBNull.Value ? dr["Phone_Number"].ToString() : null;
                    data.Contact_Address = dr["Contact_Address"] != DBNull.Value ? dr["Contact_Address"].ToString() : null;
                    data.Emergency_Contact_Person = dr["Emergency_Contact_Person"].ToString();
                    data.Emergency_Contact_Address = dr["Emergency_Contact_Address"] != DBNull.Value ? dr["Emergency_Contact_Address"].ToString() : null;
                    data.Emergency_Contact_No = dr["Emergency_Contact_No"].ToString();
                    data.Bank_AC_No = dr["Bank_AC_No"].ToString();
                    data.IFSC_Code = dr["IFSC_Code"].ToString();
                    data.Bank_Name = dr["Bank_Name"].ToString();
                    data.Bank_Address = dr["Bank_Address"] != DBNull.Value ? dr["Bank_Address"].ToString() : null;

                    data.Sys_Created_By = dr["Sys_Created_By"] != DBNull.Value ? dr["Sys_Created_By"].ToString() : null;
                    data.Sys_Created_On = (dr["Sys_Created_On"].ToString());
                    data.Sys_Modified_By = dr["Sys_Modified_By"] != DBNull.Value ? dr["Sys_Modified_By"].ToString() : null;
                    data.Sys_Modified_On = dr["Sys_Modified_On"] != DBNull.Value ? (dr["Sys_Modified_On"].ToString()) : null;
                }

                int RespCode = Convert.ToInt32(P_CODE.Value);
                db.CloseConnection();
                if (RespCode == 0)
                {
                    return data;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Function to delete Employee
        public bool Delete(int id)
        {
            try
            {
                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_HRM_DELETE_EMPLOYEE";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_SYS_CODE = new SqlParameter();
                P_SYS_CODE.ParameterName = "P_SYS_CODE";
                P_SYS_CODE.SqlDbType = SqlDbType.Int;
                P_SYS_CODE.Value = id;
                db.cmd.Parameters.Add(P_SYS_CODE);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.FillData(sql);
                int RespCode = Convert.ToInt32(P_CODE.Value);
                db.CloseConnection();

                if (RespCode == 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion


        //Tabs Details Form
        #region Function_To_Get_Branch_Location_Drop


        public List<SelectListItem> Get_Branch_Location_Drop()
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;
                // Access OrgCode claim
                var orgCode = user.FindFirst("OrgCode")?.Value;
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_EMP_GET_BRANCH_LOCATION_DROP";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);


                int RespCode = Convert.ToInt32(P_CODE.Value);

                List<SelectListItem> list = new List<SelectListItem>();

                while (dr.Read())
                {
                    SelectListItem s = new SelectListItem();


                    s.Text = dr["Location_Name"].ToString();
                    s.Value = dr["Sys_Code"].ToString();

                    list.Add(s);

                }

                db.CloseConnection();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Function_To_Get_Department_Drop
        public List<SelectListItem> Get_Department_Drop()
        {
            try
            {



                var user = _httpContextAccessor.HttpContext.User;



                // Access OrgCode claim
                var orgCode = user.FindFirst("OrgCode")?.Value;



                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_EMP_GET_DEPARTMENT_DROP";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);


                int RespCode = Convert.ToInt32(P_CODE.Value);

                List<SelectListItem> list = new List<SelectListItem>();

                while (dr.Read())
                {
                    SelectListItem s = new SelectListItem();


                    s.Text = dr["NAME"].ToString();
                    s.Value = dr["SYS_CODE"].ToString();

                    list.Add(s);

                }

                db.CloseConnection();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Function_To_Get_Department_Head_By_Dept_Id

        public DepartmentMaster Get_Department_Head_By_Dept_Id(string syscode)
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_EMP_GET_DEPARTMENT_HEAD_BY_DEPT_ID";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_DEPT_ID = new SqlParameter();
                P_DEPT_ID.ParameterName = "P_DEPT_ID";
                P_DEPT_ID.SqlDbType = SqlDbType.VarChar;
                P_DEPT_ID.Value = syscode ?? (object)DBNull.Value;
                P_DEPT_ID.Size = 250;
                db.cmd.Parameters.Add(P_DEPT_ID);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);

                int RespCode = Convert.ToInt32(P_CODE.Value);

                DepartmentMaster Dept = new DepartmentMaster();

                while (dr.Read())
                {
                    Dept.Department_Head = dr["DEPARTMENT_HEAD"] != DBNull.Value ? dr["DEPARTMENT_HEAD"].ToString() : null;
                    Dept.Code = dr["SYS_CODE"] != DBNull.Value ? dr["SYS_CODE"].ToString() : null;
                }

                db.CloseConnection();
                return Dept;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Function_To_Get_Reportin_To_Drop

        public List<SelectListItem> Get_Reportin_To_Drop()
        {
            try
            {



                var user = _httpContextAccessor.HttpContext.User;



                // Access OrgCode claim
                var orgCode = user.FindFirst("OrgCode")?.Value;



                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_EMP_GET_REPORTING_TO_DROP";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);


                int RespCode = Convert.ToInt32(P_CODE.Value);

                List<SelectListItem> list = new List<SelectListItem>();

                while (dr.Read())
                {
                    SelectListItem s = new SelectListItem();


                    s.Text = dr["NAME"].ToString();
                    s.Value = dr["SYS_CODE"].ToString();

                    list.Add(s);

                }

                db.CloseConnection();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Function_To_Get_Designation_Drop


        public List<SelectListItem> Get_Designation_Drop()
        {
            try
            {



                var user = _httpContextAccessor.HttpContext.User;



                // Access OrgCode claim
                var orgCode = user.FindFirst("OrgCode")?.Value;



                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_EMP_GET_DESIGNATION_DROP";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);


                int RespCode = Convert.ToInt32(P_CODE.Value);

                List<SelectListItem> list = new List<SelectListItem>();

                while (dr.Read())
                {
                    SelectListItem s = new SelectListItem();


                    s.Text = dr["NAME"].ToString();
                    s.Value = dr["SYS_CODE"].ToString();

                    list.Add(s);

                }
                db.CloseConnection();

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Function_To_Update_Emp_General_Details


        public Employee Update_Emp_General_Details(Employee obj)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;


                // Access Id claim
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;


                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_UPDATE_EMP_GENERAL_DETAILS";
                DBHelper db = new DBHelper();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters
                SqlParameter P_Sys_Code = new SqlParameter();
                P_Sys_Code.ParameterName = "P_Sys_Code";
                P_Sys_Code.SqlDbType = SqlDbType.Int;
                P_Sys_Code.Value = obj.Sys_Code;
                db.cmd.Parameters.Add(P_Sys_Code);

                SqlParameter P_Date_of_Joining = new SqlParameter();
                P_Date_of_Joining.ParameterName = "P_Date_of_Joining";
                P_Date_of_Joining.SqlDbType = SqlDbType.Date;
                if (string.IsNullOrWhiteSpace(obj.Date_of_Joining))
                {
                    P_Date_of_Joining.Value = DBNull.Value;
                }
                else
                {
                    P_Date_of_Joining.Value = obj.Date_of_Joining;
                }
                //P_Date_of_Joining.Value = obj.Date_of_Joining ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Date_of_Joining);

                SqlParameter P_Transfer_Date = new SqlParameter();
                P_Transfer_Date.ParameterName = "P_Transfer_Date";
                P_Transfer_Date.SqlDbType = SqlDbType.Date;
                if (string.IsNullOrWhiteSpace(obj.Transfer_Date))
                {
                    P_Transfer_Date.Value = DBNull.Value;
                }
                else
                {
                    P_Transfer_Date.Value = obj.Transfer_Date;
                }
                //P_Transfer_Date.Value = obj.Transfer_Date ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Transfer_Date);

                SqlParameter P_Employment_End_Date = new SqlParameter();
                P_Employment_End_Date.ParameterName = "P_Employment_End_Date";
                P_Employment_End_Date.SqlDbType = SqlDbType.Date;

                if (string.IsNullOrWhiteSpace(obj.Employment_End_Date))
                {
                    P_Employment_End_Date.Value = DBNull.Value;
                }
                else
                {
                    P_Employment_End_Date.Value = obj.Employment_End_Date;
                }
                //P_Employment_End_Date.Value = obj.Employment_End_Date ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Employment_End_Date);

                SqlParameter P_Probation_Period = new SqlParameter();
                P_Probation_Period.ParameterName = "P_Probation_Period";
                P_Probation_Period.SqlDbType = SqlDbType.VarChar;
                P_Probation_Period.Size = 3;
                P_Probation_Period.Value = obj.Probation_Period ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Probation_Period);

                SqlParameter P_Professional_Status = new SqlParameter();
                P_Professional_Status.ParameterName = "P_Professional_Status";
                P_Professional_Status.SqlDbType = SqlDbType.VarChar;
                P_Professional_Status.Size = 15;
                P_Professional_Status.Value = obj.Professional_Status ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Professional_Status);

                SqlParameter P_National_ID = new SqlParameter();
                P_National_ID.ParameterName = "P_National_ID";
                P_National_ID.SqlDbType = SqlDbType.VarChar;
                P_National_ID.Size = 50;
                P_National_ID.Value = obj.National_ID ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_National_ID);

                SqlParameter P_BranchCode = new SqlParameter();
                P_BranchCode.ParameterName = "P_BranchCode";
                P_BranchCode.SqlDbType = SqlDbType.VarChar;
                P_BranchCode.Size = 30;
                P_BranchCode.Value = obj.BranchCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_BranchCode);

                SqlParameter P_DepartmentCode = new SqlParameter();
                P_DepartmentCode.ParameterName = "P_DepartmentCode";
                P_DepartmentCode.SqlDbType = SqlDbType.Int;
                P_DepartmentCode.Value = obj.DepartmentCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_DepartmentCode);

                SqlParameter P_Department_Head_Code = new SqlParameter();
                P_Department_Head_Code.ParameterName = "P_Department_Head_Code";
                P_Department_Head_Code.SqlDbType = SqlDbType.Int;
                P_Department_Head_Code.Value = obj.Department_Head_Code ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Department_Head_Code);

                SqlParameter P_Reporting_To_Code = new SqlParameter();
                P_Reporting_To_Code.ParameterName = "P_Reporting_To_Code";
                P_Reporting_To_Code.SqlDbType = SqlDbType.Int;
                P_Reporting_To_Code.Value = obj.Reporting_To_Code ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Reporting_To_Code);

                SqlParameter P_Designation_Start_Date = new SqlParameter();
                P_Designation_Start_Date.ParameterName = "P_Designation_Start_Date";
                P_Designation_Start_Date.SqlDbType = SqlDbType.Date;


                if (string.IsNullOrWhiteSpace(obj.Designation_Start_Date))
                {
                    P_Designation_Start_Date.Value = DBNull.Value;
                }
                else
                {
                    P_Designation_Start_Date.Value = obj.Designation_Start_Date;
                }
                //P_Designation_Start_Date.Value = obj.Designation_Start_Date ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Designation_Start_Date);

                SqlParameter P_DesignationCode = new SqlParameter();
                P_DesignationCode.ParameterName = "P_DesignationCode";
                P_DesignationCode.SqlDbType = SqlDbType.Int;
                P_DesignationCode.Value = obj.DesignationCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_DesignationCode);

                SqlParameter P_Designation_Qualifier = new SqlParameter();
                P_Designation_Qualifier.ParameterName = "P_Designation_Qualifier";
                P_Designation_Qualifier.SqlDbType = SqlDbType.VarChar;
                P_Designation_Qualifier.Size = 100;
                P_Designation_Qualifier.Value = obj.Designation_Qualifier ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Designation_Qualifier);

                SqlParameter P_Job_Description = new SqlParameter();
                P_Job_Description.ParameterName = "P_Job_Description";
                P_Job_Description.SqlDbType = SqlDbType.VarChar;
                P_Job_Description.Size = 2000;
                P_Job_Description.Value = obj.Job_Description ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Job_Description);

                SqlParameter P_Payroll_Employee_ID = new SqlParameter();
                P_Payroll_Employee_ID.ParameterName = "P_Payroll_Employee_ID";
                P_Payroll_Employee_ID.SqlDbType = SqlDbType.VarChar;
                P_Payroll_Employee_ID.Size = 50;
                P_Payroll_Employee_ID.Value = obj.Payroll_Employee_ID ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Payroll_Employee_ID);



                //Personal Details

                SqlParameter P_P_Date_Of_Birth = new SqlParameter();
                P_P_Date_Of_Birth.ParameterName = "P_Date_Of_Birth";
                P_P_Date_Of_Birth.SqlDbType = SqlDbType.Date;

                if (string.IsNullOrWhiteSpace(obj.Date_Of_Birth))
                {
                    P_P_Date_Of_Birth.Value = DBNull.Value;
                }
                else
                {
                    P_P_Date_Of_Birth.Value = obj.Date_Of_Birth;
                }

                //P_P_Date_Of_Birth.Value = obj.Date_Of_Birth ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Date_Of_Birth);

                SqlParameter P_P_Aadhar_No = new SqlParameter();
                P_P_Aadhar_No.ParameterName = "P_Aadhar_No";
                P_P_Aadhar_No.SqlDbType = SqlDbType.VarChar;
                P_P_Aadhar_No.Size = 12;
                P_P_Aadhar_No.Value = obj.Aadhar_No ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Aadhar_No);

                SqlParameter P_P_PAN_No = new SqlParameter();
                P_P_PAN_No.ParameterName = "P_PAN_No";
                P_P_PAN_No.SqlDbType = SqlDbType.VarChar;
                P_P_PAN_No.Size = 10;
                P_P_PAN_No.Value = obj.PAN_No ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_PAN_No);

                SqlParameter P_P_Gender = new SqlParameter();
                P_P_Gender.ParameterName = "P_Gender";
                P_P_Gender.SqlDbType = SqlDbType.VarChar;
                P_P_Gender.Size = 10;
                P_P_Gender.Value = obj.Gender ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Gender);

                SqlParameter P_P_UAN_No = new SqlParameter();
                P_P_UAN_No.ParameterName = "P_UAN_No";
                P_P_UAN_No.SqlDbType = SqlDbType.VarChar;
                P_P_UAN_No.Size = 50;
                P_P_UAN_No.Value = obj.UAN_No ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_UAN_No);

                SqlParameter P_P_Nationality = new SqlParameter();
                P_P_Nationality.ParameterName = "P_Nationality";
                P_P_Nationality.SqlDbType = SqlDbType.VarChar;
                P_P_Nationality.Size = 50;
                P_P_Nationality.Value = obj.Nationality ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Nationality);

                SqlParameter P_P_Religion = new SqlParameter();
                P_P_Religion.ParameterName = "P_Religion";
                P_P_Religion.SqlDbType = SqlDbType.VarChar;
                P_P_Religion.Size = 50;
                P_P_Religion.Value = obj.Religion ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Religion);

                SqlParameter P_P_Marital_Status = new SqlParameter();
                P_P_Marital_Status.ParameterName = "P_Marital_Status";
                P_P_Marital_Status.SqlDbType = SqlDbType.VarChar;
                P_P_Marital_Status.Size = 10;
                P_P_Marital_Status.Value = obj.Marital_Status ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Marital_Status);

                SqlParameter P_P_Wedding_Annive = new SqlParameter();
                P_P_Wedding_Annive.ParameterName = "P_Wedding_Annive";
                P_P_Wedding_Annive.SqlDbType = SqlDbType.Date;

                if (string.IsNullOrWhiteSpace(obj.Wedding_Annive))
                {
                    P_P_Wedding_Annive.Value = DBNull.Value;
                }
                else
                {
                    P_P_Wedding_Annive.Value = obj.Wedding_Annive;
                }
                //P_P_Wedding_Annive.Value = obj.Wedding_Annive ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Wedding_Annive);

                SqlParameter P_P_Office_Phone_No = new SqlParameter();
                P_P_Office_Phone_No.ParameterName = "P_Office_Phone_No";
                P_P_Office_Phone_No.SqlDbType = SqlDbType.VarChar;
                P_P_Office_Phone_No.Size = 15;
                P_P_Office_Phone_No.Value = obj.Office_Phone_No ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Office_Phone_No);

                SqlParameter P_P_Personal_Email_ID = new SqlParameter();
                P_P_Personal_Email_ID.ParameterName = "P_Personal_Email_ID";
                P_P_Personal_Email_ID.SqlDbType = SqlDbType.VarChar;
                P_P_Personal_Email_ID.Size = 100;
                P_P_Personal_Email_ID.Value = obj.Personal_Email_ID ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Personal_Email_ID);

                SqlParameter P_P_Personal_Mobile_No = new SqlParameter();
                P_P_Personal_Mobile_No.ParameterName = "P_Personal_Mobile_No";
                P_P_Personal_Mobile_No.SqlDbType = SqlDbType.VarChar;
                P_P_Personal_Mobile_No.Size = 10;
                P_P_Personal_Mobile_No.Value = obj.Personal_Mobile_No ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Personal_Mobile_No);

                SqlParameter P_P_Home_Town = new SqlParameter();
                P_P_Home_Town.ParameterName = "P_Home_Town";
                P_P_Home_Town.SqlDbType = SqlDbType.VarChar;
                P_P_Home_Town.Size = 50;
                P_P_Home_Town.Value = obj.Home_Town ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Home_Town);

                SqlParameter P_P_Airport_Name = new SqlParameter();
                P_P_Airport_Name.ParameterName = "P_Airport_Name";
                P_P_Airport_Name.SqlDbType = SqlDbType.VarChar;
                P_P_Airport_Name.Size = 100;
                P_P_Airport_Name.Value = obj.Airport_Name ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Airport_Name);

                SqlParameter P_P_Personal_Address = new SqlParameter();
                P_P_Personal_Address.ParameterName = "P_Personal_Address";
                P_P_Personal_Address.SqlDbType = SqlDbType.VarChar;
                P_P_Personal_Address.Size = 500;
                P_P_Personal_Address.Value = obj.Personal_Address ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Personal_Address);

                SqlParameter P_P_Phone_Number = new SqlParameter();
                P_P_Phone_Number.ParameterName = "P_Phone_Number";
                P_P_Phone_Number.SqlDbType = SqlDbType.VarChar;
                P_P_Phone_Number.Size = 30;
                P_P_Phone_Number.Value = obj.Phone_Number ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Phone_Number);

                SqlParameter P_P_Contact_Address = new SqlParameter();
                P_P_Contact_Address.ParameterName = "P_Contact_Address";
                P_P_Contact_Address.SqlDbType = SqlDbType.VarChar;
                P_P_Contact_Address.Size = 500;
                P_P_Contact_Address.Value = obj.Contact_Address ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Contact_Address);

                SqlParameter P_P_Emergency_Contact_Person = new SqlParameter();
                P_P_Emergency_Contact_Person.ParameterName = "P_Emergency_Contact_Person";
                P_P_Emergency_Contact_Person.SqlDbType = SqlDbType.VarChar;
                P_P_Emergency_Contact_Person.Size = 100;
                P_P_Emergency_Contact_Person.Value = obj.Emergency_Contact_Person ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Emergency_Contact_Person);

                SqlParameter P_P_Emergency_Contact_Address = new SqlParameter();
                P_P_Emergency_Contact_Address.ParameterName = "P_Emergency_Contact_Address";
                P_P_Emergency_Contact_Address.SqlDbType = SqlDbType.VarChar;
                P_P_Emergency_Contact_Address.Size = 500;
                P_P_Emergency_Contact_Address.Value = obj.Emergency_Contact_Address ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Emergency_Contact_Address);

                SqlParameter P_P_Emergency_Contact_No = new SqlParameter();
                P_P_Emergency_Contact_No.ParameterName = "P_Emergency_Contact_No";
                P_P_Emergency_Contact_No.SqlDbType = SqlDbType.VarChar;
                P_P_Emergency_Contact_No.Size = 30;
                P_P_Emergency_Contact_No.Value = obj.Emergency_Contact_No ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Emergency_Contact_No);



                SqlParameter P_P_Bank_AC_No = new SqlParameter();
                P_P_Bank_AC_No.ParameterName = "P_Bank_AC_No";
                P_P_Bank_AC_No.SqlDbType = SqlDbType.VarChar;
                P_P_Bank_AC_No.Size = 50;
                P_P_Bank_AC_No.Value = obj.Bank_AC_No ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Bank_AC_No);


                SqlParameter P_P_IFSC_Code = new SqlParameter();
                P_P_IFSC_Code.ParameterName = "P_IFSC_Code";
                P_P_IFSC_Code.SqlDbType = SqlDbType.VarChar;
                P_P_IFSC_Code.Size = 11;
                P_P_IFSC_Code.Value = obj.IFSC_Code ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_IFSC_Code);

                SqlParameter P_P_Bank_Name = new SqlParameter();
                P_P_Bank_Name.ParameterName = "P_Bank_Name";
                P_P_Bank_Name.SqlDbType = SqlDbType.VarChar;
                P_P_Bank_Name.Size = 100;
                P_P_Bank_Name.Value = obj.Bank_Name ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Bank_Name);


                SqlParameter P_P_Bank_Address = new SqlParameter();
                P_P_Bank_Address.ParameterName = "P_Bank_Address";
                P_P_Bank_Address.SqlDbType = SqlDbType.VarChar;
                P_P_Bank_Address.Size = 500;
                P_P_Bank_Address.Value = obj.Bank_Address ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_P_Bank_Address);


                SqlParameter P_Sys_Modified_By = new SqlParameter();
                P_Sys_Modified_By.ParameterName = "P_Sys_Modified_By";
                P_Sys_Modified_By.SqlDbType = SqlDbType.VarChar;
                P_Sys_Modified_By.Value = userId ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Sys_Modified_By);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.FillData(sql);

                int RespCode = Convert.ToInt32(P_CODE.Value);

                string ResponseMsg = P_Message.Value.ToString();
                db.CloseConnection();
                if (RespCode == 0)
                {
                    return obj;
                }
                else
                {
                    return obj;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        #endregion

        #region Function_To_Get_Department_Head_Code_Drop

        public List<SelectListItem> Get_Department_Head_Code_Drop()
        {
            try
            {



                var user = _httpContextAccessor.HttpContext.User;



                // Access OrgCode claim
                var orgCode = user.FindFirst("OrgCode")?.Value;



                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_EMP_GET_DEPARTMENT_DROP";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);


                int RespCode = Convert.ToInt32(P_CODE.Value);

                List<SelectListItem> list = new List<SelectListItem>();

                while (dr.Read())
                {
                    SelectListItem s = new SelectListItem();


                    s.Text = dr["DEPARTMENT_HEAD"].ToString();
                    s.Value = dr["SYS_CODE"].ToString();

                    list.Add(s);

                }

                db.CloseConnection();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Get_CountryList_For_Nationality_Drop
        public List<SelectListItem> Get_CountryList_For_Nationality_Drop(int Id)
        {

            try
            {

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_GETALL_COUNTRIESLIST";
                DBHelper db = new DBHelper();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                SqlParameter P_ID = new SqlParameter();
                P_ID.ParameterName = "P_ID";
                P_ID.SqlDbType = SqlDbType.Int;
                P_ID.Value = Id;
                db.cmd.Parameters.Add(P_ID);

                var dr = db.DataReader(sql);



                int RespCode = Convert.ToInt32(P_CODE.Value);
                //string RespMsg = P_Message.Value.ToString();


                List<SelectListItem> listItems = new List<SelectListItem>();

                while (dr.Read())
                {
                    SelectListItem s = new SelectListItem();


                    s.Text = dr["Nationality"].ToString();
                    s.Value = dr["Sys_Code"].ToString();

                    listItems.Add(s);
                }

                db.CloseConnection();

                if (RespCode == 0)
                {
                    return listItems;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Function to  Get_CurrencyList_For_Cureency_DropDown
        public List<SelectListItem> Get_CurrencyList_For_Cureency_Drop()
        {

            try
            {

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_EMP_GET_ALL_CURRENCY";
                DBHelper db = new DBHelper();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);


                var dr = db.DataReader(sql);



                int RespCode = Convert.ToInt32(P_CODE.Value);
                //string RespMsg = P_Message.Value.ToString();


                List<SelectListItem> listItems = new List<SelectListItem>();

                while (dr.Read())
                {
                    SelectListItem s = new SelectListItem();


                    s.Text = dr["Name"].ToString();
                    s.Value = dr["Sys_Code"].ToString();

                    listItems.Add(s);
                }

                db.CloseConnection();

                if (RespCode == 0)
                {
                    return listItems;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        //GL Account Mapping 
        #region Function to Insert and upadte GL account against employee
        public List<Employee_GL_Account_Map> InsertUpadte_GL_Mapping_Get_GL(Employee_GL_Account_Map obj)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                var user = _httpContextAccessor.HttpContext.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_INSERTUPDATE_GLACCOUNT_MAPPING";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_Sys_Code = new SqlParameter();
                P_Sys_Code.ParameterName = "P_Sys_Code";
                P_Sys_Code.SqlDbType = SqlDbType.VarChar;
                P_Sys_Code.Value = obj.sys_Code_Gl ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Sys_Code);

                SqlParameter P_EmpCode = new SqlParameter();
                P_EmpCode.ParameterName = "P_EmpCode";
                P_EmpCode.SqlDbType = SqlDbType.Int;
                P_EmpCode.Value = obj.EmpCode;
                db.cmd.Parameters.Add(P_EmpCode);

                SqlParameter P_Branch_Location_Code = new SqlParameter();
                P_Branch_Location_Code.ParameterName = "P_Branch_Location_Code";
                P_Branch_Location_Code.SqlDbType = SqlDbType.VarChar;
                P_Branch_Location_Code.Size = 100;
                P_Branch_Location_Code.Value = obj.BranchLocationCode ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_Branch_Location_Code);

                SqlParameter P_GL_AccountCode = new SqlParameter();
                P_GL_AccountCode.ParameterName = "P_GL_AccountCode";
                P_GL_AccountCode.SqlDbType = SqlDbType.Int;
                P_GL_AccountCode.Value = 1;
                db.cmd.Parameters.Add(P_GL_AccountCode);

                SqlParameter P_CurrencyCode = new SqlParameter();
                P_CurrencyCode.ParameterName = "P_CurrencyCode";
                P_CurrencyCode.SqlDbType = SqlDbType.Int;
                P_CurrencyCode.Value = obj.CurrencyCode ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_CurrencyCode);

                SqlParameter P_Status = new SqlParameter();
                P_Status.ParameterName = "P_Status";
                P_Status.SqlDbType = SqlDbType.Char;
                P_Status.Size = 1;
                P_Status.Value = obj.Status ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_Status);

                SqlParameter P_Note = new SqlParameter();
                P_Note.ParameterName = "P_Note";
                P_Note.SqlDbType = SqlDbType.VarChar;
                P_Note.Size = 4000;
                P_Note.Value = obj.Note ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_Note);

                SqlParameter P_OrgCode = new SqlParameter();
                P_OrgCode.ParameterName = "P_OrgCode";
                P_OrgCode.SqlDbType = SqlDbType.VarChar;
                P_OrgCode.Size = 100;
                P_OrgCode.Value = orgCode ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_OrgCode);


                SqlParameter P_UserId = new SqlParameter();
                P_UserId.ParameterName = "P_UserId";
                P_UserId.SqlDbType = SqlDbType.VarChar;
                P_UserId.Size = 200;
                P_UserId.Value = userId;
                db.cmd.Parameters.Add(P_UserId);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);


                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);
                List<Employee_GL_Account_Map> list = new List<Employee_GL_Account_Map>();

                while (dr.Read())
                {
                    Employee_GL_Account_Map data = new Employee_GL_Account_Map();
                    data.sys_Code_Gl = dr["Sys_Code"].ToString();
                    data.OrgCode = dr["Organization"] != DBNull.Value ? dr["Organization"].ToString() : null;
                    data.EmpCode = dr["Employee_Name"] != DBNull.Value ? dr["Employee_Name"].ToString() : null;
                    data.Status = dr["Status"] != DBNull.Value ? dr["Status"].ToString() : null;
                    data.BranchLocationCode = dr["Branch_Name"] != DBNull.Value ? dr["Branch_Name"].ToString() : null;
                    //data.GLAccountCode = dr["GL_AccountCode"] != DBNull.Value ? dr["GL_AccountCode"].ToString() : null;
                    data.CurrencyCode = dr["Currency"] != DBNull.Value ? dr["Currency"].ToString() : null;
                    data.Note = dr["Note"] != DBNull.Value ? dr["Note"].ToString() : null;
                    data.Sys_Created_By = dr["Sys_Created_By"] != DBNull.Value ? dr["Sys_Created_By"].ToString() : null;
                    data.Sys_Created_On = dr["Sys_Created_On"] != DBNull.Value ? dr["Sys_Created_On"].ToString() : null;
                    data.Sys_Modified_By = dr["Sys_Modified_By"] != DBNull.Value ? dr["Sys_Modified_By"].ToString() : null;
                    data.Sys_Modified_On = dr["Sys_Modified_On"] != DBNull.Value ? dr["Sys_Modified_On"].ToString() : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss");

                    list.Add(data);
                }

                dr.Close();

                db.CloseConnection();
                return list;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Fuction to get Gl Mapping By sys_code
        public Employee_GL_Account_Map GetEmployee_GL_MappingById(int Id)
        {
            try
            {
                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_HRM_GETEMP_GLMAP_BYSYSCODE";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_ID = new SqlParameter();
                P_ID.ParameterName = "P_SYS_CODE";
                P_ID.SqlDbType = SqlDbType.Int;
                P_ID.Value = Id;
                db.cmd.Parameters.Add(P_ID);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);

                Employee_GL_Account_Map data = new Employee_GL_Account_Map();
                while (dr.Read())
                {

                    data.sys_Code_Gl = dr["Sys_Code"].ToString();
                    data.OrgCode = dr["Organization"] != DBNull.Value ? dr["Organization"].ToString() : null;
                    data.EmpCode = dr["Employee_Name"] != DBNull.Value ? dr["Employee_Name"].ToString() : null;
                    data.Status = dr["Status"] != DBNull.Value ? dr["Status"].ToString() : null;
                    data.BranchLocationCode = dr["Branch_Location_Code"] != DBNull.Value ? dr["Branch_Location_Code"].ToString() : null;
                    //data.GLAccountCode = dr["GL_AccountCode"] != DBNull.Value ? dr["GL_AccountCode"].ToString() : null;
                    data.CurrencyCode = dr["CurrencyCode"] != DBNull.Value ? dr["CurrencyCode"].ToString() : null;
                    data.Note = dr["Note"] != DBNull.Value ? dr["Note"].ToString() : null;
                    data.Sys_Created_By = dr["Sys_Created_By"] != DBNull.Value ? dr["Sys_Created_By"].ToString() : null;
                    data.Sys_Created_On = dr["Sys_Created_On"] != DBNull.Value ? dr["Sys_Created_On"].ToString() : null;
                    data.Sys_Modified_By = dr["Sys_Modified_By"] != DBNull.Value ? dr["Sys_Modified_By"].ToString() : null;
                    data.Sys_Modified_On = dr["Sys_Modified_On"] != DBNull.Value ? dr["Sys_Modified_On"].ToString() : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss");


                }

                int RespCode = Convert.ToInt32(P_CODE.Value);
                db.CloseConnection();
                if (RespCode == 0)
                {
                    return data;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Fuction to get Gl Mapping By empcode
        public List<Employee_GL_Account_Map> GetEmployee_GL_MappingBy_EmpCode(string Id)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                var user = _httpContextAccessor.HttpContext.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_GET_EMPGLMAPPING_BY_EMPCODE";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_ID = new SqlParameter();
                P_ID.ParameterName = "P_ID";
                P_ID.SqlDbType = SqlDbType.Int;
                P_ID.Value = Id;
                db.cmd.Parameters.Add(P_ID);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);


                var dr = db.DataReader(sql);
                List<Employee_GL_Account_Map> list = new List<Employee_GL_Account_Map>();

                while (dr.Read())
                {
                    Employee_GL_Account_Map data = new Employee_GL_Account_Map();
                    data.sys_Code_Gl = dr["Sys_Code"].ToString();
                    data.OrgCode = dr["Organization"] != DBNull.Value ? dr["Organization"].ToString() : null;
                    data.EmpCode = dr["Employee_Name"] != DBNull.Value ? dr["Employee_Name"].ToString() : null;
                    data.Status = dr["Status"] != DBNull.Value ? dr["Status"].ToString() : null;
                    data.BranchLocationCode = dr["Branch_Name"] != DBNull.Value ? dr["Branch_Name"].ToString() : null;
                    //data.GLAccountCode = dr["GL_AccountCode"] != DBNull.Value ? dr["GL_AccountCode"].ToString() : null;
                    data.CurrencyCode = dr["Currency"] != DBNull.Value ? dr["Currency"].ToString() : null;
                    data.Note = dr["Note"] != DBNull.Value ? dr["Note"].ToString() : null;
                    data.Sys_Created_By = dr["Sys_Created_By"] != DBNull.Value ? dr["Sys_Created_By"].ToString() : null;
                    data.Sys_Created_On = dr["Sys_Created_On"] != DBNull.Value ? dr["Sys_Created_On"].ToString() : null;
                    data.Sys_Modified_By = dr["Sys_Modified_By"] != DBNull.Value ? dr["Sys_Modified_By"].ToString() : null;
                    data.Sys_Modified_On = dr["Sys_Modified_On"] != DBNull.Value ? dr["Sys_Modified_On"].ToString() : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss");

                    list.Add(data);
                }

                dr.Close();
                db.CloseConnection();
                return list;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        //Documents 
        #region Function to get Emp_Documents by empcode
        public List<Employee_Doc> InsertUpadte_Emp_Documents(IFormFile Emp_documents, string document_type, string Sys_Code_Doc, string Emp_SysCode)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                var user = _httpContextAccessor.HttpContext.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }

                byte[] logoBytes = null;
                string PhotoName = "";
                if (Emp_documents != null && Emp_documents.Length > 0)
                {
                    // Read the file into a byte array

                    using (var memoryStream = new MemoryStream())
                    {
                        Emp_documents.CopyTo(memoryStream);
                        logoBytes = memoryStream.ToArray();
                    }

                    PhotoName = Path.GetFileName(Emp_documents.FileName);


                }

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_INSERTUPDATE_EMPL_DOC";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_EmpDoc_Sys_Code = new SqlParameter();
                P_EmpDoc_Sys_Code.ParameterName = "P_EmpDoc_Sys_Code";
                P_EmpDoc_Sys_Code.SqlDbType = SqlDbType.Int;
                P_EmpDoc_Sys_Code.Value = Sys_Code_Doc ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_EmpDoc_Sys_Code);

                SqlParameter P_EmpCode = new SqlParameter();
                P_EmpCode.ParameterName = "P_EmpCode";
                P_EmpCode.SqlDbType = SqlDbType.Int;
                P_EmpCode.Value = Emp_SysCode;
                db.cmd.Parameters.Add(P_EmpCode);

                SqlParameter P_Document_Type = new SqlParameter();
                P_Document_Type.ParameterName = "P_Document_Type";
                P_Document_Type.SqlDbType = SqlDbType.VarChar;
                P_Document_Type.Size = 100;
                P_Document_Type.Value = document_type;
                db.cmd.Parameters.Add(P_Document_Type);


                SqlParameter P_Document_Name = new SqlParameter();
                P_Document_Name.ParameterName = "P_Document_Name";
                P_Document_Name.SqlDbType = SqlDbType.VarChar;
                P_Document_Name.Size = 100;
                P_Document_Name.Value = PhotoName.Trim();
                db.cmd.Parameters.Add(P_Document_Name);

                SqlParameter P_Documents = new SqlParameter();
                P_Documents.ParameterName = "P_Documents";
                P_Documents.SqlDbType = SqlDbType.VarBinary;
                P_Documents.Size = -1;
                P_Documents.Value = logoBytes ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Documents);

                SqlParameter P_OrgCode = new SqlParameter();
                P_OrgCode.ParameterName = "P_OrgCode";
                P_OrgCode.SqlDbType = SqlDbType.VarChar;
                P_OrgCode.Size = 100;
                P_OrgCode.Value = orgCode;
                db.cmd.Parameters.Add(P_OrgCode);


                SqlParameter P_UserId = new SqlParameter();
                P_UserId.ParameterName = "P_UserId";
                P_UserId.SqlDbType = SqlDbType.VarChar;
                P_UserId.Size = 200;
                P_UserId.Value = userId;
                db.cmd.Parameters.Add(P_UserId);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);


                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);
                List<Employee_Doc> list = new List<Employee_Doc>();


                while (dr.Read())
                {
                    Employee_Doc data = new Employee_Doc();
                    data.sys_code = dr["Sys_Code"].ToString();
                    data.OrgCode = dr["OrgCode"] != DBNull.Value ? dr["OrgCode"].ToString() : null;
                    data.EmpCode = dr["EmpCode"] != DBNull.Value ? dr["EmpCode"].ToString() : null;
                    data.Document_Type = dr["Document_Type"] != DBNull.Value ? dr["Document_Type"].ToString() : null;
                    data.document_Name = dr["Document_Name"] != DBNull.Value ? dr["Document_Name"].ToString().Trim() : null;

                    byte[] documentBytes = dr["Documents"] as byte[];

                    var base64 = Convert.ToBase64String(documentBytes);

                    string fileName = data.document_Name ?? "document";
                    string extension = System.IO.Path.GetExtension(fileName);
                    string mimeType = CommonFunction.GetMimeType(extension);

                    //string mimeType = "application/octet-stream"; // Change this to the appropriate MIME type if known

                    data.empdocumentBase64 = $"data:{mimeType};base64,{base64}";




                    //data.Document = dr["Documents"] as byte[];
                    data.Sys_Created_By = dr["Sys_Created_By"] != DBNull.Value ? dr["Sys_Created_By"].ToString() : null;
                    data.Sys_Created_On = dr["Sys_Created_On"] != DBNull.Value ? dr["Sys_Created_On"].ToString() : null;
                    data.Sys_Modified_By = dr["Sys_Modified_By"] != DBNull.Value ? dr["Sys_Modified_By"].ToString() : null;
                    data.Sys_Modified_On = dr["Sys_Modified_On"] != DBNull.Value ? dr["Sys_Modified_On"].ToString() : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss");

                    list.Add(data);
                }

                dr.Close();

                db.CloseConnection();
                return list;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Function to get employee documents by employee code
        public List<Employee_Doc> GetEmp_Documents_By_EmpCode(string Id)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                var user = _httpContextAccessor.HttpContext.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_GET_EMPL_DOC_BY_EMP_SYS_CODE";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_EmpSysCode = new SqlParameter();
                P_EmpSysCode.ParameterName = "P_EmpSysCode";
                P_EmpSysCode.SqlDbType = SqlDbType.Int;
                P_EmpSysCode.Value = Id;
                db.cmd.Parameters.Add(P_EmpSysCode);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);


                var dr = db.DataReader(sql);
                List<Employee_Doc> list = new List<Employee_Doc>();




                while (dr.Read())
                {
                    Employee_Doc data = new Employee_Doc();
                    data.sys_code = dr["Sys_Code"].ToString();
                    data.OrgCode = dr["OrgCode"] != DBNull.Value ? dr["OrgCode"].ToString() : null;
                    data.EmpCode = dr["EmpCode"] != DBNull.Value ? dr["EmpCode"].ToString() : null;
                    data.Document_Type = dr["Document_Type"] != DBNull.Value ? dr["Document_Type"].ToString() : null;
                    data.document_Name = dr["Document_Name"] != DBNull.Value ? dr["Document_Name"].ToString().Trim() : null;

                    byte[] documentBytes = dr["Documents"] as byte[];

                    var base64 = Convert.ToBase64String(documentBytes);

                    string fileName = data.document_Name ?? "document";
                    string extension = System.IO.Path.GetExtension(fileName);
                    string mimeType = CommonFunction.GetMimeType(extension);

                    //string mimeType = "application/octet-stream"; // Change this to the appropriate MIME type if known

                    data.empdocumentBase64 = $"data:{mimeType};base64,{base64}";




                    //data.Document = dr["Documents"] as byte[];
                    data.Sys_Created_By = dr["Sys_Created_By"] != DBNull.Value ? dr["Sys_Created_By"].ToString() : null;
                    data.Sys_Created_On = dr["Sys_Created_On"] != DBNull.Value ? dr["Sys_Created_On"].ToString() : null;
                    data.Sys_Modified_By = dr["Sys_Modified_By"] != DBNull.Value ? dr["Sys_Modified_By"].ToString() : null;
                    data.Sys_Modified_On = dr["Sys_Modified_On"] != DBNull.Value ? dr["Sys_Modified_On"].ToString() : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss");

                    list.Add(data);
                }
                dr.Close();

                db.CloseConnection();
                return list;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Function To check document type already exist or not
        public bool Check_DocumentType(string SysCode, string DocumentType)
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_CHECK_EMPL_DOCTYP_BY_EMPCODE";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                // Add parameters
                SqlParameter P_EmpCode = new SqlParameter();
                P_EmpCode.ParameterName = "P_EmpCode";
                P_EmpCode.SqlDbType = SqlDbType.Int;
                P_EmpCode.Value = SysCode;
                db.cmd.Parameters.Add(P_EmpCode);


                SqlParameter P_DocumentType = new SqlParameter();
                P_DocumentType.ParameterName = "P_DocumentType";
                P_DocumentType.SqlDbType = SqlDbType.VarChar;
                P_DocumentType.Size = 100;
                P_DocumentType.Value = DocumentType;
                db.cmd.Parameters.Add(P_DocumentType);


                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                db.FillData(sql);


                int RespCode = Convert.ToInt32(P_CODE.Value);
                db.CloseConnection();
                if (RespCode == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Function to delete Employee Documents
        public List<Employee_Doc> Delete_Emp_Documents(string Code)
        {
            try
            {
                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_HRM_DELETE_EMPL_DOC";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_Sys_Code = new SqlParameter();
                P_Sys_Code.ParameterName = "P_Sys_Code";
                P_Sys_Code.SqlDbType = SqlDbType.Int;
                P_Sys_Code.Value = Code;
                db.cmd.Parameters.Add(P_Sys_Code);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);
                var dr = db.DataReader(sql);
                List<Employee_Doc> list = new List<Employee_Doc>();
                while (dr.Read())
                {
                    Employee_Doc data = new Employee_Doc();
                    data.sys_code = dr["Sys_Code"].ToString();
                    data.OrgCode = dr["OrgCode"] != DBNull.Value ? dr["OrgCode"].ToString() : null;
                    data.EmpCode = dr["EmpCode"] != DBNull.Value ? dr["EmpCode"].ToString() : null;
                    data.Document_Type = dr["Document_Type"] != DBNull.Value ? dr["Document_Type"].ToString() : null;
                    data.document_Name = dr["Document_Name"] != DBNull.Value ? dr["Document_Name"].ToString().Trim() : null;

                    byte[] documentBytes = dr["Documents"] as byte[];

                    var base64 = Convert.ToBase64String(documentBytes);

                    string fileName = data.document_Name ?? "document";
                    string extension = System.IO.Path.GetExtension(fileName);
                    string mimeType = CommonFunction.GetMimeType(extension);

                    //string mimeType = "application/octet-stream"; // Change this to the appropriate MIME type if known

                    data.empdocumentBase64 = $"data:{mimeType};base64,{base64}";




                    //data.Document = dr["Documents"] as byte[];
                    data.Sys_Created_By = dr["Sys_Created_By"] != DBNull.Value ? dr["Sys_Created_By"].ToString() : null;
                    data.Sys_Created_On = dr["Sys_Created_On"] != DBNull.Value ? dr["Sys_Created_On"].ToString() : null;
                    data.Sys_Modified_By = dr["Sys_Modified_By"] != DBNull.Value ? dr["Sys_Modified_By"].ToString() : null;
                    data.Sys_Modified_On = dr["Sys_Modified_On"] != DBNull.Value ? dr["Sys_Modified_On"].ToString() : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss");

                    list.Add(data);
                }
                db.CloseConnection();
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        //Family Details
        #region Function_To_Insert_Family_Details
        public List<FamilyDetails> Insert_Family_Details(FamilyDetails obj)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;


                // Access Id claim
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Access OrgCode claim
                var orgCode = user.FindFirst("OrgCode")?.Value;


                if (obj.Family_Sys_Code == "")
                {
                    obj.Family_Sys_Code = "0";
                }

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_INSERTUPDATE_FAMILY_DETAILS";
                DBHelper db = new DBHelper();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;



                SqlParameter P_FamilySysCode = new SqlParameter();
                P_FamilySysCode.ParameterName = "P_FamilySysCode";
                P_FamilySysCode.SqlDbType = SqlDbType.Int;
                P_FamilySysCode.Value = obj.Family_Sys_Code ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_FamilySysCode);

                // Add parameters
                SqlParameter P_EmpSysCode = new SqlParameter();
                P_EmpSysCode.ParameterName = "P_EmpSysCode";
                P_EmpSysCode.SqlDbType = SqlDbType.Int;
                P_EmpSysCode.Value = obj.sys_code ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_EmpSysCode);

                SqlParameter P_ORGCODE = new SqlParameter();
                P_ORGCODE.ParameterName = "P_OrgCode";
                P_ORGCODE.SqlDbType = SqlDbType.VarChar;
                P_ORGCODE.Value = orgCode ?? (object)DBNull.Value; // Assuming obj.ID is a single value, not a list
                db.cmd.Parameters.Add(P_ORGCODE);

                SqlParameter P_Relationship_Type = new SqlParameter();
                P_Relationship_Type.ParameterName = "P_Relationship_Type";
                P_Relationship_Type.SqlDbType = SqlDbType.VarChar;
                P_Relationship_Type.Size = 50;
                P_Relationship_Type.Value = obj.Relationship_Type ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Relationship_Type);

                SqlParameter P_Relationship_Name = new SqlParameter();
                P_Relationship_Name.ParameterName = "P_Relationship_Name";
                P_Relationship_Name.SqlDbType = SqlDbType.VarChar;
                P_Relationship_Name.Size = 50;
                P_Relationship_Name.Value = obj.Relationship_Name ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Relationship_Name);

                SqlParameter P_Relationship_Date_Of_Birth = new SqlParameter();
                P_Relationship_Date_Of_Birth.ParameterName = "P_Relationship_Date_Of_Birth";
                P_Relationship_Date_Of_Birth.SqlDbType = SqlDbType.Date;
                P_Relationship_Date_Of_Birth.Value = obj.Relationship_Date_Of_Birth ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Relationship_Date_Of_Birth);

                SqlParameter P_Sys_Created_By = new SqlParameter();
                P_Sys_Created_By.ParameterName = "P_Sys_Created_By";
                P_Sys_Created_By.SqlDbType = SqlDbType.VarChar;
                P_Sys_Created_By.Value = userId ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Sys_Created_By);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);

                int RespCode = Convert.ToInt32(P_CODE.Value);



                List<FamilyDetails> list = new List<FamilyDetails>();

                while (dr.Read())
                {
                    FamilyDetails s = new FamilyDetails();


                    s.sys_code = dr["Sys_Code"].ToString();
                    s.Relationship_Type = dr["Relationship_Type"].ToString();
                    s.Relationship_Name = dr["Relationship_Name"].ToString();
                    s.Relationship_Date_Of_Birth = dr["Relationship_Date_Of_Birth"].ToString();

                    list.Add(s);

                }

                db.CloseConnection();
                return list;


                //if (RespCode == 0)
                //{
                //    return ResponseMsg;
                //}
                //else
                //{
                //    return ResponseMsg;
                //}
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Function_To_Get_Family_Details_By_Emp_SysCode
        public List<FamilyDetails> Get_Family_Details_By_Emp_SysCode(string empcode)
        {
            try
            {




                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_GET_FAMILY_DETAILS_BY_EMPSYSCODE";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                // Add parameters
                SqlParameter P_EmpSysCode = new SqlParameter();
                P_EmpSysCode.ParameterName = "P_EMP_SYSCODE";
                P_EmpSysCode.SqlDbType = SqlDbType.Int;
                P_EmpSysCode.Value = empcode;
                db.cmd.Parameters.Add(P_EmpSysCode);


                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);


                int RespCode = Convert.ToInt32(P_CODE.Value);

                List<FamilyDetails> list = new List<FamilyDetails>();

                while (dr.Read())
                {
                    FamilyDetails s = new FamilyDetails();


                    s.sys_code = dr["Sys_Code"].ToString();
                    s.Relationship_Type = dr["Relationship_Type"].ToString();
                    s.Relationship_Name = dr["Relationship_Name"].ToString();
                    s.Relationship_Date_Of_Birth = dr["Relationship_Date_Of_Birth"].ToString();

                    list.Add(s);

                }

                db.CloseConnection();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Function_To_Get_Family_Details_By_Fam_SysCode
        public List<FamilyDetails> Get_Family_Details_By_Fam_SysCode(int FamSysCode)
        {
            try
            {




                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_GET_FAMILY_DETAILS_BY_FAM_SYSCODE";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                // Add parameters
                SqlParameter P_FamilySysCode = new SqlParameter();
                P_FamilySysCode.ParameterName = "P_FamilySysCode";
                P_FamilySysCode.SqlDbType = SqlDbType.Int;
                P_FamilySysCode.Value = FamSysCode;
                db.cmd.Parameters.Add(P_FamilySysCode);


                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);


                int RespCode = Convert.ToInt32(P_CODE.Value);

                List<FamilyDetails> list = new List<FamilyDetails>();

                while (dr.Read())
                {
                    FamilyDetails s = new FamilyDetails();


                    s.Family_Sys_Code = dr["Sys_Code"].ToString();
                    s.Relationship_Type = dr["Relationship_Type"].ToString();
                    s.Relationship_Name = dr["Relationship_Name"].ToString();
                    s.Relationship_Date_Of_Birth = dr["Relationship_Date_Of_Birth"].ToString();

                    list.Add(s);

                }

                db.CloseConnection();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Function to delete employee Family details
        public List<FamilyDetails> Delete_Emp_familyDetails(string Code)
        {
            try
            {
                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_HRM_DELETE_FAMILY_DETAILS";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_Sys_Code = new SqlParameter();
                P_Sys_Code.ParameterName = "P_Sys_Code";
                P_Sys_Code.SqlDbType = SqlDbType.Int;
                P_Sys_Code.Value = Code;
                db.cmd.Parameters.Add(P_Sys_Code);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);

                int RespCode = Convert.ToInt32(P_CODE.Value);



                List<FamilyDetails> list = new List<FamilyDetails>();

                while (dr.Read())
                {
                    FamilyDetails s = new FamilyDetails();


                    s.sys_code = dr["Sys_Code"].ToString();
                    s.Relationship_Type = dr["Relationship_Type"].ToString();
                    s.Relationship_Name = dr["Relationship_Name"].ToString();
                    s.Relationship_Date_Of_Birth = dr["Relationship_Date_Of_Birth"].ToString();

                    list.Add(s);

                }
                db.CloseConnection();

                return list;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Function to Insertupdate Employee Signature
        public InsertUpdateResponse InsertUpdate_EmployeeSign(Employee_Signature obj)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                var user = _httpContextAccessor.HttpContext.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }

                #region To Format Html String
                string originalHtml = obj.emp_Sign;
                string escapedHtml = originalHtml.Replace("\"", "\\\"");
                escapedHtml = escapedHtml.Replace("\r\n", "\\\\n");
                string formattedString = $"<div><pre style=\\\"cursor: auto;\\\">{escapedHtml}</pre><ul></ul></div>";
                formattedString = System.Text.RegularExpressions.Regex.Replace(formattedString, @"(?<!\\)\\(?!\\)", "");
                #endregion


                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_INSERTUPDATE_EMPLOYEE_SIGNATURE";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_Sys_Code = new SqlParameter();
                P_Sys_Code.ParameterName = "P_Sys_Code";
                P_Sys_Code.SqlDbType = SqlDbType.VarChar;
                P_Sys_Code.Value = obj.sys_Code ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Sys_Code);

                SqlParameter P_OrgCode = new SqlParameter();
                P_OrgCode.ParameterName = "P_OrgCode";
                P_OrgCode.SqlDbType = SqlDbType.VarChar;
                P_OrgCode.Size = 100;
                P_OrgCode.Value = orgCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_OrgCode);

                SqlParameter P_EmpCode = new SqlParameter();
                P_EmpCode.ParameterName = "P_Employee_Code";
                P_EmpCode.SqlDbType = SqlDbType.Int;
                P_EmpCode.Value = obj.EmpCode;
                db.cmd.Parameters.Add(P_EmpCode);

                SqlParameter P_Employee_signature = new SqlParameter();
                P_Employee_signature.ParameterName = "P_Employee_signature";
                P_Employee_signature.SqlDbType = SqlDbType.VarChar;
                P_Employee_signature.Size = -1;
                P_Employee_signature.Value = formattedString ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Employee_signature);

                SqlParameter P_UserId = new SqlParameter();
                P_UserId.ParameterName = "P_UserId";
                P_UserId.SqlDbType = SqlDbType.VarChar;
                P_UserId.Size = 200;
                P_UserId.Value = userId;
                db.cmd.Parameters.Add(P_UserId);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = -1;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);


                //var Resp_Msg = P_Message.Value.ToString();

                //ResponceMessage res = new ResponceMessage();
                //res.code = 
                //res.Message =


                InsertUpdateResponse list = new InsertUpdateResponse();

                while (dr.Read())
                {
                    Employee_Signature data = new Employee_Signature();
                    data.sys_Code = dr["Sys_Code"].ToString();
                    data.OrgCode = dr["Organization_Name"] != DBNull.Value ? dr["Organization_Name"].ToString() : null;
                    data.EmpCode = dr["Employee_Full_Name"] != DBNull.Value ? dr["Employee_Full_Name"].ToString() : null;
                    data.emp_Sign = dr["Employee_signature"] != DBNull.Value ? dr["Employee_signature"].ToString() : null;
                    data.Sys_CreatedBy = dr["Created_By"] != DBNull.Value ? dr["Created_By"].ToString() : null;
                    data.Sys_CreatedOn = dr["Created_On"] != DBNull.Value ? dr["Created_On"].ToString() : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss");
                    data.Sys_ModifiedBy = dr["Modified_By"] != DBNull.Value ? dr["Modified_By"].ToString() : null;
                    data.Sys_ModifiedOn = dr["Modified_On"] != DBNull.Value ? dr["Modified_On"].ToString() : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss");
                    list.SignaturesList.Add(data);
                }

                dr.Close();
                list.ResponseCode = Convert.ToInt32(db.cmd.Parameters["P_CODE"].Value);
                db.CloseConnection();

                return list;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Function To get All employee Signature
        public List<Employee_Signature> Get_All_EmployeeSign()
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                var user = _httpContextAccessor.HttpContext.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM__GET_ALL_EMPLOYEE_SIGNATURE";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_OrgCode = new SqlParameter();
                P_OrgCode.ParameterName = "P_OrgCode";
                P_OrgCode.SqlDbType = SqlDbType.VarChar;
                P_OrgCode.Value = P_OrgCode;
                P_OrgCode.Value = orgCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_OrgCode);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);


                var dr = db.DataReader(sql);
                List<Employee_Signature> list = new List<Employee_Signature>();

                while (dr.Read())
                {
                    Employee_Signature data = new Employee_Signature();
                    data.sys_Code = dr["Sys_Code"].ToString();
                    data.OrgCode = dr["Organization_Name"] != DBNull.Value ? dr["Organization_Name"].ToString() : null;
                    data.EmpCode = dr["Employee_Full_Name"] != DBNull.Value ? dr["Employee_Full_Name"].ToString() : null;
                    data.emp_Sign = dr["Employee_signature"] != DBNull.Value ? dr["Employee_signature"].ToString() : null;
                    data.Sys_CreatedBy = dr["Created_By"] != DBNull.Value ? dr["Created_By"].ToString() : null;
                    data.Sys_CreatedOn = dr["Created_On"] != DBNull.Value ? dr["Created_On"].ToString() : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss");
                    data.Sys_ModifiedBy = dr["Modified_By"] != DBNull.Value ? dr["Modified_By"].ToString() : null;
                    data.Sys_ModifiedOn = dr["Modified_On"] != DBNull.Value ? dr["Modified_On"].ToString() : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss");
                    list.Add(data);
                }

                dr.Close();
                db.CloseConnection();
                return list;

            }
            catch (Exception ex)
            {

                throw;
            }

        }
        #endregion

        #region Function to get Employe Signature By Sys_Code
        public Employee_Signature Get__EmployeeSignBy_SysCode(string SysCode)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                var user = _httpContextAccessor.HttpContext.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM__GET_ALL_EMPLOYEE_SIGNATURE_BY_ID";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_Sys_Code = new SqlParameter();
                P_Sys_Code.ParameterName = "P_Sys_Code";
                P_Sys_Code.SqlDbType = SqlDbType.VarChar;
                P_Sys_Code.Value = SysCode;
                db.cmd.Parameters.Add(P_Sys_Code);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);


                var dr = db.DataReader(sql);

                Employee_Signature data = new Employee_Signature();
                while (dr.Read())
                {

                    data.sys_Code = dr["Sys_Code"].ToString();
                    data.OrgCode = dr["Organization_Name"] != DBNull.Value ? dr["Organization_Name"].ToString() : null;
                    data.EmpCode = dr["EmpCode"] != DBNull.Value ? dr["EmpCode"].ToString() : null;
                    data.emp_Sign = dr["Employee_signature"] != DBNull.Value ? dr["Employee_signature"].ToString() : null;
                    data.Sys_CreatedBy = dr["Created_By"] != DBNull.Value ? dr["Created_By"].ToString() : null;
                    data.Sys_CreatedOn = dr["Created_On"] != DBNull.Value ? dr["Created_On"].ToString() : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss");
                    data.Sys_ModifiedBy = dr["Modified_By"] != DBNull.Value ? dr["Modified_By"].ToString() : null;
                    data.Sys_ModifiedOn = dr["Modified_On"] != DBNull.Value ? dr["Modified_On"].ToString() : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss");

                }

                dr.Close();
                db.CloseConnection();
                return data;

            }
            catch (Exception ex)
            {

                throw;
            }

        }
        #endregion

        #region Function to delete employee Signature
        public List<Employee_Signature> Delete_Emp_Signature(string Code)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (string.IsNullOrEmpty(orgCode))
                {
                    orgCode = null;
                }

                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_HRM_DELETE_EMPLOYEE_SIGNATURE";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_Sys_Code = new SqlParameter
                {
                    ParameterName = "P_Sys_Code",
                    SqlDbType = SqlDbType.Int,
                    Value = int.Parse(Code) // Ensure Code is an integer or parse it
                };
                db.cmd.Parameters.Add(P_Sys_Code);

                SqlParameter P_OrgCode = new SqlParameter
                {
                    ParameterName = "P_OrgCode",
                    SqlDbType = SqlDbType.VarChar,
                    Value = orgCode ?? (object)DBNull.Value // Assign the actual value or DBNull
                };
                db.cmd.Parameters.Add(P_OrgCode);

                SqlParameter P_CODE = new SqlParameter
                {
                    ParameterName = "P_CODE",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter
                {
                    ParameterName = "P_MESSAGE",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 200,
                    Direction = ParameterDirection.Output
                };
                db.cmd.Parameters.Add(P_Message);

                var dr = db.DataReader(sql);

                List<Employee_Signature> list = new List<Employee_Signature>();

                while (dr.Read())
                {
                    Employee_Signature data = new Employee_Signature
                    {
                        sys_Code = dr["Sys_Code"].ToString(),
                        OrgCode = dr["Organization_Name"] != DBNull.Value ? dr["Organization_Name"].ToString() : null,
                        EmpCode = dr["Employee_Full_Name"] != DBNull.Value ? dr["Employee_Full_Name"].ToString() : null,
                        emp_Sign = dr["Employee_signature"] != DBNull.Value ? dr["Employee_signature"].ToString() : null,
                        Sys_CreatedBy = dr["Created_By"] != DBNull.Value ? dr["Created_By"].ToString() : null,
                        Sys_CreatedOn = dr["Created_On"] != DBNull.Value ? dr["Created_On"].ToString() : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss"),
                        Sys_ModifiedBy = dr["Modified_By"] != DBNull.Value ? dr["Modified_By"].ToString() : null,
                        Sys_ModifiedOn = dr["Modified_On"] != DBNull.Value ? dr["Modified_On"].ToString() : new DateTime(2000, 1, 1).ToString("yyyy-MM-dd HH:mm:ss")
                    };

                    list.Add(data);
                }
                db.CloseConnection();
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
