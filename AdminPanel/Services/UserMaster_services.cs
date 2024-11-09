
using System.Data.SqlClient;
using System.Data;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http.HttpResults;

using System.Net.Sockets;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;
using  AdminPanel.Models;
using AdminPanel.CommonRepo;

namespace AdminPanel.Services
{
    public interface IUserMaster
    {
        ResponceMessage InserUpdate(VMUserMaser obj, IFormFile Logo);
        VMUserMaser USERMASTERBYID(string id);
        List<VMUserMaser> GETLISTUSERMASTER();
        ResponceMessage DELETEUSERMASTER(string id);
        List<SelectListItem> DropDownSQ();
        List<SelectListItem> GetEmpCodeEmpName();
        List<SelectListItem> GetOrganizationDropDownbyorgid();

        DataTable GetUsermasterDataTomail(string id);
       // List<TempletConfigration> GETEmailCONFIGRATION();

    }
    public class UserMaster_services : IUserMaster
    {
        IConfiguration configuration;

        public readonly IHttpContextAccessor _httpContextAccessor;
        public UserMaster_services(IConfiguration _configuration, IHttpContextAccessor httpContextAccessor)
        {
            configuration = _configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        #region Dropdown Bind ORGMASTER
        public List<SelectListItem> GetOrganizationDropDownbyorgid()
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;
                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }

                List<SelectListItem> list = new List<SelectListItem>();

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "GEN_GETALL_ORGNIZATION_BYORGID";
                DBHelper db = new DBHelper();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter P_ORGCODE = new SqlParameter();
                P_ORGCODE.ParameterName = "P_ORGCODE";
                P_ORGCODE.SqlDbType = SqlDbType.VarChar;
                P_ORGCODE.Size = 30;
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
                while (dr.Read())
                {
                    SelectListItem o = new SelectListItem();
                    o.Value = dr["Sys_Code"].ToString();
                    o.Text = dr["Organization_Name"].ToString();
                    list.Add(o);

                }

                int RespCode = Convert.ToInt32(P_CODE.Value);
                // string RespMsg = P_Message.Value.ToString();

                db.CloseConnection();
                if (RespCode == 0)
                {
                    return list;
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


        #region Delete User Master
        public ResponceMessage DELETEUSERMASTER(string id)
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_DELETE_USERMASTER";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_ID = new SqlParameter();
                P_ID.ParameterName = "P_ID";
                P_ID.SqlDbType = SqlDbType.VarChar;
                P_ID.Value = id;
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
                var dr = db.FillData(sql);
                string msg = P_Message.Value.ToString();
                int code = Convert.ToInt32(P_CODE.Value);
                ResponceMessage data = new ResponceMessage();
                data.code = code;
                data.Message = msg;
                db.CloseConnection();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Bind Sequrity Question
        public List<SelectListItem> DropDownSQ()
        {
            try
            {

                #region Get Data in state  to bind Dropdown 
                try
                {
                    List<SelectListItem> list = new List<SelectListItem>();

                    string ConString = configuration.GetConnectionString("Myconnection");
                    string sql = "USP_ADM_GETALL_SECURITYQUESTION";
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
                    while (dr.Read())
                    {
                        SelectListItem o = new SelectListItem();
                        o.Value = dr["Code"].ToString();
                        o.Text = dr["Question"].ToString();
                        list.Add(o);

                    }
                    db.CloseConnection();

                    return list;
                }
                catch (Exception ex)
                {

                    throw;
                }

                #endregion
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion



        #region  Get All User MAsters
        public List<VMUserMaser> GETLISTUSERMASTER()
        {
            #region Get All list Of USermaster 
            try
            {
                var user = _httpContextAccessor.HttpContext.User;
                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_GETALL_USERMASTER";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p_OrgCode = new SqlParameter();
                p_OrgCode.ParameterName = "p_OrgCode";
                p_OrgCode.SqlDbType = SqlDbType.VarChar;
                p_OrgCode.Size = 30;
                p_OrgCode.Value = orgCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(p_OrgCode);

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

                List<VMUserMaser> list = new List<VMUserMaser>();

                while (dr.Read())
                {
                    VMUserMaser data = new VMUserMaser();

                    data.UserID = dr["UserID"].ToString();
                    data.OrgCode = dr["OrgCode"].ToString();
                    data.UserName = dr["UserName"].ToString();
                    data.EmpCode = dr["First_Name"].ToString();

                    data.LoginAttempts = dr["LoginAttempts"] != DBNull.Value ? Convert.ToInt32(dr["LoginAttempts"]) : 0;
                    data.MultipleDevice = dr["MultipleDevice"] != DBNull.Value ? Convert.ToInt32(dr["MultipleDevice"]) : 0;
                    data.UserLoginType = dr["UserLoginType"].ToString();
                    data.UserTypeCode = dr["UserTypeCode"].ToString();
                    data.SecurityAnswer = dr["SecurityAnswer"].ToString();
                    data.Password = dr["Password"].ToString();
                    data.SecurityQuestion = dr["SecurityQuestion"].ToString();
                    data.Status = dr["Status"].ToString();

                    // Handle the CreationDate as a DateTime
                    if (dr["CreationDate"] != DBNull.Value)
                    {
                        data.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                    }

                    // Handle IsAdministrator field properly
                    data.IsAdministrator = dr["IsAdministrator"] != DBNull.Value && Convert.ToBoolean(dr["IsAdministrator"]);

                    list.Add(data);
                }


                db.CloseConnection();
                return list;
            }
            catch (Exception)
            {

                throw;
            }

            #endregion 
        }
        #endregion

        #region Insert or Update User MAster In Db
        public ResponceMessage InserUpdate(VMUserMaser OBJ, IFormFile Logo)
        {
            var user = _httpContextAccessor.HttpContext.User;

            var userIds = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Logo != null && Logo.Length > 0)
            {
                // Read the file into a byte array
                byte[] logoBytes;
                using (var memoryStream = new MemoryStream())
                {
                    Logo.CopyTo(memoryStream);
                    logoBytes = memoryStream.ToArray();
                }

                // Assuming Organization has a property to hold the byte array of the logo
                OBJ.ImageName = logoBytes;
            }
            #region Insert USerMAster Data in Db
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_INSERTUPDATE_USERMASTER";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_USERID = new SqlParameter();
                P_USERID.ParameterName = "P_USERID";
                P_USERID.SqlDbType = SqlDbType.VarChar;
                P_USERID.Value = OBJ.UserID ?? (object)DBNull.Value;
                P_USERID.Size = 250;
                db.cmd.Parameters.Add(P_USERID);

                SqlParameter P_OrgCode = new SqlParameter();
                P_OrgCode.ParameterName = "P_OrgCode";
                P_OrgCode.SqlDbType = SqlDbType.VarChar;
                P_OrgCode.Size = 250;
                P_OrgCode.Value = OBJ.OrgCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_OrgCode);

                SqlParameter P_USERNAME = new SqlParameter();
                P_USERNAME.ParameterName = "P_USERNAME";
                P_USERNAME.SqlDbType = SqlDbType.VarChar;
                P_USERNAME.Size = 350;
                P_USERNAME.Value = OBJ.UserName ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_USERNAME);


                SqlParameter P_EMPCODE = new SqlParameter();
                P_EMPCODE.ParameterName = "P_EMPCODE";
                P_EMPCODE.SqlDbType = SqlDbType.Int;
                P_EMPCODE.Value = OBJ.EmpCode ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_EMPCODE);

                SqlParameter P_USERlOGINTYPE = new SqlParameter();
                P_USERlOGINTYPE.ParameterName = "P_USERlOGINTYPE";
                P_USERlOGINTYPE.SqlDbType = SqlDbType.VarChar;
                P_USERlOGINTYPE.Size = 200;
                P_USERlOGINTYPE.Value = OBJ.UserLoginType ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_USERlOGINTYPE);

                SqlParameter P_USERTYPECODE = new SqlParameter();
                P_USERTYPECODE.ParameterName = "P_USERTYPECODE";
                P_USERTYPECODE.SqlDbType = SqlDbType.VarChar;
                P_USERTYPECODE.Size = 200;
                P_USERTYPECODE.Value = OBJ.UserTypeCode ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_USERTYPECODE);

                SqlParameter P_ISADMIN = new SqlParameter();
                P_ISADMIN.ParameterName = "P_ISADMIN";
                P_ISADMIN.SqlDbType = SqlDbType.Bit;
                P_ISADMIN.Value = OBJ.IsAdmin ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_ISADMIN);

                SqlParameter P_LOGINATTEMPTS = new SqlParameter();
                P_LOGINATTEMPTS.ParameterName = "P_LOGINATTEMPTS";
                P_LOGINATTEMPTS.SqlDbType = SqlDbType.Int;
                P_LOGINATTEMPTS.Value = OBJ.LoginAttempts;
                db.cmd.Parameters.Add(P_LOGINATTEMPTS);

                SqlParameter P_PASSWORD = new SqlParameter();
                P_PASSWORD.ParameterName = "P_PASSWORD";
                P_PASSWORD.SqlDbType = SqlDbType.VarChar;
                P_PASSWORD.Size = 200;
                P_PASSWORD.Value = OBJ.Password ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_PASSWORD);

                SqlParameter P_ISADUSER = new SqlParameter();
                P_ISADUSER.ParameterName = "P_ISADUSER";
                P_ISADUSER.SqlDbType = SqlDbType.Bit;
                P_ISADUSER.Value = OBJ.IsADUser ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_ISADUSER);

                SqlParameter P_MULTIPLEDEVICE = new SqlParameter();
                P_MULTIPLEDEVICE.ParameterName = "P_MULTIPLEDEVICE";
                P_MULTIPLEDEVICE.SqlDbType = SqlDbType.Int;
                P_MULTIPLEDEVICE.Value = OBJ.MultipleDevice;
                db.cmd.Parameters.Add(P_MULTIPLEDEVICE);


                SqlParameter P_SEQUESTION = new SqlParameter();
                P_SEQUESTION.ParameterName = "P_SEQUESTION";
                P_SEQUESTION.SqlDbType = SqlDbType.Int;
                P_SEQUESTION.Value = OBJ.SecurityQuestion ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_SEQUESTION);

                SqlParameter P_SEQANSWER = new SqlParameter();
                P_SEQANSWER.ParameterName = "P_SEQANSWER";
                P_SEQANSWER.SqlDbType = SqlDbType.VarChar;
                P_SEQANSWER.Size = 500;
                P_SEQANSWER.Value = OBJ.SecurityAnswer ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_SEQANSWER);

                SqlParameter P_ISADMINISTRATOR = new SqlParameter();
                P_ISADMINISTRATOR.ParameterName = "P_ISADMINISTRATOR";
                P_ISADMINISTRATOR.SqlDbType = SqlDbType.Bit;
                P_ISADMINISTRATOR.Value = OBJ.IsAdministrator ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_ISADMINISTRATOR);

                SqlParameter P_ISEDIT = new SqlParameter();
                P_ISEDIT.ParameterName = "P_ISEDIT";
                P_ISEDIT.SqlDbType = SqlDbType.Bit;
                P_ISEDIT.Value = OBJ.IsEdit ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_ISEDIT);

                SqlParameter P_DELETE = new SqlParameter();
                P_DELETE.ParameterName = "P_DELETE";
                P_DELETE.SqlDbType = SqlDbType.Bit;
                P_DELETE.Value = OBJ.IsDelete ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_DELETE);

                SqlParameter P_Status = new SqlParameter();
                P_Status.ParameterName = "P_Status";
                P_Status.SqlDbType = SqlDbType.VarChar;
                P_Status.Size = 30;
                P_Status.Value = OBJ.Status ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_Status);

                SqlParameter P_CREATEDBY = new SqlParameter();
                P_CREATEDBY.ParameterName = "P_CREATEDBY";
                P_CREATEDBY.SqlDbType = SqlDbType.VarChar;
                P_CREATEDBY.Size = 500;
                P_CREATEDBY.Value = userIds ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_CREATEDBY);

                SqlParameter p_Photo = new SqlParameter();
                p_Photo.ParameterName = "p_Photo";
                p_Photo.SqlDbType = SqlDbType.VarBinary;
                p_Photo.Size = 6000;
                p_Photo.Value = OBJ.ImageName ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(p_Photo);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.NVarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                SqlParameter p_syscode = new SqlParameter();
                p_syscode.ParameterName = "p_syscode";
                p_syscode.SqlDbType = SqlDbType.VarChar;
                p_syscode.Size = 200;
                p_syscode.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(p_syscode);

                var obj = db.FillData(sql);
                string msg = P_Message.Value.ToString();
                int code = Convert.ToInt32(P_CODE.Value);

                ResponceMessage data = new ResponceMessage();
                data.code = code;
                data.Message = msg;
                data.Sys_Code = p_syscode.Value.ToString();


                db.CloseConnection();
                return data;

            }
            catch (Exception er)
            {

                throw;
            }

            #endregion
        }
        #endregion

        #region Get User By ID in DB

        public VMUserMaser USERMASTERBYID(string id)
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_GET_USERMASTER_BYID";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_USERID = new SqlParameter();
                P_USERID.ParameterName = "P_USERID";
                P_USERID.SqlDbType = SqlDbType.VarChar;
                P_USERID.Value = id;
                db.cmd.Parameters.Add(P_USERID);


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
                VMUserMaser data = new VMUserMaser();
                while (dr.Read())
                {
                    data.UserID = dr["UserID"].ToString();
                    data.OrgCode = dr["OrgCode"].ToString();
                    data.UserName = dr["UserName"].ToString();
                    data.EmpCode = dr["EmpCode"].ToString();
                    data.UserLoginType = dr["UserLoginType"].ToString();
                    data.UserTypeCode = dr["UserTypeCode"].ToString();
                    data.SecurityAnswer = dr["SecurityAnswer"].ToString();
                    data.Password = dr["Password"].ToString();
                    data.LoginAttempts = dr["LoginAttempts"] != DBNull.Value ? Convert.ToInt32(dr["LoginAttempts"]) : 0;
                    data.MultipleDevice = dr["MultipleDevice"] != DBNull.Value ? Convert.ToInt32(dr["MultipleDevice"]) : 0;
                    data.SecurityQuestion = dr["SecurityQuestion"].ToString();
                    data.IsAdministrator = Convert.ToBoolean(dr.IsDBNull(dr.GetOrdinal("IsAdministrator")) ? (bool?)null : dr.GetBoolean(dr.GetOrdinal("IsAdministrator")));
                    data.IsADUser = Convert.ToBoolean(dr.IsDBNull(dr.GetOrdinal("IsADUser")) ? (bool?)null : dr.GetBoolean(dr.GetOrdinal("IsADUser")));
                    data.IsEdit = Convert.ToBoolean(dr.IsDBNull(dr.GetOrdinal("IsEdit")) ? (bool?)null : dr.GetBoolean(dr.GetOrdinal("IsEdit")));
                    data.IsDelete = Convert.ToBoolean(dr.IsDBNull(dr.GetOrdinal("IsDelete")) ? (bool?)null : dr.GetBoolean(dr.GetOrdinal("IsDelete")));
                    data.Status = dr["Status"].ToString();
                    data.IsAdmin = Convert.ToBoolean(dr.IsDBNull(dr.GetOrdinal("IsAdmin")) ? (bool?)null : dr.GetBoolean(dr.GetOrdinal("IsAdmin")));

                }
                db.CloseConnection();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region GetEmpCodeEmpName
        public List<SelectListItem> GetEmpCodeEmpName()
        {
            try
            {
                try
                {

                    var user = _httpContextAccessor.HttpContext.User;

                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    string orgCode = user.FindFirst("OrgCode")?.Value;

                    if (orgCode == "" || orgCode == null)
                    {
                        orgCode = null;
                    }
                    List<SelectListItem> list = new List<SelectListItem>();

                    string ConString = configuration.GetConnectionString("Myconnection");
                    string sql = "USP_ADM_GETALL_EMPCODE&EMPNAME";
                    DBHelper db = new DBHelper();
                    db.cmd.Parameters.Clear();
                    db.OpenConnection(ConString);
                    db.cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter P_Org_Sys_Code = new SqlParameter();
                    P_Org_Sys_Code.ParameterName = "P_Org_Sys_Code";
                    P_Org_Sys_Code.SqlDbType = SqlDbType.VarChar;
                    P_Org_Sys_Code.Size = 30;
                    P_Org_Sys_Code.Value = orgCode ?? (object)DBNull.Value;
                    db.cmd.Parameters.Add(P_Org_Sys_Code);


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
                    while (dr.Read())
                    {
                        SelectListItem o = new SelectListItem();
                        o.Value = dr["Sys_Code"].ToString();
                        o.Text = dr["Code_First_Name"].ToString();
                        list.Add(o);

                    }
                    db.CloseConnection();

                    return list;
                }
                catch (Exception ex)
                {

                    throw;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion


        #region  get data ti Send Email to user Creation

        public DataTable GetUsermasterDataTomail(string id)
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_GETEMAILDATA_USERMASTER_BYID";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_USERID = new SqlParameter();
                P_USERID.ParameterName = "P_USERID";
                P_USERID.SqlDbType = SqlDbType.VarChar;
                P_USERID.Size = 30;
                P_USERID.Value = id ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_USERID);

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

                DataSet Dt = db.DaSet(sql);

                DataTable table = Dt.Tables[0];

                return table;


            }
            catch (Exception)
            {

                throw;
            }




        }


        //public List<TempletConfigration> GETEmailCONFIGRATION()
        //{
        //    try
        //    {
        //        var user = _httpContextAccessor.HttpContext.User;
        //        string UserCode = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //        string orgCode = user.FindFirst("OrgCode")?.Value;

        //        if (orgCode == "" || orgCode == null)
        //        {
        //            orgCode = null;
        //        }
        //        string ConString = configuration.GetConnectionString("MyConnection");
        //        string sql = "USP_GET_ALL_TEMPLATE_CONFIGURATION";
        //        DBHelper db = new DBHelper();
        //        db.OpenConnection(ConString);
        //        db.cmd.CommandType = CommandType.StoredProcedure;

        //        SqlParameter P_DocType = new SqlParameter();
        //        P_DocType.ParameterName = "P_DocType";
        //        P_DocType.SqlDbType = SqlDbType.NVarChar;
        //        P_DocType.Size = 4000;
        //        P_DocType.Value = "UserEamil";
        //        db.cmd.Parameters.Add(P_DocType);

        //        // Output parameters
        //        SqlParameter P_CODE = new SqlParameter();
        //        P_CODE.ParameterName = "P_CODE";
        //        P_CODE.SqlDbType = SqlDbType.Int;
        //        P_CODE.Direction = ParameterDirection.Output;
        //        db.cmd.Parameters.Add(P_CODE);

        //        SqlParameter P_MESSAGE = new SqlParameter();
        //        P_MESSAGE.ParameterName = "P_MESSAGE";
        //        P_MESSAGE.SqlDbType = SqlDbType.NVarChar;
        //        P_MESSAGE.Size = 4000;
        //        P_MESSAGE.Direction = ParameterDirection.Output;
        //        db.cmd.Parameters.Add(P_MESSAGE);


        //        var reader = db.DataReader(sql);
        //        List<TempletConfigration> data = new List<TempletConfigration>();

        //        while (reader.Read())
        //        {
        //            TempletConfigration Obj = new TempletConfigration();
        //            Obj.PlaceHolderName = reader["PlaceHolder_Name"] != DBNull.Value ? reader["PlaceHolder_Name"].ToString().Replace("\r", "") : "";
        //            Obj.DbColoumnName = reader["DB_Column_Name"] != DBNull.Value ? reader["DB_Column_Name"].ToString() : "";
        //            Obj.fsize = reader["Font_Size"] != DBNull.Value ? Convert.ToInt32(reader["Font_Size"]) : 0;
        //            Obj.Ismultiline = reader["Is_Multiline"] != DBNull.Value ? Convert.ToBoolean(reader["Is_Multiline"]) : false;
        //            Obj.Isimage = reader["Is_Image"] != DBNull.Value ? Convert.ToBoolean(reader["Is_Image"]) : false;
        //            Obj.Multisize = reader["Multisize"] != DBNull.Value ? Convert.ToInt32(reader["Multisize"]) : 0;



        //            data.Add(Obj);

        //        }
        //        db.CloseConnection();
        //        return data;






        //    }
        //    catch (Exception er)
        //    {

        //        throw;
        //    }
        //}

        #endregion



    }
}
