using AdminPanel.CommonRepo;
using  AdminPanel.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Syncfusion.EJ2.Grids;
using System.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using UAParser;

namespace AdminPanel.Services
{

    public interface IDeviceDetailse
    {

        List<DeviceDetails> DeviceDetailseList();
        public List<DeviceDetails> Get_Device_Details_BY_USER_CODE(string ActiveUserCode);

        public int Deactive_Priveous_Device(string userCode, string isChecked, string LogedinUser);

        //for user
        //public DeviceDetails Get_Device_Details_By_Sys_Code();

        //for admin
        public List<SelectListItem> Get_All_UserMaster_Dropdown();

        public DeviceDetails Get_Device_Details_By_Sys_Code(string sysCode);

        public ResponceMessage Deactive_Device_By_Admin(string userCode, string isChecked, string LogedinUser);

        public List<DeviceDetails> Get_Device_Details_BY_USER_CODE_Admin(string ActiveUserCode);


        public (string OS, string Browser) GetUserAgentInfo(HttpRequest request);

    }
    public class DeviceDetailse_Service : IDeviceDetailse
    {
        IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeviceDetailse_Service(IConfiguration _configuration, IHttpContextAccessor httpContextAccessor)
        {
            configuration = _configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Get All DeviceDetailse List
        public List<DeviceDetails> DeviceDetailseList()
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;
                string UserCode = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //string OrgCode = user.FindFirst("OrgCode")?.Value;

                string ConString = configuration.GetConnectionString("MyConnection");
                string sql = "USP_GEN_GET_DEVICE_DETAILS_BY_USERCODE]";
                DBHelper db = new DBHelper();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_MESSAGE = new SqlParameter();
                P_MESSAGE.ParameterName = "P_MESSAGE";
                P_MESSAGE.SqlDbType = SqlDbType.NVarChar;
                P_MESSAGE.Size = 4000;
                P_MESSAGE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_MESSAGE);


                var dr = db.DataReader(sql);

                List<DeviceDetails> List = new List<DeviceDetails>();

                while (dr.Read())
                {

                    DeviceDetails DeviceDetailsInfo = new DeviceDetails();
                    DeviceDetailsInfo.Sys_Code = dr["Sys_Code"].ToString();
                    DeviceDetailsInfo.UserCode = dr["UserCode"] as string ?? string.Empty;
                    DeviceDetailsInfo.BiosSerialNo = dr["BiosSerialNo"] as string ?? string.Empty;
                    DeviceDetailsInfo.MotherboardSerialNo = dr["MotherboardSerialNo"] as string ?? string.Empty;
                    DeviceDetailsInfo.AuthCode = dr["AuthCode"] as string ?? string.Empty;
                    DeviceDetailsInfo.Device_Flag = dr["Device_Flag"] as string ?? string.Empty;
                    DeviceDetailsInfo.DeviceFlagBool = dr["Device_Flag"] != DBNull.Value ? Convert.ToBoolean(dr["Device_Flag"]) : (bool?)null;
                    DeviceDetailsInfo.DeviceUserName = dr["DeviceUserName"] as string ?? string.Empty;
                    DeviceDetailsInfo.Sys_Created_By = dr["Sys_Created_By"] as string ?? string.Empty;
                    DeviceDetailsInfo.Sys_Created_On = dr["Sys_Created_On"] != DBNull.Value ? Convert.ToDateTime(dr["Sys_Created_On"]) : DateTime.MinValue;
                    List.Add(DeviceDetailsInfo);


                }


                int RespCode = Convert.ToInt32(P_CODE.Value);
                db.CloseConnection();

                if (RespCode == 0)
                {
                    return List;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion






        #region Get_Device_Details_BY_USER_CODE
        public List<DeviceDetails> Get_Device_Details_BY_USER_CODE(string ActiveUserCode)
        {
            try
            {



                var user = _httpContextAccessor.HttpContext.User;

                // Access Id claim
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (ActiveUserCode == null || ActiveUserCode == "")
                {
                    ActiveUserCode = userId;
                }


                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_GET_DEVICE_DETAILS_BY_USERCODE";
                DBHelper db = new DBHelper();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_UserCode = new SqlParameter();
                P_UserCode.ParameterName = "P_UserCode ";
                P_UserCode.SqlDbType = SqlDbType.VarChar;
                P_UserCode.Size = 10;
                P_UserCode.Direction = ParameterDirection.Input;
                P_UserCode.Value = ActiveUserCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_UserCode);


                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "@P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "@P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);


                var dr = db.DataReader(sql);

                int RespCode = Convert.ToInt32(P_CODE.Value);

                List<DeviceDetails> list = new List<DeviceDetails>();

                while (dr.Read())
                {
                    DeviceDetails s = new DeviceDetails();


                    s.Sys_Code = dr["Sys_Code"].ToString();
                    //s.MotherboardSerialNo = dr["MotherboardSerialNo"].ToString();
                    //s.BiosSerialNo = dr["BiosSerialNo"].ToString();
                    //s.DeviceUserName = dr["DeviceUserName"].ToString();
                    s.LogedinUser = dr["UserCode"].ToString();
                    s.OS = dr["OS"].ToString();
                    s.Browser = dr["Browser"].ToString();
                    s.Location = dr["Loction"].ToString();
                    s.LastLogin = dr["LastLogin"] != DBNull.Value ? Convert.ToDateTime(dr["LastLogin"]) : (DateTime?)null;

                    if (dr["Device_Flag"].ToString() == "1")
                    {
                        s.Device_Flag = "Active";
                    }
                    else
                    {
                        s.Device_Flag = "De-Active";
                    }

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



        #region Function_To_Deactive_Priveous_Device
        public int Deactive_Priveous_Device(string userCode, string isChecked, string LogedinUser)
        {
            try
            {


                //var user = _httpContextAccessor.HttpContext.User;

                //// Access Id claim
                //var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;


                string Con_String = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);
                string sql = "USP_GEN_DEACTIVE_DEVICE_BY_UPDATION";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                SqlParameter P_Device_Sys_Code = new SqlParameter();
                P_Device_Sys_Code.ParameterName = "P_Device_Sys_Code";
                P_Device_Sys_Code.SqlDbType = SqlDbType.VarChar;
                P_Device_Sys_Code.Value = userCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Device_Sys_Code);


                SqlParameter P_isChecked = new SqlParameter();
                P_isChecked.ParameterName = "P_isChecked";
                P_isChecked.SqlDbType = SqlDbType.VarChar;
                P_isChecked.Value = isChecked ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_isChecked);

                SqlParameter P_UserCode = new SqlParameter();
                P_UserCode.ParameterName = "P_UserCode";
                P_UserCode.SqlDbType = SqlDbType.VarChar;
                P_UserCode.Value = LogedinUser ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_UserCode);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);


                SqlParameter P_MESSAGE = new SqlParameter();
                P_MESSAGE.ParameterName = "P_MESSAGE";
                P_MESSAGE.Size = 2000;
                P_MESSAGE.SqlDbType = SqlDbType.VarChar;
                P_MESSAGE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_MESSAGE);


                var dr = db.FillData(sql);

                int RespCode = Convert.ToInt32(P_CODE.Value);

                var RespMsg = P_MESSAGE.Value.ToString();


                db.CloseConnection();
                return RespCode;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        //// for user

        //#region Function_To_Get_Device_Details_By_Sys_Code For User
        //public DeviceDetails Get_Device_Details_By_Sys_Code()
        //{
        //    try
        //    {
        //        var user = _httpContextAccessor.HttpContext.User;
        //        string UserCode = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //        string ConString = configuration.GetConnectionString("Myconnection");
        //        string sql = "USP_GEN_GET_DEVICE_DETAILS_BY_SYS_CODE";
        //        DBHelper db = new DBHelper();
        //        db.cmd.Parameters.Clear();
        //        db.OpenConnection(ConString);
        //        db.cmd.CommandType = CommandType.StoredProcedure;


        //        SqlParameter SYS_CODE = new SqlParameter();
        //        SYS_CODE.ParameterName = "P_Sys_Code";
        //        SYS_CODE.SqlDbType = SqlDbType.VarChar;
        //        SYS_CODE.Value = UserCode ?? (object)DBNull.Value;
        //        SYS_CODE.Size = 250;
        //        db.cmd.Parameters.Add(SYS_CODE);

        //        SqlParameter P_CODE = new SqlParameter();
        //        P_CODE.ParameterName = "P_CODE";
        //        P_CODE.SqlDbType = SqlDbType.Int;
        //        P_CODE.Direction = ParameterDirection.Output;
        //        db.cmd.Parameters.Add(P_CODE);

        //        SqlParameter P_Message = new SqlParameter();
        //        P_Message.ParameterName = "P_MESSAGE";
        //        P_Message.SqlDbType = SqlDbType.VarChar;
        //        P_Message.Size = 200;
        //        P_Message.Direction = ParameterDirection.Output;
        //        db.cmd.Parameters.Add(P_Message);

        //        var dr = db.DataReader(sql);

        //        int RespCode = Convert.ToInt32(P_CODE.Value);

        //        DeviceDetails s = new DeviceDetails();


        //        while (dr.Read())
        //        {



        //            s.Sys_Code = dr["Sys_Code"].ToString();
        //            s.MotherboardSerialNo = dr["MotherboardSerialNo"].ToString();
        //            s.BiosSerialNo = dr["BiosSerialNo"].ToString();
        //            s.LogedinUser = dr["UserCode"].ToString();

        //            if (dr["Device_Flag"].ToString() == "1")
        //            {
        //                s.DeviceFlagBool = true;
        //            }
        //            else
        //            {
        //                s.DeviceFlagBool = false;
        //            }


        //        }
        //        db.CloseConnection();
        //        return s;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //#endregion




        // for admin
        #region Function_To_Get_All_UserMaster_Dropdown
        public List<SelectListItem> Get_All_UserMaster_Dropdown()
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_GETALL_USERMASTER_FOR_DEVICE_DETAILS";
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

                List<SelectListItem> list = new List<SelectListItem>();

                while (dr.Read())
                {
                    SelectListItem data = new SelectListItem();


                    data.Value = dr["UserID"].ToString();
                    data.Text = dr["UserName"].ToString();



                    list.Add(data);
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get_Device_Details_By_Sys_Code
        public DeviceDetails Get_Device_Details_By_Sys_Code(string sysCode)
        {
            try
            {


                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_GET_DEVICE_DETAILS_BY_SYS_CODE";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter SYS_CODE = new SqlParameter();
                SYS_CODE.ParameterName = "P_Sys_Code";
                SYS_CODE.SqlDbType = SqlDbType.Int;
                SYS_CODE.Value = sysCode;
                SYS_CODE.Size = 250;
                db.cmd.Parameters.Add(SYS_CODE);

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

                DeviceDetails s = new DeviceDetails();


                while (dr.Read())
                {
                    s.Sys_Code = dr["Sys_Code"].ToString();
                    s.DeviceUserName = dr["DeviceUserName"].ToString();
                    s.LogedinUser = dr["UserCode"].ToString();
                    s.BiosSerialNo = dr["BiosSerialNo"].ToString();
                    s.MotherboardSerialNo = dr["MotherboardSerialNo"].ToString();
                    s.LastLogin = dr["LastLogin"] != DBNull.Value ? Convert.ToDateTime(dr["LastLogin"]) : (DateTime?)null;
                    //s.OS = dr["OS"].ToString();
                    //s.Browser = dr["Browser"].ToString();
                    //s.LastLoginDate = dr["LastLogin"] != DBNull.Value && !string.IsNullOrEmpty(dr["LastLogin"].ToString()) ? Convert.ToDateTime(dr["LastLogin"].ToString()) : new DateTime(1900, 1, 1);
                    if (dr["Device_Flag"].ToString() == "1")
                    {
                        s.DeviceFlagBool = true;
                    }
                    else
                    {
                        s.DeviceFlagBool = false;
                    }


                }

                return s;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Deactive_Device_By_Admin
        public ResponceMessage Deactive_Device_By_Admin(string userCode, string isChecked, string LogedinUser)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);
                string sql = "USP_ADM_DEACTIVE_DEVICE_BY_ADMIN";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                SqlParameter P_Device_Sys_Code = new SqlParameter();
                P_Device_Sys_Code.ParameterName = "P_Device_Sys_Code";
                P_Device_Sys_Code.SqlDbType = SqlDbType.Int;
                P_Device_Sys_Code.Value = userCode;
                db.cmd.Parameters.Add(P_Device_Sys_Code);


                SqlParameter P_isChecked = new SqlParameter();
                P_isChecked.ParameterName = "P_isChecked";
                P_isChecked.SqlDbType = SqlDbType.VarChar;
                P_isChecked.Value = isChecked ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_isChecked);

                SqlParameter P_UserCode = new SqlParameter();
                P_UserCode.ParameterName = "P_UserCode";
                P_UserCode.SqlDbType = SqlDbType.VarChar;
                P_UserCode.Value = LogedinUser;
                P_UserCode.Size = 250;
                db.cmd.Parameters.Add(P_UserCode);



                //SqlParameter P_UserCode = new SqlParameter();
                //    P_UserCode.ParameterName = "P_UserCode";
                //    P_UserCode.SqlDbType = SqlDbType.Int;
                //    P_UserCode.Value = LogedinUser;
                //    db.cmd.Parameters.Add(P_UserCode);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);


                SqlParameter P_MESSAGE = new SqlParameter();
                P_MESSAGE.ParameterName = "P_MESSAGE";
                P_MESSAGE.Size = 2000;
                P_MESSAGE.SqlDbType = SqlDbType.VarChar;
                P_MESSAGE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_MESSAGE);


                var dr = db.FillData(sql);

                ResponceMessage re = new ResponceMessage();

                int RespCode = Convert.ToInt32(P_CODE.Value);

                var RespMsg = P_MESSAGE.Value.ToString();

                re.code = RespCode;
                re.Message = RespMsg;

                return re;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Get_Device_Details_BY_USER_CODE_Admin
        public List<DeviceDetails> Get_Device_Details_BY_USER_CODE_Admin(string ActiveUserCode)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;

                // Access Id claim
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (ActiveUserCode == null)
                {
                    ActiveUserCode = userId;
                }


                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_GET_DEVICE_DETAILS_BY_USERCODE";
                DBHelper db = new DBHelper();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_UserCode = new SqlParameter();
                P_UserCode.ParameterName = "P_UserCode";
                P_UserCode.SqlDbType = SqlDbType.VarChar;
                P_UserCode.Value = ActiveUserCode;
                P_UserCode.Size = 250;
                db.cmd.Parameters.Add(P_UserCode);


                //SqlParameter P_UserCode = new SqlParameter();
                //    P_UserCode.ParameterName = "P_UserCode ";
                //    P_UserCode.SqlDbType = SqlDbType.VarChar;
                //      P_UserCode.Size = 100;
                //    P_UserCode.Direction = ParameterDirection.Input;
                //    P_UserCode.Value = ActiveUserCode;
                //    db.cmd.Parameters.Add(P_UserCode);


                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "@P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "@P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);


                var dr = db.DataReader(sql);

                int RespCode = Convert.ToInt32(P_CODE.Value);

                List<DeviceDetails> list = new List<DeviceDetails>();

                while (dr.Read())
                {
                    DeviceDetails s = new DeviceDetails();


                    s.Sys_Code = dr["Sys_Code"].ToString();
                    //s.MotherboardSerialNo = dr["MotherboardSerialNo"].ToString();
                    //s.BiosSerialNo = dr["BiosSerialNo"].ToString();
                    //s.DeviceUserName = dr["DeviceUserName"].ToString();
                    s.LogedinUser = dr["UserCode"].ToString();
                    s.OS = dr["OS"].ToString();
                    s.Browser = dr["Browser"].ToString();
                    s.Location = dr["Loction"].ToString();
                    s.LastLogin = dr["LastLogin"] != DBNull.Value ? Convert.ToDateTime(dr["LastLogin"]) : (DateTime?)null;



                    //s.OrgCode = dr["OrgCode"].ToString();
                    // s.OrgName= dr["Organization_Name"].ToString();
                    //s.LogedinUser = dr["UserCode"].ToString();
                    //s.OS = dr["OS"].ToString();
                    //s.Browser = dr["Browser"].ToString();
                    //s.LastLoginDate = dr["LastLogin"] != DBNull.Value && !string.IsNullOrEmpty(dr["LastLogin"].ToString()) ? Convert.ToDateTime(dr["LastLogin"].ToString()) : new DateTime(1900, 1, 1);


                    if (dr["Device_Flag"].ToString() == "1")
                    {
                        s.Device_Flag = "Active";
                    }
                    else
                    {
                        s.Device_Flag = "De-Active";
                    }

                    list.Add(s);
                }
                return list;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion




        #region Function_To_Get_UserDeviceInformation
        public (string OS, string Browser) GetUserAgentInfo(HttpRequest request)
        {
            var userAgent = request.Headers["User-Agent"].ToString();
            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(userAgent);

            var os = clientInfo.OS.ToString();
            var browser = clientInfo.UA.ToString();

            return (os, browser);
        }
        #endregion

    }
}
