
using AdminPanel.CommonRepo;
using  AdminPanel.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Syncfusion.EJ2.Spreadsheet;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Claims;

namespace AdminPanel.Services
{

    public interface IGeneral
    {

        public LoginCrediential LoginCheck(Login obj);

        public void Insert_Log(ExceptionLog exc);

        public void Write_Log_Into_Txt(string message, string ControllerName);


        public void InsertExcepstionlog(string message, string ControllerName);


        public List<SelectListItem> Get_All_Region_Master();

        public List<Org_Saas_License> Get_All_Org_Saas();

        public Org_Saas_License Get_Org_Saas_by_syscode(string syscode);


        public string InsertUpdate_Organization_License(Org_Saas_License obj);

        public List<Org_Saas_License> Delete_Organization_License(string syscode);


        public Login ActiveLoginCheck(LoginCrediential o);



        public ResponceMessage InsertUserLoginSession(DeviceDetails DDT);
        public ResponceMessage UpdateUserLoginSession(string UserId, int remarkcode);

    }




    public class General_Services : IGeneral
    {
        IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public General_Services(IConfiguration _configuration, IHttpContextAccessor httpContextAccessor)
        {
            configuration = _configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public void InsertExcepstionlog(string message, string ControllerName)
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
                string Con_String = configuration.GetConnectionString("MyConnection");

                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);
                string sql = "USP_INSERT_APPLICATION_ERRORLOG";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                SqlParameter P_ErrorMessage = new SqlParameter();
                P_ErrorMessage.ParameterName = "P_ErrorMessage";
                P_ErrorMessage.SqlDbType = SqlDbType.VarChar;
                P_ErrorMessage.Size = 8000;
                P_ErrorMessage.Value = message ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_ErrorMessage);

                SqlParameter P_Controller_Name = new SqlParameter();
                P_Controller_Name.ParameterName = "P_Controller_Name";
                P_Controller_Name.SqlDbType = SqlDbType.VarChar;
                P_Controller_Name.Size = 200;
                P_Controller_Name.Value = ControllerName ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Controller_Name);

                SqlParameter P_UserName = new SqlParameter();
                P_UserName.ParameterName = "P_UserName";
                P_UserName.SqlDbType = SqlDbType.VarChar;
                P_UserName.Size = 30;
                P_UserName.Value = userId ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_UserName);

                SqlParameter P_Org_Code = new SqlParameter();
                P_Org_Code.ParameterName = "P_Org_Code";
                P_Org_Code.SqlDbType = SqlDbType.VarChar;
                P_Org_Code.Size = 30;
                P_Org_Code.Value = orgCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Org_Code);
                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_Code";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_Message = new SqlParameter();

                P_Message.ParameterName = "P_Message";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);
                db.FillData(sql);
            }
            catch (Exception ex)
            {

                throw;
            }


        }



        #region Write Log into Txt File Function
        public void Write_Log_Into_Txt(string message, string ControllerName)
        {
            try
            {
                string LogDirectoryPath = configuration["LogTXTPath"];


                // Construct the file path with the current date
                string filePath = Path.Combine(LogDirectoryPath, $"Log_{DateTime.Now:yyyyMMdd}.txt");

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}|{ControllerName}| Error==> {message}");

                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        #endregion



        #region Function_To_LoginCheck
        public LoginCrediential LoginCheck(Login o)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);

                db.cmd.Parameters.Clear();
                string Sql = "USP_GEN_VALIDATE_LOGINUSER";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_USERNAME = new SqlParameter();
                P_USERNAME.ParameterName = "P_USERNAME";
                P_USERNAME.SqlDbType = SqlDbType.VarChar;
                P_USERNAME.Size = 200;
                P_USERNAME.Value = o.User_Name;
                db.cmd.Parameters.Add(P_USERNAME);

                SqlParameter P_PASSWORD = new SqlParameter();
                P_PASSWORD.ParameterName = "P_PASSWORD";
                P_PASSWORD.SqlDbType = SqlDbType.VarChar;
                P_PASSWORD.Size = 200;
                P_PASSWORD.Value = o.Password;
                db.cmd.Parameters.Add(P_PASSWORD);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_MESSAGE = new SqlParameter();
                P_MESSAGE.ParameterName = "P_MESSAGE";
                P_MESSAGE.SqlDbType = SqlDbType.VarChar;
                P_MESSAGE.Size = 200;
                P_MESSAGE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_MESSAGE);

                var dr = db.DataReader(Sql);


                LoginCrediential OBJ = new LoginCrediential();
                OBJ.Code = Convert.ToInt32(P_CODE.Value);

                if (P_MESSAGE.Value != null)
                {
                    OBJ.Message = (P_MESSAGE.Value.ToString());
                }
                else

                if (OBJ.Code == -1)
                {
                    return null;
                }
                else
                {

                    LoginCrediential obj = new LoginCrediential();
                    while (dr.Read())
                    {
                        obj.Id = dr["Id"].ToString();
                        obj.Name = dr["UserName"].ToString();
                        obj.Roles = dr["Roles"].ToString();
                        obj.OrgCode = dr["OrgCode"].ToString();
                        obj.OrgName = dr["OrgName"].ToString();
                        obj.Email = dr["Email"].ToString();

                        obj.isChecker = dr["isChecker"] != DBNull.Value ? Convert.ToBoolean(dr["isChecker"]) : (bool?)null;
                        //obj.wrongloginattempts = dr["wrongloginattempts"].ToString();
                    }
                    db.CloseConnection();
                    return obj;
                }

                return OBJ;


            }
            catch (Exception ex)
            {

                return null;
                throw;
            }


        }
        #endregion

        #region Function_To_Get_All_Region_Master
        public List<SelectListItem> Get_All_Region_Master()
        {

            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_GETALL_REGION_MASTER";
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
                    SelectListItem si = new SelectListItem();

                    si.Text = dr["Name"].ToString();
                    si.Value = dr["Sys_Code"].ToString();

                    list.Add(si);
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

        #region Function_To_Write_Log_Into_Txt

        #endregion

        #region Function_To_Insert_Log
        public void Insert_Log(ExceptionLog exc)
        {
            //try
            //{
            //    string Con_String = configuration.GetConnectionString("MyConnection");

            //    DBHelper db = new DBHelper();
            //    db.OpenConnection(Con_String);
            //    string sql = "USP_GEN_INSERTUPDATE_COUNTRYMASTER";
            //    db.cmd.CommandType = CommandType.StoredProcedure;
            //    db.cmd.Parameters.Clear();

            //    // Input parameters
            //    SqlParameter P_NAME = new SqlParameter();
            //    P_NAME.ParameterName = "P_Name";
            //    P_NAME.SqlDbType = SqlDbType.NVarChar;
            //    P_NAME.Size = 50;
            //    P_NAME.Value = obj.Name;
            //    db.cmd.Parameters.Add(P_NAME);

            //    SqlParameter P_CCode = new SqlParameter();
            //    P_CCode.ParameterName = "P_CCode";
            //    P_CCode.SqlDbType = SqlDbType.NVarChar;
            //    P_CCode.Size = 50;
            //    P_CCode.Value = obj.Code;
            //    db.cmd.Parameters.Add(P_CCode);

            //    SqlParameter P_Nationality = new SqlParameter();
            //    P_Nationality.ParameterName = "P_Nationality";
            //    P_Nationality.SqlDbType = SqlDbType.NVarChar;
            //    P_Nationality.Size = 50;
            //    P_Nationality.Value = obj.Nationality;
            //    db.cmd.Parameters.Add(P_Nationality);

            //    SqlParameter P_SectorCode = new SqlParameter();
            //    P_SectorCode.ParameterName = "P_SectorCode";
            //    P_SectorCode.SqlDbType = SqlDbType.VarChar;
            //    P_SectorCode.Size = 10;
            //    P_SectorCode.Value = obj.SectorCode;
            //    db.cmd.Parameters.Add(P_SectorCode);

            //    SqlParameter P_SectorName = new SqlParameter();
            //    P_SectorName.ParameterName = "P_SectorName";
            //    P_SectorName.SqlDbType = SqlDbType.VarChar;
            //    P_SectorName.Size = 100;
            //    P_SectorName.Value = obj.SectorName;
            //    db.cmd.Parameters.Add(P_SectorName);

            //    SqlParameter P_CurrencyCode = new SqlParameter();
            //    P_CurrencyCode.ParameterName = "P_CurrencyCode";
            //    P_CurrencyCode.SqlDbType = SqlDbType.VarChar;
            //    P_CurrencyCode.Size = 10;
            //    P_CurrencyCode.Value = obj.CurrencyCode;
            //    db.cmd.Parameters.Add(P_CurrencyCode);

            //    SqlParameter P_CurrencyName = new SqlParameter();
            //    P_CurrencyName.ParameterName = "P_CurrencyName";
            //    P_CurrencyName.SqlDbType = SqlDbType.VarChar;
            //    P_CurrencyName.Size = 100;
            //    P_CurrencyName.Value = obj.Currencyname;
            //    db.cmd.Parameters.Add(P_CurrencyName);

            //    SqlParameter P_CountryCode = new SqlParameter();
            //    P_CountryCode.ParameterName = "P_CountryCode";
            //    P_CountryCode.SqlDbType = SqlDbType.VarChar;
            //    P_CountryCode.Size = 50;
            //    P_CountryCode.Value = obj.CountryCode;
            //    db.cmd.Parameters.Add(P_CountryCode);

            //    SqlParameter P_Weight_CBM = new SqlParameter();
            //    P_Weight_CBM.ParameterName = "P_Weight_CBM";
            //    P_Weight_CBM.SqlDbType = SqlDbType.VarChar;
            //    P_Weight_CBM.Size = 10;
            //    P_Weight_CBM.Value = obj.Weight_CBM;
            //    db.cmd.Parameters.Add(P_Weight_CBM);

            //    SqlParameter P_Status = new SqlParameter();
            //    P_Status.ParameterName = "P_Status";
            //    P_Status.SqlDbType = SqlDbType.Char;
            //    P_Status.Size = 10;
            //    P_Status.Value = 'A';
            //    db.cmd.Parameters.Add(P_Status);

            //    SqlParameter P_Note = new SqlParameter();
            //    P_Note.ParameterName = "P_Note";
            //    P_Note.SqlDbType = SqlDbType.VarChar;
            //    P_Note.Size = 4000;
            //    P_Note.Value = obj.Note;
            //    db.cmd.Parameters.Add(P_Note);

            //    SqlParameter P_CreatedBY = new SqlParameter();
            //    P_CreatedBY.ParameterName = "P_CreatedBY";
            //    P_CreatedBY.SqlDbType = SqlDbType.Int;
            //    P_CreatedBY.Value = 1; // Assuming a default value for sys_Created_By
            //    db.cmd.Parameters.Add(P_CreatedBY);

            //    // Output parameters
            //    SqlParameter P_CODE = new SqlParameter();
            //    P_CODE.ParameterName = "P_CODE";
            //    P_CODE.SqlDbType = SqlDbType.Int;
            //    P_CODE.Direction = ParameterDirection.Output;
            //    db.cmd.Parameters.Add(@P_CODE);

            //    SqlParameter P_MESSAGE = new SqlParameter();
            //    P_MESSAGE.ParameterName = "P_MESSAGE";
            //    P_MESSAGE.SqlDbType = SqlDbType.VarChar;
            //    P_MESSAGE.Size = 4000; // Max size for VARCHAR(MAX)
            //    P_MESSAGE.Direction = ParameterDirection.Output;
            //    db.cmd.Parameters.Add(P_MESSAGE);

            //    db.FillData(sql);

            //    int ResponseCode = Convert.ToInt32(P_CODE.Value);
            //    string ResponseMsg = P_MESSAGE.Value.ToString();
            //    return ResponseMsg;
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;
            //}
        }
        #endregion

        #region Function_To_Get_All_Org_Saas
        public List<Org_Saas_License> Get_All_Org_Saas()
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_GETALL_ORGNIZATION_LICENSE_LIST";
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

                List<Org_Saas_License> list = new List<Org_Saas_License>();


                while (dr.Read())
                {
                    Org_Saas_License si = new Org_Saas_License();


                    si.Sys_Code = dr["Sys_Code"].ToString();
                    si.OrgCode = dr["OrgCode"].ToString();
                    DateTime validFrom;
                    if (DateTime.TryParse(dr["Valid_From"].ToString(), out validFrom))
                    {
                        si.Valid_From = validFrom.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        si.Valid_From = string.Empty; // or handle the invalid date scenario
                    }

                    DateTime validTill;
                    if (DateTime.TryParse(dr["Valid_Till"].ToString(), out validTill))
                    {
                        si.Valid_Till = validTill.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        si.Valid_Till = string.Empty; // or handle the invalid date scenario
                    }

                    si.Sys_Created_By = dr["Sys_Created_By"].ToString();

                    DateTime sysCreatedOn;
                    if (DateTime.TryParse(dr["Sys_Created_On"].ToString(), out sysCreatedOn))
                    {
                        si.Sys_Created_On = sysCreatedOn.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        si.Sys_Created_On = string.Empty; // or handle the invalid date scenario
                    }

                    list.Add(si);
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

        #region Function_To_Get_Org_Saas_by_syscode
        public Org_Saas_License Get_Org_Saas_by_syscode(string syscode)
        {
            try
            {


                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_GETALL_ORGNIZATION_LICENSE_LIST_BY_ID";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter SYS_CODE = new SqlParameter();
                SYS_CODE.ParameterName = "P_Sys_Code";
                SYS_CODE.SqlDbType = SqlDbType.VarChar;
                SYS_CODE.Value = syscode ?? (object)DBNull.Value;
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

                Org_Saas_License si = new Org_Saas_License();


                while (dr.Read())
                {

                    si.Sys_Code = dr["Sys_Code"].ToString();
                    si.OrgCode = dr["OrgCode"].ToString();
                    DateTime validFrom;
                    if (DateTime.TryParse(dr["Valid_From"].ToString(), out validFrom))
                    {
                        si.Valid_From = validFrom.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        si.Valid_From = string.Empty; // or handle the invalid date scenario
                    }

                    DateTime validTill;
                    if (DateTime.TryParse(dr["Valid_Till"].ToString(), out validTill))
                    {
                        si.Valid_Till = validTill.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        si.Valid_Till = string.Empty; // or handle the invalid date scenario
                    }

                    si.Sys_Created_By = dr["Sys_Created_By"].ToString();

                    DateTime sysCreatedOn;
                    if (DateTime.TryParse(dr["Sys_Created_On"].ToString(), out sysCreatedOn))
                    {
                        si.Sys_Created_On = sysCreatedOn.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        si.Sys_Created_On = string.Empty; // or handle the invalid date scenario
                    }


                }

                db.CloseConnection();
                return si;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Function_To_InsertUpdate_Organization_License
        public string InsertUpdate_Organization_License(Org_Saas_License obj)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;

                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                //if (obj.Valid_From != null)
                //{
                //   obj.Valid_From_Dt=Convert.ToDateTime(obj.Valid_From);    
                //}
                //if (obj.Valid_Till != null)
                //{
                //    obj.Valid_Till_Dt = Convert.ToDateTime(obj.Valid_Till);
                //}



                if (obj.Sys_Code == null)
                {
                    obj.Sys_Code = "0";
                }

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_HRM_INSERTUPDATE_ORGANIZATION_LICENSE";
                DBHelper db = new DBHelper();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;



                SqlParameter Org_License_Sys_Code = new SqlParameter();
                Org_License_Sys_Code.ParameterName = "Org_License_Sys_Code";
                Org_License_Sys_Code.SqlDbType = SqlDbType.VarChar;
                Org_License_Sys_Code.Value = obj.Sys_Code ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(Org_License_Sys_Code);

                // Add parameters
                SqlParameter OrgCode = new SqlParameter();
                OrgCode.ParameterName = "OrgCode";
                OrgCode.SqlDbType = SqlDbType.VarChar;
                OrgCode.Value = obj.OrgCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(OrgCode);

                SqlParameter AuthKey = new SqlParameter();
                AuthKey.ParameterName = "AuthKey";
                AuthKey.SqlDbType = SqlDbType.VarChar;
                AuthKey.Value = obj.AuthKey ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(AuthKey);


                SqlParameter Valid_From = new SqlParameter();
                Valid_From.ParameterName = "Valid_From";
                Valid_From.SqlDbType = SqlDbType.VarChar;
                Valid_From.Size = 50;
                Valid_From.Value = obj.Valid_From ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(Valid_From);

                SqlParameter Valid_Till = new SqlParameter();
                Valid_Till.ParameterName = "Valid_Till";
                Valid_Till.SqlDbType = SqlDbType.VarChar;
                Valid_Till.Size = 50;
                Valid_Till.Value = obj.Valid_Till ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(Valid_Till);

                SqlParameter P_Sys_Created_By = new SqlParameter();
                P_Sys_Created_By.ParameterName = "Sys_Created_By";
                P_Sys_Created_By.SqlDbType = SqlDbType.VarChar;
                P_Sys_Created_By.Value = userId ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Sys_Created_By);

                SqlParameter P_Message = new SqlParameter();
                P_Message.ParameterName = "P_MESSAGE";
                P_Message.SqlDbType = SqlDbType.VarChar;
                P_Message.Size = 200;
                P_Message.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_Message);

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);


                var dr = db.FillData(sql);

                int RespCode = Convert.ToInt32(P_CODE.Value);

                var RespMsg = P_Message.Value;
                db.CloseConnection();
                if (RespCode == 0)
                {
                    return "Inserted";
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Function_To_Delete_Organization_License
        public List<Org_Saas_License> Delete_Organization_License(string syscode)
        {
            try
            {


                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_DELETE_ORGANIZATION_LICENSE_BY_ID";
                DBHelper db = new DBHelper();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;



                SqlParameter P_Org_Saas_Sys_Code = new SqlParameter();
                P_Org_Saas_Sys_Code.ParameterName = "P_Org_Saas_Sys_Code";
                P_Org_Saas_Sys_Code.SqlDbType = SqlDbType.VarChar;
                P_Org_Saas_Sys_Code.Value = syscode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Org_Saas_Sys_Code);


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

                var ResponseMsg = P_Message.Value;

                List<Org_Saas_License> list = new List<Org_Saas_License>();

                while (dr.Read())
                {


                    Org_Saas_License si = new Org_Saas_License();

                    si.Sys_Code = dr["Sys_Code"].ToString();
                    si.OrgCode = dr["OrgCode"].ToString();

                    DateTime validFrom;
                    if (DateTime.TryParse(dr["Valid_From"].ToString(), out validFrom))
                    {
                        si.Valid_From = validFrom.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        si.Valid_From = string.Empty; // or handle the invalid date scenario
                    }

                    DateTime validTill;
                    if (DateTime.TryParse(dr["Valid_Till"].ToString(), out validTill))
                    {
                        si.Valid_Till = validTill.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        si.Valid_Till = string.Empty; // or handle the invalid date scenario
                    }

                    si.Sys_Created_By = dr["Sys_Created_By"].ToString();

                    DateTime sysCreatedOn;
                    if (DateTime.TryParse(dr["Sys_Created_On"].ToString(), out sysCreatedOn))
                    {
                        si.Sys_Created_On = sysCreatedOn.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        si.Sys_Created_On = string.Empty; // or handle the invalid date scenario
                    }


                    list.Add(si);
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



        #region Check active login session of user
        public Login ActiveLoginCheck(LoginCrediential o)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);

                db.cmd.Parameters.Clear();
                string Sql = "USP_ADM_CHECK_LOGINSESSION_STATUS";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_USERID = new SqlParameter();
                P_USERID.ParameterName = "P_USERID";
                P_USERID.SqlDbType = SqlDbType.VarChar;
                P_USERID.Size = 10;
                P_USERID.Value = o.Id;
                db.cmd.Parameters.Add(P_USERID);



                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_MESSAGE = new SqlParameter();
                P_MESSAGE.ParameterName = "P_MESSAGE";
                P_MESSAGE.SqlDbType = SqlDbType.VarChar;
                P_MESSAGE.Size = 200;
                P_MESSAGE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_MESSAGE);

                var dr = db.DataReader(Sql);

                Login obj = new Login();
                UserLoginSession OBJ = new UserLoginSession();
                OBJ.Code = Convert.ToInt32(P_CODE.Value);

                if (P_MESSAGE.Value != null)
                {
                    OBJ.Message = (P_MESSAGE.Value.ToString());
                }
                else

                if (OBJ.Code == -1)
                {
                    return null;
                }
                else
                {


                    while (dr.Read())
                    {
                        obj.User_Name = dr["UserName"]?.ToString() ?? string.Empty;
                        obj.UserId = dr["UserId"]?.ToString() ?? string.Empty;
                        obj.Device_OS = dr["Device_OS"]?.ToString() ?? string.Empty;
                        obj.Browser_Detail = dr["Browser_Detail"]?.ToString() ?? string.Empty;
                        obj.Device_Location = dr["Device_Location"]?.ToString() ?? string.Empty;
                        obj.LoginTime = dr["LoginTime"]?.ToString() ?? string.Empty;
                        obj.DuniqueIdentifier = dr["SessionId"]?.ToString() ?? string.Empty;


                        //obj.Remarks = dr["Remarks"].ToString();


                    }
                    db.CloseConnection();
                    return obj;
                }

                return obj;


            }
            catch (Exception ex)
            {

                return null;
                throw;
            }


        }
        #endregion


        #region Insert user login status 
        public ResponceMessage InsertUserLoginSession(DeviceDetails DDT)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);
                string sql = "USP_ADM_INSERT_LOGINSESSION";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                SqlParameter P_USERID = new SqlParameter();
                P_USERID.ParameterName = "P_USERID";
                P_USERID.SqlDbType = SqlDbType.VarChar;
                P_USERID.Size = 15;
                P_USERID.Value = DDT.UserCode;
                db.cmd.Parameters.Add(P_USERID);


                SqlParameter P_DEVICE_OS = new SqlParameter();
                P_DEVICE_OS.ParameterName = "P_DEVICE_OS";
                P_DEVICE_OS.SqlDbType = SqlDbType.VarChar;
                P_DEVICE_OS.Size = 250;
                P_DEVICE_OS.Value = DDT.OS ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_DEVICE_OS);

                SqlParameter P_BROWSER_DETAIL = new SqlParameter();
                P_BROWSER_DETAIL.ParameterName = "P_BROWSER_DETAIL";
                P_BROWSER_DETAIL.SqlDbType = SqlDbType.VarChar;
                P_BROWSER_DETAIL.Value = DDT.Browser ?? (object)DBNull.Value;
                P_BROWSER_DETAIL.Size = 250;
                db.cmd.Parameters.Add(P_BROWSER_DETAIL);


                SqlParameter P_DEVICE_LOCATION = new SqlParameter();
                P_DEVICE_LOCATION.ParameterName = "P_DEVICE_LOCATION";
                P_DEVICE_LOCATION.SqlDbType = SqlDbType.VarChar;
                P_DEVICE_LOCATION.Value = DDT.Location ?? (object)DBNull.Value;
                P_DEVICE_LOCATION.Size = 250;
                db.cmd.Parameters.Add(P_DEVICE_LOCATION);

                SqlParameter P_SESSIONID = new SqlParameter();
                P_SESSIONID.ParameterName = "P_SESSIONID";
                P_SESSIONID.SqlDbType = SqlDbType.VarChar;
                P_SESSIONID.Value = DDT.DuniqueIdentifier ?? (object)DBNull.Value;
                P_SESSIONID.Size = 250;
                db.cmd.Parameters.Add(P_SESSIONID);


                SqlParameter P_REMARK = new SqlParameter();
                P_REMARK.ParameterName = "P_REMARK";
                P_REMARK.SqlDbType = SqlDbType.VarChar;
                P_REMARK.Value = "";
                P_REMARK.Size = 250;
                db.cmd.Parameters.Add(P_REMARK);

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



        #region Update user login status 
        public ResponceMessage UpdateUserLoginSession(string UserId, int remarkcode)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);
                string sql = "USP_ADM_UPDATE_LOGINSESSION";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                SqlParameter P_USERID = new SqlParameter();
                P_USERID.ParameterName = "P_USERID";
                P_USERID.SqlDbType = SqlDbType.VarChar;
                P_USERID.Size = 15;
                P_USERID.Value = UserId;
                db.cmd.Parameters.Add(P_USERID);

                SqlParameter P_RESPCODE = new SqlParameter();
                P_RESPCODE.ParameterName = "P_RESPCODE";
                P_RESPCODE.SqlDbType = SqlDbType.Int;
                P_RESPCODE.Value = remarkcode;
                db.cmd.Parameters.Add(P_RESPCODE);


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
    }
}
