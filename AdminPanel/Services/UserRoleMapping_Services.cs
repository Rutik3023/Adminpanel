

using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc.Rendering;
using  AdminPanel.Models;

namespace AdminPanel.Services
{
    public interface IUserRoleMapping
    {
        public string Insert_URM(VMUserRoleMapping obj);


        public List<VMUrm_By_Id> GetURM_ById(string userid);


        public List<SelectListItem> Get_User_By_OrgCode(string orgid);
    }


    public class UserRoleMapping_Services : IUserRoleMapping
    {
        IConfiguration configuration;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRoleMapping_Services(IConfiguration _configuration, IHttpContextAccessor httpContextAccessor)
        {
            configuration = _configuration;
            _httpContextAccessor = httpContextAccessor;
        }


        #region Function_To_Insert_URM
        public string Insert_URM(VMUserRoleMapping obj)
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

                string sql = "USP_ADM_INSERTUPDATE_USERROLEMAPPING";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_USERID = new SqlParameter();
                P_USERID.ParameterName = "P_USERID";
                P_USERID.SqlDbType = SqlDbType.VarChar;
                P_USERID.Size = 10;
                P_USERID.Value = obj.DropdownValue; // Assuming obj.ID is a single value, not a list
                db.cmd.Parameters.Add(P_USERID);

                SqlParameter P_STATUS = new SqlParameter();
                P_STATUS.ParameterName = "P_STATUS";
                P_STATUS.SqlDbType = SqlDbType.Bit;
                P_STATUS.Value = false;
                db.cmd.Parameters.Add(P_STATUS);

                SqlParameter P_EDITANDVIEW = new SqlParameter();
                P_EDITANDVIEW.ParameterName = "P_EDITANDVIEW";
                P_EDITANDVIEW.SqlDbType = SqlDbType.Bit;
                P_EDITANDVIEW.Value = false;
                db.cmd.Parameters.Add(P_EDITANDVIEW);

                SqlParameter P_ROLES = new SqlParameter();
                P_ROLES.ParameterName = "P_ROLES";
                P_ROLES.TypeName = "ADM_UDT_ROLES";
                P_ROLES.SqlDbType = SqlDbType.Structured;
                P_ROLES.Value = ToDataTable(obj.ID); // Assuming obj.Roles is a List<int>
                db.cmd.Parameters.Add(P_ROLES);

                SqlParameter P_CREATEDBY = new SqlParameter();
                P_CREATEDBY.ParameterName = "P_CREATEDBY";
                P_CREATEDBY.SqlDbType = SqlDbType.VarChar;
                P_CREATEDBY.Size = 200;
                P_CREATEDBY.Value = userId; // Assuming obj.CreatedBy is a single value, not a list
                db.cmd.Parameters.Add(P_CREATEDBY);



                SqlParameter P_ORGCODE = new SqlParameter();
                P_ORGCODE.ParameterName = "P_ORGCODE";
                P_ORGCODE.SqlDbType = SqlDbType.VarChar;
                P_ORGCODE.Value = orgCode; // Assuming obj.ID is a single value, not a list
                db.cmd.Parameters.Add(P_ORGCODE);



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

        #region Function_To_GetURM_ById
        public List<VMUrm_By_Id> GetURM_ById(string userid)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;
                var orgCode = user.FindFirst("OrgCode")?.Value;


                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_GET_URM_BYID";
                DBHelper db = new DBHelper();
                db.cmd.Parameters.Clear();
                db.OpenConnection(ConString);
                db.cmd.CommandType = CommandType.StoredProcedure;


                SqlParameter P_USERID = new SqlParameter();
                P_USERID.ParameterName = "P_USERID";
                P_USERID.SqlDbType = SqlDbType.VarChar;
                P_USERID.Value = userid ?? (object)DBNull.Value;
                P_USERID.Size = 250;
                db.cmd.Parameters.Add(P_USERID);

                SqlParameter @P_ORGCode = new SqlParameter();
                @P_ORGCode.ParameterName = "@P_ORGCode";
                @P_ORGCode.SqlDbType = SqlDbType.VarChar;
                @P_ORGCode.Value = orgCode ?? (object)DBNull.Value;
                @P_ORGCode.Size = 250;
                db.cmd.Parameters.Add(@P_ORGCode);

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

                List<VMUrm_By_Id> data = new List<VMUrm_By_Id>();

                while (dr.Read())
                {
                    VMUrm_By_Id d = new VMUrm_By_Id();
                    d.Code = dr["Code"].ToString();
                    d.RoleName = dr["RoleName"].ToString();
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
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Function_To_Get_ToDataTable
        public static DataTable ToDataTable(List<int> items)
        {
            try
            {

                DataTable dataTable = new DataTable("IntTable");

                // Add a single column for the integers
                dataTable.Columns.Add("Roles", typeof(int));

                // Loop through all the items and add rows to the DataTable
                foreach (var item in items)
                {
                    DataRow row = dataTable.NewRow();
                    row["Roles"] = item;
                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Functrion_To_Get_User_By_OrgCode
        public List<SelectListItem> Get_User_By_OrgCode(string orgid)
        {
            try
            {
                try
                {
                    string ConString = configuration.GetConnectionString("Myconnection");
                    string sql = "USP_ADM_GET_USERMASTER_BYORGID";
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




                        data.Text = dr["UserName"].ToString();

                        data.Value = dr["UserID"].ToString();


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
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        //public static DataTable ToDataTable(List<int> items)
        //{
        //    DataTable dataTable = new DataTable(typeof(int).Name);

        //    // Get all the properties
        //    var props = typeof(int).GetProperties();

        //    // Loop through all the properties and add columns to the DataTable
        //    foreach (var prop in props)
        //    {
        //        dataTable.Columns.Add(prop.Name, prop.PropertyType);
        //    }

        //    // Loop through all the items and add rows to the DataTable
        //    foreach (var item in items)
        //    {
        //        var values = new object[props.Length];
        //        for (int i = 0; i < props.Length; i++)
        //        {
        //            values[i] = props[i].GetValue(item, null);
        //        }
        //        dataTable.Rows.Add(values);
        //    }

        //    return dataTable;
        //}

    }

}
