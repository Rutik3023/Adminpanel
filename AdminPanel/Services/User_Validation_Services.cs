
using System.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

using System.Net.Mail;
using System.Net;
using AdminPanel.CommonRepo;
using  AdminPanel.Models;


namespace AdminPanel.Services
{
    public interface IUserValidationService
    {
        //Forget Username and Password validation
        public ResponceMessage UserMailvalidationCheck(string Email);
        public ResponceMessage PassswordMailvalidationCheck(string Email, string UserName);
        public bool mailalert_ForgetUsername(string Username, string Email, string Fullname);
        public bool mailalert_ForgetPassword(string Email, string Username, string Fullname);

        public bool Check_USER_PASSWORDCHANGE_LINK_Status(string Email);
        public bool Update_USER_PASSWORDCHANGE_LINK(string Email);

        public bool Update_USER_PASSWORD(ForgetUserPassword Obj);


        //Org sas validation and machine validation
        public OrganizationLicense Get_Org_License_By_OrgCode(string orgcode);
        public ResponceMessage ValidateOrgLicencebyorgcode(string orgcode);
        public string Insert_Device_Details(DeviceDetails obj);

        public int Check_Machine_Details_Validation(DeviceDetails obj);

        public int Check_OrgNo_UserName_Validation(string orgcode, string username);
        public bool mailalert_For_Auth_Code(string Email, string Fullname, string Authcode);

        public ResponceMessage UpdateDeviceDetailsFlg(string Usercode, string UniqCode);

    }
    public class User_Validation_Services : IUserValidationService
    {
        IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public User_Validation_Services(IConfiguration _configuration, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            configuration = _configuration;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        //Forget User and password validation
        #region Function to check Username in case of forget username
        public ResponceMessage UserMailvalidationCheck(string Email)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");


                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);
                string sql = "USP_GEN_USERNAME_MAIL_VALIDATION";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                SqlParameter P_EMAIL = new SqlParameter();
                P_EMAIL.ParameterName = "P_EMAIL";
                P_EMAIL.SqlDbType = SqlDbType.VarChar;
                P_EMAIL.Size = 100;
                P_EMAIL.Value = Email;
                db.cmd.Parameters.Add(P_EMAIL);


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


                var dr = db.DataReader(sql);

                VMUserMaser data = new VMUserMaser();
                while (dr.Read())
                {


                    data.UserName = dr["UserName"].ToString();
                    data.FullName = dr["FullName"].ToString();

                }


                ResponceMessage OBJ = new ResponceMessage();
                int RespCode = Convert.ToInt32(P_CODE.Value);
                OBJ.code = RespCode;

                //OBJ.Sys_Code = P_MESSAGE.Value.ToString();
                OBJ.Message = data;



                db.CloseConnection();


                return OBJ;


            }
            catch (Exception ex)
            {
                return null;
            }




        }
        #endregion

        #region Function to check Username in case of forget username
        public ResponceMessage PassswordMailvalidationCheck(string Email, string UserName)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");


                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);
                string sql = "USP_GEN_FORGOTPASSWORDCHECK";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                SqlParameter P_UserName = new SqlParameter();
                P_UserName.ParameterName = "P_UserName";
                P_UserName.SqlDbType = SqlDbType.VarChar;
                P_UserName.Size = 100;
                P_UserName.Value = UserName;
                db.cmd.Parameters.Add(P_UserName);

                SqlParameter P_Co_Email_ID = new SqlParameter();
                P_Co_Email_ID.ParameterName = "P_Co_Email_ID";
                P_Co_Email_ID.SqlDbType = SqlDbType.VarChar;
                P_Co_Email_ID.Size = 100;
                P_Co_Email_ID.Value = Email;
                db.cmd.Parameters.Add(P_Co_Email_ID);


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


                var dr = db.DataReader(sql);

                ForgetUserPassword data = new ForgetUserPassword();
                while (dr.Read())
                {


                    data.Username = dr["UserName"].ToString();
                    data.Email = dr["Co_Email_ID"].ToString();
                    data.Password = dr["Password"].ToString();
                    data.FullName = dr["First_Name"].ToString();

                }


                ResponceMessage OBJ = new ResponceMessage();
                int RespCode = Convert.ToInt32(P_CODE.Value);
                OBJ.code = RespCode;

                //OBJ.Sys_Code = P_MESSAGE.Value.ToString();
                OBJ.Message = data;


                db.CloseConnection();



                return OBJ;


            }
            catch (Exception ex)
            {
                return null;
            }




        }
        #endregion



        #region Function to send username via mail in case of forget username
        public bool mailalert_ForgetUsername(string Username, string Email, string Fullname)
        {
            try
            {
                //string EmailBody = configuration["MailTemplate_Forget_Username"];

                string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Mailtemplate");
                string EmailBody = Path.Combine(rootPath, "Forget_UsernameMail.html");

                // Read HTML template from the text file
                string body = File.ReadAllText(EmailBody);
                string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
                string SenderEmail = configuration["SenderEmail"];
                string SenderName = configuration["SenderName"];
                string recipientEmail = Email;
                string recipientName = Fullname;
                string User_name = configuration["Username"];
                string Password = configuration["Password"];
                string Mailserver = configuration["Mailserver"];
                int portNumber = Convert.ToInt32(configuration["portNumber"]);

                // Replace placeholders in the HTML template with actual values
                body = body.Replace("{FullName}", Fullname)
                               .Replace("{UserName}", Username);


                // Mail subject and body
                string subject = "Your Username Retrieval Request";


                // Create and configure the mail message
                MailMessage message = new MailMessage(new MailAddress(SenderEmail, SenderName), new MailAddress(recipientEmail, recipientName));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                // Create and configure the SMTP client
                using (SmtpClient client = new SmtpClient(Mailserver, portNumber))
                {
                    client.Credentials = new NetworkCredential(User_name, Password);
                    client.EnableSsl = true;

                    // Send the email
                    client.Send(message);

                    return true;


                }

            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

        #region Function to send reset password link via mail in case of forget passsword
        public bool mailalert_ForgetPassword(string Email, string Username, string Fullname)
        {
            try
            {

                //string EmailBody = configuration["MailTemplate_Forget_Password"];
                string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Mailtemplate");
                string EmailBody = Path.Combine(rootPath, "Forget_PasswordMail.html");
                // Read HTML template from the text file
                string body = File.ReadAllText(EmailBody);
                string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
                string SenderEmail = configuration["SenderEmail"];
                string SenderName = configuration["SenderName"];
                string recipientEmail = Email;
                string recipientName = Fullname;
                string User_name = configuration["Username"];
                string Password = configuration["Password"];
                string Mailserver = configuration["Mailserver"];
                int portNumber = Convert.ToInt32(configuration["portNumber"]);

                byte[] Encryption = System.Text.Encoding.UTF8.GetBytes(Email);
                string EnceryptEmail = Convert.ToBase64String(Encryption);

                byte[] EncryptionFullname = System.Text.Encoding.UTF8.GetBytes(Fullname);
                string EnceryptFullname = Convert.ToBase64String(EncryptionFullname);

                byte[] EncryptionUsername = System.Text.Encoding.UTF8.GetBytes(Username);
                string EnceryptUsername = Convert.ToBase64String(EncryptionUsername);

                string url = configuration["hosturl"];
                // Replace placeholders in the HTML template with actual values
                body = body.Replace("{FullName}", Fullname)
                               .Replace("{Email}", EnceryptEmail)
                               .Replace("{url}", url)
                                .Replace("{Username}", EnceryptUsername)
                                .Replace("{EmpName}", EnceryptFullname);


                // Mail subject and body
                string subject = "Password Reset Request";


                // Create and configure the mail message
                MailMessage message = new MailMessage(new MailAddress(SenderEmail, SenderName), new MailAddress(recipientEmail, recipientName));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                // Create and configure the SMTP client
                using (SmtpClient client = new SmtpClient(Mailserver, portNumber))
                {
                    client.Credentials = new NetworkCredential(User_name, Password);
                    client.EnableSsl = true;

                    // Send the email
                    client.Send(message);
                    InsertUpdate_USER_PASSWORDCHANGE_LINK(Email);

                    return true;


                }

            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion



        #region Function to   InsertUpdate_USER_PASSWORDCHANGE_LINK
        public bool InsertUpdate_USER_PASSWORDCHANGE_LINK(string Email)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");


                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);
                string sql = "USP_GEN_UPDATE_ACTIVE_USERPASSWORD_CHANGE_LINK";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                SqlParameter P_EMAIL = new SqlParameter();
                P_EMAIL.ParameterName = "EMAILID";
                P_EMAIL.SqlDbType = SqlDbType.VarChar;
                P_EMAIL.Size = 100;
                P_EMAIL.Value = Email;
                db.cmd.Parameters.Add(P_EMAIL);


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


                var dr = db.DataReader(sql);




                ResponceMessage OBJ = new ResponceMessage();
                int RespCode = Convert.ToInt32(P_CODE.Value);
                db.CloseConnection();
                if (RespCode == 0 || RespCode == 1)
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


        #region Function to  Check_USER_PASSWORDCHANGE_LINK_Status
        public bool Check_USER_PASSWORDCHANGE_LINK_Status(string Email)
        {

            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");


                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);
                string sql = "USP_GEN_CHECK_USERPASSWORD_CHANGE_LINK";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                SqlParameter P_EMAIL = new SqlParameter();
                P_EMAIL.ParameterName = "EMAILID";
                P_EMAIL.SqlDbType = SqlDbType.VarChar;
                P_EMAIL.Size = 100;
                P_EMAIL.Value = Email;
                db.cmd.Parameters.Add(P_EMAIL);


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


                var dr = db.DataReader(sql);




                ResponceMessage OBJ = new ResponceMessage();
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


        #region  Function to Upadte USER_PASSWORDCHANGE_LINK after Reset
        public bool Update_USER_PASSWORDCHANGE_LINK(string Email)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");


                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);
                string sql = "[USP_GEN_UPDATE_DEACTIVE_USERPASSWORD_CHANGE_LINK]";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                SqlParameter P_EMAIL = new SqlParameter();
                P_EMAIL.ParameterName = "EMAILID";
                P_EMAIL.SqlDbType = SqlDbType.VarChar;
                P_EMAIL.Size = 100;
                P_EMAIL.Value = Email;
                db.cmd.Parameters.Add(P_EMAIL);


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

                ResponceMessage OBJ = new ResponceMessage();
                int RespCode = Convert.ToInt32(P_CODE.Value);
                db.CloseConnection();
                if (RespCode == 0 || RespCode == 1)
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

        #region Function To reset User Password 
        public bool Update_USER_PASSWORD(ForgetUserPassword Obj)
        {

            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");


                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);
                string sql = "USP_GEN_RESET_USERPASSWORD";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                SqlParameter P_EMAILID = new SqlParameter();
                P_EMAILID.ParameterName = "P_EMAILID";
                P_EMAILID.SqlDbType = SqlDbType.VarChar;
                P_EMAILID.Size = 100;
                P_EMAILID.Value = Obj.Email;
                db.cmd.Parameters.Add(P_EMAILID);

                SqlParameter P_EmpName = new SqlParameter();
                P_EmpName.ParameterName = "P_EmpName";
                P_EmpName.SqlDbType = SqlDbType.VarChar;
                P_EmpName.Size = 100;
                P_EmpName.Value = Obj.FullName;
                db.cmd.Parameters.Add(P_EmpName);

                SqlParameter p_USERNAME = new SqlParameter();
                p_USERNAME.ParameterName = "p_USERNAME";
                p_USERNAME.SqlDbType = SqlDbType.VarChar;
                p_USERNAME.Size = 100;
                p_USERNAME.Value = Obj.Username;
                db.cmd.Parameters.Add(p_USERNAME);


                SqlParameter P_PASSWORD = new SqlParameter();
                P_PASSWORD.ParameterName = "P_PASSWORD";
                P_PASSWORD.SqlDbType = SqlDbType.VarChar;
                P_PASSWORD.Size = 100;
                P_PASSWORD.Value = Obj.Password;
                db.cmd.Parameters.Add(P_PASSWORD);


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

                ResponceMessage OBJ = new ResponceMessage();
                string resmsg = P_MESSAGE.Value.ToString();
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



        //Org sas validation and machine validation

        #region Function_To_Get_Org_License_By_OrgCode
        public OrganizationLicense Get_Org_License_By_OrgCode(string orgcode)
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_GET_ORG_LICENSE_BY_ORGCODE";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter OrgCode = new SqlParameter();
                OrgCode.ParameterName = "OrgCode";
                OrgCode.SqlDbType = SqlDbType.VarChar;
                OrgCode.Value = orgcode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(OrgCode);


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

                OrganizationLicense ol = new OrganizationLicense();

                if (RespCode == 2)
                {
                    ol.Sys_Code = "SA";

                    return ol;
                }

                if (RespCode == 1)
                {
                    return null;
                }
                else
                {


                    while (dr.Read())
                    {


                        ol.Sys_Code = dr["Sys_Code"] != DBNull.Value ? dr["Sys_Code"].ToString() : null; // Assuming Sys_Code is an int
                        ol.AuthKey = dr["AuthKey"] != DBNull.Value ? dr["AuthKey"].ToString() : null;
                        ol.OrgCode = dr["OrgCode"] != DBNull.Value ? dr["OrgCode"].ToString() : null;
                        ol.Valid_From = dr["Valid_From"] != DBNull.Value ? (DateTime?)dr["Valid_From"] : null; // Convert to DateTime?
                        ol.Valid_Till = dr["Valid_Till"] != DBNull.Value ? (DateTime?)dr["Valid_Till"] : null; // Convert to DateTime?
                    }
                    db.CloseConnection();
                    return ol;

                }


            }
            catch (Exception)
            {

                throw;
            }
        }



        public ResponceMessage ValidateOrgLicencebyorgcode(string orgcode)
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "ORG_ValidateLicenace";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter OrgCode = new SqlParameter();
                OrgCode.ParameterName = "OrgCode";
                OrgCode.SqlDbType = SqlDbType.VarChar;
                OrgCode.Value = orgcode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(OrgCode);


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

                string message = P_Message.Value.ToString();
                ResponceMessage ol = new ResponceMessage();
                ol.code = RespCode;
                ol.Message = message;

                return ol;




            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

        #region Function_To_Insert_Device_Details
        public string Insert_Device_Details(DeviceDetails obj)
        {
            try
            {


                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_INSERT_DEVICE_DETAILS";
                DBHelper db = new DBHelper();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;



                SqlParameter P_UserCode = new SqlParameter();
                P_UserCode.ParameterName = "P_UserCode";
                P_UserCode.SqlDbType = SqlDbType.VarChar;
                P_UserCode.Size = 20;
                P_UserCode.Value = obj.UserCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_UserCode);


                //SqlParameter P_UserCode = new SqlParameter();
                //P_UserCode.ParameterName = "P_UserCode";
                //P_UserCode.SqlDbType = SqlDbType.VarChar;
                //P_UserCode.Size = 20;
                //P_UserCode.Value = obj.UserCode;
                //db.cmd.Parameters.Add(P_UserCode);

                // Add parameters
                SqlParameter P_OS = new SqlParameter();
                P_OS.ParameterName = "P_OS";
                P_OS.SqlDbType = SqlDbType.VarChar;
                P_OS.Value = obj.OS ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_OS);

                SqlParameter P_Browser = new SqlParameter();
                P_Browser.ParameterName = "P_Browser";
                P_Browser.SqlDbType = SqlDbType.VarChar;
                P_Browser.Value = obj.Browser ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Browser);


                SqlParameter P_Location = new SqlParameter();
                P_Location.ParameterName = "P_Location";
                P_Location.SqlDbType = SqlDbType.VarChar;
                P_Location.Value = obj.Location ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Location);

                SqlParameter P_Device_Flag = new SqlParameter();
                P_Device_Flag.ParameterName = "P_Device_Flag";
                P_Device_Flag.SqlDbType = SqlDbType.VarChar;
                P_Device_Flag.Value = "0" ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Device_Flag);


                SqlParameter P_DeviceUniqueCode = new SqlParameter();
                P_DeviceUniqueCode.ParameterName = "P_DeviceUniqueCode";
                P_DeviceUniqueCode.SqlDbType = SqlDbType.VarChar;
                P_DeviceUniqueCode.Value = obj.DuniqueIdentifier ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_DeviceUniqueCode);


                SqlParameter P_Sys_Created_By = new SqlParameter();
                P_Sys_Created_By.ParameterName = "P_Sys_Created_By";
                P_Sys_Created_By.SqlDbType = SqlDbType.VarChar;
                P_Sys_Created_By.Size = 20;
                P_Sys_Created_By.Value = obj.UserCode;
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

                var dr = db.FillData(sql);

                int RespCode = Convert.ToInt32(P_CODE.Value);

                string mes = P_Message.Value.ToString();

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

        #region Function_To_Check_OrgNo_UserName_Validation
        public int Check_OrgNo_UserName_Validation(string orgcode, string username)
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_ORG_USER_MAPPING";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter OrgCode = new SqlParameter();
                OrgCode.ParameterName = "P_ORGCODE";
                OrgCode.SqlDbType = SqlDbType.VarChar;
                OrgCode.Value = orgcode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(OrgCode);


                SqlParameter P_USERNAME = new SqlParameter();
                P_USERNAME.ParameterName = "P_USERNAME";
                P_USERNAME.SqlDbType = SqlDbType.VarChar;
                P_USERNAME.Value = username ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_USERNAME);

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
                db.CloseConnection();
                return RespCode;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Function_To_Check_Machine_Details_Validation
        public int Check_Machine_Details_Validation(DeviceDetails obj)
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_CHECK_DEVICE_DETAILS_VALIDATION";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_UserCode = new SqlParameter();
                P_UserCode.ParameterName = "P_UserCode";
                P_UserCode.SqlDbType = SqlDbType.VarChar;
                P_UserCode.Value = obj.UserCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_UserCode);

                SqlParameter P_OS = new SqlParameter();
                P_OS.ParameterName = "P_OS";
                P_OS.SqlDbType = SqlDbType.VarChar;
                P_OS.Value = obj.OS ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_OS);

                SqlParameter P_Browser = new SqlParameter();
                P_Browser.ParameterName = "P_Browser";
                P_Browser.SqlDbType = SqlDbType.VarChar;
                P_Browser.Value = obj.Browser ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_Browser);


                SqlParameter P_DeviceUniqueCode = new SqlParameter();
                P_DeviceUniqueCode.ParameterName = "P_DeviceUniqueCode";
                P_DeviceUniqueCode.SqlDbType = SqlDbType.VarChar;
                P_DeviceUniqueCode.Value = obj.DuniqueIdentifier ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_DeviceUniqueCode);

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

                var dr = db.DataReader(sql);

                int RespCode = Convert.ToInt32(P_CODE.Value);

                var RespMsg = P_MESSAGE.Value.ToString();

                db.CloseConnection();
                return RespCode;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion



        #region Function_To_Send_Auth_Token_By_Mail
        public bool mailalert_For_Auth_Code(string Email, string Fullname, string Authcode)
        {
            try
            {


                string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Mailtemplate");
                string EmailBody = Path.Combine(rootPath, "AuthCode_MailBody.html");



                //string EmailBody = configuration["MailTemplate_Auth_Token_Registration"];



                string body = File.ReadAllText(EmailBody);
                string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
                string SenderEmail = configuration["SenderEmail"];
                string SenderName = configuration["SenderName"];
                string recipientEmail = Email;
                string recipientName = Fullname;
                string User_name = configuration["Username"];
                string Password = configuration["Password"];
                string Mailserver = configuration["Mailserver"];
                int portNumber = Convert.ToInt32(configuration["portNumber"]);

                byte[] Encryption = System.Text.Encoding.UTF8.GetBytes(Email);
                string EnceryptEmail = Convert.ToBase64String(Encryption);

                // Replace placeholders in the HTML template with actual values
                body = body.Replace("{FullName}", Fullname)
                               .Replace("{Email}", EnceryptEmail).Replace("{AuthCode}", Authcode);


                // Mail subject and body
                string subject = "Your Authentication Code for Device Registration";



                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Mailserver);
                mail.From = new MailAddress(SenderEmail);
                mail.To.Add(recipientEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                mail.IsBodyHtml = true;
                SmtpServer.Port = Convert.ToInt32(portNumber);



                SmtpServer.Credentials = new System.Net.NetworkCredential(User_name, Password);
                SmtpServer.EnableSsl = true;
                SmtpServer.Host = configuration["Mailserver"];
                SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                SmtpServer.TargetName = configuration["SmtpTargetName"];

                SmtpServer.Send(mail);


                return true;



            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

        public ResponceMessage UpdateDeviceDetailsFlg(string Usercode, string UniqCode)
        {
            #region
            try
            {

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_UPDATEDEVICEDETAILSFLAG";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_DeviceUniqueCode = new SqlParameter();
                P_DeviceUniqueCode.ParameterName = "P_DeviceUniqueCode";
                P_DeviceUniqueCode.SqlDbType = SqlDbType.NVarChar;
                P_DeviceUniqueCode.Value = UniqCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_DeviceUniqueCode);

                SqlParameter P_UserCode = new SqlParameter();
                P_UserCode.ParameterName = "P_UserCode";
                P_UserCode.SqlDbType = SqlDbType.VarChar;
                P_UserCode.Value = Usercode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_UserCode);



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

                var dr = db.DataReader(sql);

                int RespCode = Convert.ToInt32(P_CODE.Value);

                db.CloseConnection();

                ResponceMessage res = new ResponceMessage();
                res.code = RespCode;
                return res;



            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
