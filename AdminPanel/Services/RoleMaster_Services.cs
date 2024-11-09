


using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net.Sockets;
using System.Security.Claims;
using  AdminPanel.Models;
using AdminPanel.CommonRepo;


namespace AdminPanel.Services
{
    public interface IRoleMaster
    {
        string INSERTUPDATE(VMRoleMaser obj);
        List<VMRoleMaser> GetAllRoleMasterList();
        List<VMRoleMaser> GetAllRoleMasterByOrgCode();
        VMRoleMaser GetAllRoleMasterById(int Id);
        bool DeleteRoleMasterById(int Id);

    }

    public class RoleMaster_Service : IRoleMaster
    {
        IConfiguration configuration;
        IGeneral general;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleMaster_Service(IConfiguration _configuration, IHttpContextAccessor httpContextAccessor, IGeneral _general)
        {
            configuration = _configuration;
            _httpContextAccessor = httpContextAccessor;
            general = _general;

        }


        #region Function To Delete Role Data
        public bool DeleteRoleMasterById(int Id)
        {
            try
            {
                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_ADM_DELETE_ROLEMASTER";
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

        #region Function To Get Role Data By RoleId
        public VMRoleMaser GetAllRoleMasterById(int Id)
        {
            try
            {
                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_ADM_GET_ROLEMASTER_BYID";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_ID = new SqlParameter();
                P_ID.ParameterName = "P_RoleID";
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

                VMRoleMaser obj = new VMRoleMaser();

                while (dr.Read())
                {
                    obj.Code = dr["Code"].ToString();
                    obj.OrgCode = dr["OrgCode"].ToString();
                    obj.RoleName = dr["RoleName"].ToString();
                    obj.RoleDescription = dr["RoleDescription"].ToString();
                    obj.Status = dr["Status"].ToString();
                    obj.isChecker = Convert.ToBoolean(dr["isChecker"]);
                    obj.CreatedBy = dr["CreatedBy"].ToString();
                    obj.CreatedOn = dr["CreationDate"].ToString();
                    obj.LastModifiedBy = dr["LastModifiedBy"].ToString();
                    obj.LastModificationDate = dr["LastModificationDate"].ToString();


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
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Function To All Get Role Data For Grid
        public List<VMRoleMaser> GetAllRoleMasterList()
        {
            try
            {



                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_ROLEMASTERLIST";
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

                List<VMRoleMaser> OrganizationData = CommonFunction.MapToList<VMRoleMaser>(dr);

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
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Function To Insert Or Update Role Data 
        public string INSERTUPDATE(VMRoleMaser obj)
        {
            try
            {

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_INSERTUPDATE_ROLEMASTER";
                DBHelper db = new DBHelper();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_CodeId = new SqlParameter();
                P_CodeId.ParameterName = "P_CodeId";
                P_CodeId.SqlDbType = SqlDbType.VarChar;
                P_CodeId.Value = obj.Code ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_CodeId);

                SqlParameter P_OrgID = new SqlParameter();
                P_OrgID.ParameterName = "P_OrgID";
                P_OrgID.SqlDbType = SqlDbType.VarChar;
                P_OrgID.Value = obj.OrgCode ?? (object)DBNull.Value; ;
                db.cmd.Parameters.Add(P_OrgID);

                SqlParameter P_Name = new SqlParameter();
                P_Name.ParameterName = "P_NAME";
                P_Name.SqlDbType = SqlDbType.VarChar;
                P_Name.Size = 200;
                P_Name.Value = obj.RoleName;
                db.cmd.Parameters.Add(P_Name);

                SqlParameter P_LOCKED = new SqlParameter();
                P_LOCKED.ParameterName = "P_LOCKED";
                P_LOCKED.SqlDbType = SqlDbType.VarChar;
                P_LOCKED.Value = obj.Status;
                db.cmd.Parameters.Add(P_LOCKED);

                SqlParameter P_DESCRIPTION = new SqlParameter();
                P_DESCRIPTION.ParameterName = "P_DESCRIPTION";
                P_DESCRIPTION.SqlDbType = SqlDbType.VarChar;
                P_DESCRIPTION.Size = 200;
                P_DESCRIPTION.Value = obj.RoleDescription ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_DESCRIPTION);


                SqlParameter P_CREATEDBY = new SqlParameter();
                P_CREATEDBY.ParameterName = "P_CREATEDBY";
                P_CREATEDBY.SqlDbType = SqlDbType.VarChar;
                P_CREATEDBY.Value = 1;
                db.cmd.Parameters.Add(P_CREATEDBY);

                SqlParameter P_isChecker = new SqlParameter();
                P_isChecker.ParameterName = "P_isChecker";
                P_isChecker.SqlDbType = SqlDbType.Bit;
                P_isChecker.Value = obj.isChecker;
                db.cmd.Parameters.Add(P_isChecker);

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

                int ResponseCode = Convert.ToInt32(P_CODE.Value.ToString());
                string ResponseMsg = P_Message.Value.ToString();
                db.CloseConnection();
                return ResponseMsg;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Function To Get Role Data By OrgCode
        public List<VMRoleMaser> GetAllRoleMasterByOrgCode()
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

                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_GET_ROLEMASTER_BYORGID";
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
                var RoleList = new List<VMRoleMaser>();
                //List<VMRoleMaser> OrganizationData = CommonFunction.MapToList<VMRoleMaser>(dr);
                while (dr.Read())
                {
                    var VMRoleMaser = new VMRoleMaser
                    {
                        Code = dr.IsDBNull(dr.GetOrdinal("Code")) ? null : dr.GetString(dr.GetOrdinal("Code")).ToString(),
                        OrgCode = dr.IsDBNull(dr.GetOrdinal("OrgCode")) ? null : dr.GetString(dr.GetOrdinal("OrgCode")),
                        RoleName = dr.IsDBNull(dr.GetOrdinal("RoleName")) ? null : dr.GetString(dr.GetOrdinal("RoleName")),
                        RoleDescription = dr.IsDBNull(dr.GetOrdinal("RoleDescription")) ? null : dr.GetString(dr.GetOrdinal("RoleDescription")),
                        Status = dr.IsDBNull(dr.GetOrdinal("Status")) ? null : dr.GetString(dr.GetOrdinal("Status")),
                        creationdate = dr.IsDBNull(dr.GetOrdinal("creationdate")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("creationdate")),

                        CreatedBy = dr.IsDBNull(dr.GetOrdinal("createdby")) ? null : dr.GetString(dr.GetOrdinal("createdby")),
                        LastModifiedBy = dr["LastModifiedBy"].ToString(),
                        LastModificationDate = dr["LastModificationDate"].ToString()

                        //isLocked = !dr.IsDBNull(dr.GetOrdinal("isLocked")) && dr.GetBoolean(dr.GetOrdinal("isLocked")),
                        //isChecker = !dr.IsDBNull(dr.GetOrdinal("isChecker")) && dr.GetBoolean(dr.GetOrdinal("isChecker"))
                    };

                    RoleList.Add(VMRoleMaser);
                }

                int RespCode = Convert.ToInt32(P_CODE.Value);
                //string RespMsg = P_Message.Value.ToString();
                db.CloseConnection();

                if (RespCode == 0)
                {
                    return RoleList;
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


    }

}
