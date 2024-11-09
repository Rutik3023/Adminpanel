

using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using AdminPanel.CommonRepo;
using  AdminPanel.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminPanel.Services
{

    public interface IOrganization
    {
        public ResponceMessage InsertUpadte_Oraganiz(Organization_model obj);
        public bool Delete_Organization(string OrgCode);
        public List<Organization_model> GetOrganizationList(int Id);
        public List<SelectDropItem> GetOrganizationDropDown();
        public Organization_model GetOrganizationById(string OrgId);
        ResponceMessage InsertUpadte_OrgLogo(Organization_model obj, string OrgCode);

        public List<SelectListItem> GetOrganizationListFor_License();
        List<Organization_model> GetOrgLogoList();
        Organization_model GetOrgLogoById(string Sys_Code, Organization_model O);
        public bool Delete_Doc_Logo(string code);
    }

    public class Organization_Services : IOrganization
    {
        IConfiguration configuration;
        IGeneral general;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Organization_Services(IConfiguration _configuration, IHttpContextAccessor httpContextAccessor, IGeneral _general)
        {
            configuration = _configuration;
            _httpContextAccessor = httpContextAccessor;
            general = _general;
        }

        #region Function To Insert or Update_Organization
        public ResponceMessage InsertUpadte_Oraganiz(Organization_model obj)
        {
            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");
                var user = _httpContextAccessor.HttpContext.User;
                string UserCode = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (UserCode == "" || UserCode == null)
                {
                    UserCode = null;
                    //return null;
                }




                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);
                string sql = "USP_GEN_INSERTUPDATE_ORGANIZATIONMASTER";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                SqlParameter P_SYSCODE = new SqlParameter();
                P_SYSCODE.ParameterName = "P_SYSCODE";
                P_SYSCODE.SqlDbType = SqlDbType.VarChar;
                P_SYSCODE.Size = 20;
                P_SYSCODE.Value = obj.Sys_Code ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_SYSCODE);

                SqlParameter P_ORGANIZATIONNAME = new SqlParameter();
                P_ORGANIZATIONNAME.ParameterName = "P_ORGANIZATIONNAME";
                P_ORGANIZATIONNAME.SqlDbType = SqlDbType.VarChar;
                P_ORGANIZATIONNAME.Size = 200;
                P_ORGANIZATIONNAME.Value = obj.Organization_Name;
                db.cmd.Parameters.Add(P_ORGANIZATIONNAME);

                SqlParameter P_ORGANIZATIONCODE = new SqlParameter();
                P_ORGANIZATIONCODE.ParameterName = "P_ORGANIZATIONCODE";
                P_ORGANIZATIONCODE.SqlDbType = SqlDbType.NVarChar;
                P_ORGANIZATIONCODE.Size = 200;
                P_ORGANIZATIONCODE.Value = obj.Organization_Code;
                db.cmd.Parameters.Add(P_ORGANIZATIONCODE);


                SqlParameter P_ADDRESS = new SqlParameter();
                P_ADDRESS.ParameterName = "P_ADDRESS";
                P_ADDRESS.SqlDbType = SqlDbType.VarChar;
                P_ADDRESS.Size = 500;
                P_ADDRESS.Value = obj.Address;
                db.cmd.Parameters.Add(P_ADDRESS);


                SqlParameter P_CITYID = new SqlParameter();
                P_CITYID.ParameterName = "P_CITYID";
                P_CITYID.SqlDbType = SqlDbType.VarChar;
                P_CITYID.Size = 30;
                P_CITYID.Value = obj.CityId ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_CITYID);


                SqlParameter P_STATEID = new SqlParameter();
                P_STATEID.ParameterName = "P_STATEID";
                P_STATEID.SqlDbType = SqlDbType.VarChar;
                P_STATEID.Size = 200;
                P_STATEID.Value = obj.StateId ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_STATEID);


                SqlParameter P_COUNTRYID = new SqlParameter();
                P_COUNTRYID.ParameterName = "P_COUNTRYID";
                P_COUNTRYID.SqlDbType = SqlDbType.VarChar;
                P_COUNTRYID.Size = 200;
                P_COUNTRYID.Value = obj.CountryId ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_COUNTRYID);

                SqlParameter P_CURRENCYID = new SqlParameter();
                P_CURRENCYID.ParameterName = "P_CURRENCYID";
                P_CURRENCYID.SqlDbType = SqlDbType.Int;
                P_CURRENCYID.Value = obj.CurrencyId ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_CURRENCYID);

                SqlParameter P_PINCODE = new SqlParameter();
                P_PINCODE.ParameterName = "P_PINCODE";
                P_PINCODE.SqlDbType = SqlDbType.NVarChar;
                P_PINCODE.Value = obj.PinCode;
                P_PINCODE.Size = 200;
                db.cmd.Parameters.Add(P_PINCODE);


                SqlParameter P_EMAIL = new SqlParameter();
                P_EMAIL.ParameterName = "P_EMAIL";
                P_EMAIL.SqlDbType = SqlDbType.VarChar;
                P_EMAIL.Size = 200;
                P_EMAIL.Value = obj.Email;
                db.cmd.Parameters.Add(P_EMAIL);

                SqlParameter P_TELEPHONENO = new SqlParameter();
                P_TELEPHONENO.ParameterName = "P_TELEPHONENO";
                P_TELEPHONENO.SqlDbType = SqlDbType.VarChar;
                P_TELEPHONENO.Size = 14;
                P_TELEPHONENO.Value = obj.Telephone_No;
                db.cmd.Parameters.Add(P_TELEPHONENO);

                SqlParameter P_CONTACTPERSONNAME = new SqlParameter();
                P_CONTACTPERSONNAME.ParameterName = "P_CONTACTPERSONNAME";
                P_CONTACTPERSONNAME.SqlDbType = SqlDbType.NVarChar;
                P_CONTACTPERSONNAME.Size = 200;
                P_CONTACTPERSONNAME.Value = obj.Contact_Person_Name;
                db.cmd.Parameters.Add(P_CONTACTPERSONNAME);


                SqlParameter P_CONTACTPERSONNO = new SqlParameter();
                P_CONTACTPERSONNO.ParameterName = "P_CONTACTPERSONNO";
                P_CONTACTPERSONNO.SqlDbType = SqlDbType.NVarChar;
                P_CONTACTPERSONNO.Size = 200;
                P_CONTACTPERSONNO.Value = obj.Contact_Person_No;
                db.cmd.Parameters.Add(P_CONTACTPERSONNO);


                SqlParameter P_GSTNO = new SqlParameter();
                P_GSTNO.ParameterName = "P_GSTNO";
                P_GSTNO.SqlDbType = SqlDbType.NVarChar;
                P_GSTNO.Size = 20;
                P_GSTNO.Value = obj.GSTNO;
                db.cmd.Parameters.Add(P_GSTNO);

                SqlParameter P_PAN = new SqlParameter();
                P_PAN.ParameterName = "P_PAN";
                P_PAN.SqlDbType = SqlDbType.NVarChar;
                P_PAN.Size = 200;
                P_PAN.Value = obj.PANNO;
                db.cmd.Parameters.Add(P_PAN);

                SqlParameter P_ISAIR = new SqlParameter();
                P_ISAIR.ParameterName = "P_ISAIR";
                P_ISAIR.SqlDbType = SqlDbType.Bit;
                P_ISAIR.Value = obj.is_Air;
                db.cmd.Parameters.Add(P_ISAIR);


                SqlParameter P_ISLAND = new SqlParameter();
                P_ISLAND.ParameterName = "P_ISLAND";
                P_ISLAND.SqlDbType = SqlDbType.Bit;
                P_ISLAND.Value = obj.is_Land;
                db.cmd.Parameters.Add(P_ISLAND);

                SqlParameter P_OCEAN = new SqlParameter();
                P_OCEAN.ParameterName = "P_OCEAN";
                P_OCEAN.SqlDbType = SqlDbType.Bit;
                P_OCEAN.Value = obj.is_Ocean;
                db.cmd.Parameters.Add(P_OCEAN);

                SqlParameter P_STATUS = new SqlParameter();
                P_STATUS.ParameterName = "P_STATUS";
                P_STATUS.SqlDbType = SqlDbType.VarChar;
                P_STATUS.Value = obj.Status;
                db.cmd.Parameters.Add(P_STATUS);


                SqlParameter P_CREATEDBY = new SqlParameter();
                P_CREATEDBY.ParameterName = "P_CREATEDBY";
                P_CREATEDBY.SqlDbType = SqlDbType.VarChar;
                P_CREATEDBY.Value = UserCode;
                db.cmd.Parameters.Add(P_CREATEDBY);

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


                db.FillData(sql);

                ResponceMessage OBJ = new ResponceMessage();
                OBJ.code = Convert.ToInt32(P_CODE.Value.ToString());
                OBJ.Message = P_MESSAGE.Value.ToString();
                db.CloseConnection();
                return OBJ;

            }
            catch (Exception ex)
            {

                throw;
            }




        }
        #endregion

        #region InsertUpdate_Organization
        public ResponceMessage InsertUpadte_OrgLogo(Organization_model obj, string OrgCode)
        {
            try
            {

                var user = _httpContextAccessor.HttpContext.User;

                var userIds = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;



                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_UPDATE_ORG_LOGO";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_ORGCODE = new SqlParameter();
                P_ORGCODE.ParameterName = "P_ORGCODE";
                P_ORGCODE.SqlDbType = SqlDbType.VarChar;
                P_ORGCODE.Size = 30;
                P_ORGCODE.Value = OrgCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_ORGCODE);


                SqlParameter P_Logo = new SqlParameter();
                P_Logo.ParameterName = "P_Logo";
                P_Logo.SqlDbType = SqlDbType.VarBinary;
                P_Logo.Size = -1;  // Allow variable-sized data
                P_Logo.Value = obj.Logo;
                db.cmd.Parameters.Add(P_Logo);

                SqlParameter P_USERID = new SqlParameter();
                P_USERID.ParameterName = "P_USERID";
                P_USERID.SqlDbType = SqlDbType.VarChar;
                P_USERID.Value = userIds;//userIds;
                P_USERID.Size = 250;
                db.cmd.Parameters.Add(P_USERID);

                SqlParameter P_Note = new SqlParameter();
                P_Note.ParameterName = "P_Note";
                P_Note.SqlDbType = SqlDbType.VarChar;
                P_Note.Value = obj.Note;
                P_Note.Size = 4000;
                db.cmd.Parameters.Add(P_Note);

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

                var o = db.FillData(sql);
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

        #region Function To Get all Oraganization data On Grid
        public List<Organization_model> GetOrganizationList(int Id)
        {

            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_GETALL_ORGNIZATIONMASTER";
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

                List<Organization_model> OrganizationData = CommonFunction.MapToList<Organization_model>(dr);

                int RespCode = Convert.ToInt32(P_CODE.Value);
                // string RespMsg = P_Message.Value.ToString();
                db.CloseConnection();

                if (RespCode == 0)
                {
                    return OrganizationData;
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


        #region Function To Get Organization Data By OrgCode
        public Organization_model GetOrganizationById(string OrgId)
        {
            try
            {
                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_GEN_GET_ORGANIZATIONMASTER_BYID";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_SYS_CODE = new SqlParameter();
                P_SYS_CODE.ParameterName = "P_SYS_CODE";
                P_SYS_CODE.SqlDbType = SqlDbType.VarChar;
                P_SYS_CODE.Value = OrgId;
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

                var dr = db.DataReader(sql);
                Organization_model obj = new Organization_model();

                while (dr.Read())
                {
                    obj.Sys_Code = dr["SYS_CODE"] != DBNull.Value ? dr["SYS_CODE"].ToString() : string.Empty;
                    obj.Organization_Name = dr["Organization_Name"] != DBNull.Value ? dr["Organization_Name"].ToString() : string.Empty;
                    obj.Email = dr["Email"] != DBNull.Value ? dr["Email"].ToString() : string.Empty;
                    obj.Telephone_No = dr["Telephone_No"] != DBNull.Value ? dr["Telephone_No"].ToString() : string.Empty;
                    obj.PinCode = dr["PinCode"] != DBNull.Value ? dr["PinCode"].ToString() : string.Empty;
                    obj.Password = dr["Password"] != DBNull.Value ? dr["Password"].ToString() : string.Empty;
                    obj.Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : string.Empty;
                    obj.Contact_Person_Name = dr["Contact_Person_Name"] != DBNull.Value ? dr["Contact_Person_Name"].ToString() : string.Empty;
                    obj.Contact_Person_No = dr["Contact_Person_No"] != DBNull.Value ? dr["Contact_Person_No"].ToString() : string.Empty;
                    obj.GSTNO = dr["GSTNO"] != DBNull.Value ? dr["GSTNO"].ToString() : string.Empty;
                    obj.PANNO = dr["PANNO"] != DBNull.Value ? dr["PANNO"].ToString() : string.Empty;
                    obj.Logo = dr["Logo"] != DBNull.Value ? (byte[])dr["Logo"] : null;
                    obj.CityId = dr["CityId"] != DBNull.Value ? dr["CityId"].ToString() : string.Empty;
                    obj.CountryId = dr["CountryId"] != DBNull.Value ? dr["CountryId"].ToString() : string.Empty;
                    obj.CurrencyId = dr["CurrencyId"] != DBNull.Value ? dr["CurrencyId"].ToString() : string.Empty;
                    obj.StateId = dr["StateId"] != DBNull.Value ? dr["StateId"].ToString() : string.Empty;
                    obj.is_Ocean = dr["is_Ocean"] != DBNull.Value ? Convert.ToBoolean(dr["is_Ocean"]) : false;
                    obj.is_Land = dr["is_Land"] != DBNull.Value ? Convert.ToBoolean(dr["is_Land"]) : false;
                    obj.is_Air = dr["is_Air"] != DBNull.Value ? Convert.ToBoolean(dr["is_Air"]) : false;

                    obj.Organization_Code = dr["Organization_Code"] != DBNull.Value ? dr["Organization_Code"].ToString() : string.Empty;
                    obj.Status = dr["Status"] != DBNull.Value ? dr["Status"].ToString() : string.Empty;


                }

                int RespCode = Convert.ToInt32(P_CODE.Value);
                db.CloseConnection();
                if (RespCode == 0)
                {
                    return obj;
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

        #region Function To get Organization Data For Dropdown 
        public List<SelectDropItem> GetOrganizationDropDown()
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var orgCode = user.FindFirst("OrgCode")?.Value;

                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }

                List<SelectDropItem> list = new List<SelectDropItem>();

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_GET_ORGNIZATIONLIST_BY_ORGCODE";
                DBHelper db = new DBHelper();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_ORGID = new SqlParameter();
                P_ORGID.ParameterName = "P_ORGID";
                P_ORGID.SqlDbType = SqlDbType.VarChar;
                P_ORGID.Value = orgCode ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_ORGID);

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
                    SelectDropItem o = new SelectDropItem();
                    o.value = dr["Sys_Code"].ToString();
                    o.text = dr["Organization_Name"].ToString();
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
            catch (Exception ex)
            {

                throw;
            }

        }
        #endregion

        #region Function To Delete Organization 
        public bool Delete_Organization(string OrgCode)
        {
            try
            {
                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_DELETE_ORGANIZATIONMASTER";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_ORGCODE = new SqlParameter();
                P_ORGCODE.ParameterName = "P_ORGCODE";
                P_ORGCODE.SqlDbType = SqlDbType.VarChar;
                P_ORGCODE.Value = OrgCode;
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





        public List<SelectListItem> GetOrganizationListFor_License()
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_GETALL_ORGNIZATIONLIST_FOR_LICENSE";
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

                List<SelectListItem> org = new List<SelectListItem>();


                while (dr.Read())
                {
                    SelectListItem s = new SelectListItem();

                    s.Text = dr["Organization_Name"].ToString();
                    s.Value = dr["Sys_Code"].ToString();

                    org.Add(s);
                }



                return org;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        #region Function To Get all Oraganization Logo data On Grid
        public List<Organization_model> GetOrgLogoList()
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;

                var userIds = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_GETALL_ORGNIZATIONLOGO";
                DBHelper db = new DBHelper();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_OrgCode = new SqlParameter();
                P_OrgCode.ParameterName = "P_OrgCode";
                P_OrgCode.SqlDbType = SqlDbType.VarChar;
                P_OrgCode.Size = 30;
                P_OrgCode.Value = (object)orgCode ?? DBNull.Value;
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

                //SqlParameter P_ID = new SqlParameter();
                //P_ID.ParameterName = "P_ID";
                //P_ID.SqlDbType = SqlDbType.Int;
                //P_ID.Value = Id;
                //db.cmd.Parameters.Add(P_ID);

                var dr = db.DataReader(sql);

                List<Organization_model> OrganizationData = new List<Organization_model>();

                while (dr.Read())
                {
                    Organization_model O = new Organization_model();
                    O.Sys_Code = dr["Sys_Code"].ToString();
                    O.Organization_Name = dr["Organization_Name"] != DBNull.Value ? dr["Organization_Name"].ToString() : "";
                    O.Organization_Code = dr["Organization_Code"] != DBNull.Value ? dr["Organization_Code"].ToString() : "";
                    O.Note = dr["Note"] != DBNull.Value ? dr["Note"].ToString() : "";
                    O.Logo = dr["Logo"] != DBNull.Value ? (byte[])dr["Logo"] : null;
                    if (O.Logo != null)
                    {
                        O.LogoImg = Convert.ToBase64String(O.Logo);
                        O.MimeType = GetMimeType(O.Logo);
                    }
                    else
                    {
                        O.LogoImg = null;
                        O.MimeType = null;
                    }
                    OrganizationData.Add(O);
                }

                int RespCode = Convert.ToInt32(P_CODE.Value);
                // string RespMsg = P_Message.Value.ToString();
                db.CloseConnection();
                return OrganizationData;


            }
            catch (Exception ex)
            {

                throw;
            }


        }

        private string GetMimeType(byte[] imageData)
        {

            if (imageData.Length > 0)
            {
                // Check the image signature or header to determine the MIME type
                if (imageData.Take(2).SequenceEqual(new byte[] { 0xFF, 0xD8 })) return "image/jpeg"; // JPEG
                if (imageData.Take(8).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A })) return "image/png"; // PNG
                if (imageData.Take(2).SequenceEqual(new byte[] { 0x47, 0x49 })) return "image/gif"; // GIF

                // Add more image types as needed
            }

            return "application/octet-stream"; // Default MIME type
        }

        #endregion

        #region Function To Get Organization Logo By OrgCode
        public Organization_model GetOrgLogoById(string Sys_Code, Organization_model O)
        {
            try
            {
                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_GEN_GET_ORGANIZATIONLOGO_BYID";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_SYS_CODE = new SqlParameter();
                P_SYS_CODE.ParameterName = "P_SYS_CODE";
                P_SYS_CODE.SqlDbType = SqlDbType.VarChar;
                P_SYS_CODE.Value = Sys_Code;
                db.cmd.Parameters.Add(P_SYS_CODE);

                SqlParameter P_Logo = new SqlParameter();
                P_Logo.ParameterName = "P_Logo";
                P_Logo.SqlDbType = SqlDbType.VarBinary;
                P_Logo.Size = -1;  // Allow variable-sized data
                P_Logo.Value = O.Logo;
                db.cmd.Parameters.Add(P_Logo);

                SqlParameter P_Note = new SqlParameter();
                P_Note.ParameterName = "P_Note";
                P_Note.SqlDbType = SqlDbType.VarChar;
                P_Note.Value = O.Note ?? (object)DBNull.Value;
                P_Note.Size = 4000;
                db.cmd.Parameters.Add(P_Note);

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
                Organization_model obj = new Organization_model();

                while (dr.Read())
                {
                    obj.Sys_Code = dr["SYS_CODE"] != DBNull.Value ? dr["SYS_CODE"].ToString() : string.Empty;
                    obj.Organization_Name = dr["Organization_Name"] != DBNull.Value ? dr["Organization_Name"].ToString() : string.Empty;
                    obj.Organization_Code = dr["Organization_Code"] != DBNull.Value ? dr["Organization_Code"].ToString() : string.Empty;
                    obj.Note = dr["Note"] != DBNull.Value ? dr["Note"].ToString() : string.Empty;

                    obj.Logo = dr["Logo"] != DBNull.Value ? (byte[])dr["Logo"] : null;
                    if (obj.Logo != null)
                    {
                        obj.LogoImg = Convert.ToBase64String(obj.Logo);
                        obj.MimeType = GetMimeType(obj.Logo);
                    }
                    else
                    {
                        obj.LogoImg = null;
                        obj.MimeType = null;
                    }

                }

                int RespCode = Convert.ToInt32(P_CODE.Value);
                db.CloseConnection();
                if (RespCode == 0)
                {
                    return obj;
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

        #region Function to Delete Organization logo
        public bool Delete_Doc_Logo(string code)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;
                //string UserCode = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //string OrgCode = user.FindFirst("OrgCode")?.Value;

                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_GEN_DELETE_ORG_LOGO";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.CommandText = sql;

                // Input parameters
                SqlParameter P_SYS_CODE = new SqlParameter();
                P_SYS_CODE.ParameterName = "P_SYS_CODE";
                P_SYS_CODE.SqlDbType = SqlDbType.VarChar;
                P_SYS_CODE.Size = 30;
                P_SYS_CODE.Value = code;
                db.cmd.Parameters.Add(P_SYS_CODE);


                // Output parameters
                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "@P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_MESSAGE = new SqlParameter();
                P_MESSAGE.ParameterName = "@P_MESSAGE";
                P_MESSAGE.SqlDbType = SqlDbType.VarChar;
                P_MESSAGE.Size = int.MaxValue;
                P_MESSAGE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_MESSAGE);

                var dr = db.FillData(sql);
                int RespCode = Convert.ToInt32(P_CODE.Value);
                string msg = P_MESSAGE.Value.ToString();
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
    }
    #endregion
}
