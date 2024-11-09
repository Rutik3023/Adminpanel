
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

using System.Security.Claims;
using  AdminPanel.Models;

namespace AdminPanel.Services
{

    #region Interface_IRoleMenuMapping
    public interface IRoleMenuMapping
    {
        public List<SelectListItem> Get_RoleMaster_By_OrgCode(string orgid);


        public List<VMGet_Menu_ByRoleID> Get_Menu_ByRoleID(string roleid);


        public string Insert_RMM(VMRoleMenuMapping obj);
    }
    #endregion


    public class RoleMenuMapping_Services : IRoleMenuMapping
    {
        IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleMenuMapping_Services(IConfiguration configuration, IHttpContextAccessor htp)
        {
            this.configuration = configuration;
            _httpContextAccessor = htp;
        }



        #region Function_To_Get_Menu_ByRoleID
        public List<VMGet_Menu_ByRoleID> Get_Menu_ByRoleID(string roleid)
        {



            try
            {


                var user = _httpContextAccessor.HttpContext.User;

                //// Access UserName claim
                //var userName = user.FindFirst(ClaimTypes.Name)?.Value;

                // Access Id claim
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Access OrgCode claim
                var orgCode = user.FindFirst("OrgCode")?.Value;



                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }



                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_ADM_GET_RMMAPPING_BYROLEID";
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



                SqlParameter P_ROLECODE = new SqlParameter();
                P_ROLECODE.ParameterName = "P_ROLECODE";
                P_ROLECODE.SqlDbType = SqlDbType.VarChar;
                P_ROLECODE.Value = roleid ?? (object)DBNull.Value;
                P_ROLECODE.Size = 250;
                db.cmd.Parameters.Add(P_ROLECODE);


                SqlParameter P_ORGCODE = new SqlParameter();
                P_ORGCODE.ParameterName = "P_ORGCODE";
                P_ORGCODE.SqlDbType = SqlDbType.VarChar;
                P_ORGCODE.Value = orgCode ?? (object)DBNull.Value; // Assuming obj.ID is a single value, not a list
                db.cmd.Parameters.Add(P_ORGCODE);




                var dr = db.DataReader(sql);

                List<VMGet_Menu_ByRoleID> data = new List<VMGet_Menu_ByRoleID>();

                while (dr.Read())
                {
                    VMGet_Menu_ByRoleID d = new VMGet_Menu_ByRoleID();
                    d.Code = dr["Code"].ToString();
                    d.MenuName = dr["MenuName"].ToString();
                    //d.IsAssigned = Convert.ToBoolean(dr["IsAssigned"]) ;


                    // Check for DBNull before conversion
                    if (dr["IsAssigned"] != DBNull.Value)
                    {
                        d.IsAssigned = Convert.ToBoolean(dr["IsAssigned"]);
                    }
                    else
                    {
                        // Handle DBNull case here, you can set it to false or any default value
                        d.IsAssigned = false; // or any default value suitable for your application
                    }



                    data.Add(d);



                }

                db.CloseConnection();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }


        }
        #endregion

        #region Function_To_Get_RoleMaster_By_OrgCode
        public List<SelectListItem> Get_RoleMaster_By_OrgCode(string orgid)
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_GET_ROLEMASTER_BYORGID";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_ORGID = new SqlParameter();
                P_ORGID.ParameterName = "P_ORGID";
                P_ORGID.SqlDbType = SqlDbType.VarChar;
                P_ORGID.Value = orgid ?? (object)DBNull.Value;
                P_ORGID.Size = 250;
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

                List<SelectListItem> list = new List<SelectListItem>();

                int RespCode = Convert.ToInt32(P_CODE.Value);

                while (dr.Read())
                {


                    SelectListItem data = new SelectListItem();




                    data.Text = dr["RoleName"].ToString();

                    data.Value = dr["Code"].ToString();


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

        #region Function_To_Insert_RMM
        public string Insert_RMM(VMRoleMenuMapping obj)
        {
            try
            {


                var user = _httpContextAccessor.HttpContext.User;

                //// Access UserName claim
                //var userName = user.FindFirst(ClaimTypes.Name)?.Value;

                // Access Id claim
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Access OrgCode claim
                var orgCode = user.FindFirst("OrgCode")?.Value;



                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_ADM_INSERTUPDATE_RMMAPING";
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_ORGCODE = new SqlParameter();
                P_ORGCODE.ParameterName = "P_ORGCODE";
                P_ORGCODE.SqlDbType = SqlDbType.VarChar;
                P_ORGCODE.Value = orgCode; // Assuming obj.ID is a single value, not a list
                db.cmd.Parameters.Add(P_ORGCODE);




                SqlParameter P_ROLECODE = new SqlParameter();
                P_ROLECODE.ParameterName = "P_ROLECODE";
                P_ROLECODE.SqlDbType = SqlDbType.VarChar;
                P_ROLECODE.Size = 10;
                P_ROLECODE.Value = obj.DropdownValue; // Assuming obj.ID is a single value, not a list
                db.cmd.Parameters.Add(P_ROLECODE);



                SqlParameter P_MENUCODE = new SqlParameter();
                P_MENUCODE.ParameterName = "P_MENUCODE";
                P_MENUCODE.TypeName = "ADM_UDT_RMM_MENUS";
                P_MENUCODE.SqlDbType = SqlDbType.Structured;
                P_MENUCODE.Value = ToRMMDataTable(obj.GridData); // Assuming obj.Roles is a List<int>
                db.cmd.Parameters.Add(P_MENUCODE);


                SqlParameter P_STATUS = new SqlParameter();
                P_STATUS.ParameterName = "P_Status";
                P_STATUS.SqlDbType = SqlDbType.Char;
                P_STATUS.Value = DBNull.Value;
                db.cmd.Parameters.Add(P_STATUS);


                SqlParameter P_CREATEDBY = new SqlParameter();
                P_CREATEDBY.ParameterName = "P_CREATEDBY";
                P_CREATEDBY.SqlDbType = SqlDbType.VarChar;
                P_CREATEDBY.Size = 200;
                P_CREATEDBY.Value = userId; // Assuming obj.CreatedBy is a single value, not a list
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

                var dr = db.FillData(sql);
                int RespCode = Convert.ToInt32(P_CODE.Value);

                string ResponseMsg = P_MESSAGE.Value.ToString();
                db.CloseConnection();
                if (RespCode == 0)
                {
                    return ResponseMsg;
                }
                else
                {
                    return ResponseMsg;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Function_To_Get_ToRMMDataTable
        public static DataTable ToRMMDataTable(List<GridData> items)
        {
            try
            {
                DataTable dataTable = new DataTable("ADM_UDT_RMM_MENUS");

                // Add columns matching the UDTT
                dataTable.Columns.Add("Code", typeof(string));
                dataTable.Columns.Add("IsAssigned", typeof(bool));
                dataTable.Columns.Add("IsEditView", typeof(bool));

                // Loop through all the items and add rows to the DataTable
                foreach (var item in items)
                {
                    DataRow row = dataTable.NewRow();
                    row["Code"] = item.Code;
                    row["IsAssigned"] = item.IsAssigned;
                    row["IsEditView"] = item.IsEditView;
                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating DataTable", ex);
            }
        }
        #endregion
    }
}
