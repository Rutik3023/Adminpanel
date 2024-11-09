



using Microsoft.AspNetCore.Mvc.Rendering;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using  AdminPanel.Models;
using AdminPanel.CommonRepo;


namespace AdminPanel.Services
{

    public interface IMenuMaster
    {

        string INSERTUPDATE(MenuMaster OBJ, List<string> orgCodeList);
        bool DELETEMENU(int Code);

        MenuMaster GETMENUBYID(int ID);

        List<MenuMaster> GETALLMENU();

        List<MenuMaster> GETALLORG();
    }
    public class MenuMaster_Service : IMenuMaster
    {
        IConfiguration configuration;
        public MenuMaster_Service(IConfiguration _configuration)
        {
            configuration = _configuration;
        }



        #region Function_To_DELETEMENU
        //Delete
        public bool DELETEMENU(int Code)
        {
            try
            {
                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_ADM_DELETE_MENUMASTER_BYID";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_MENUCODE = new SqlParameter();
                P_MENUCODE.ParameterName = "P_MENUCODE";
                P_MENUCODE.SqlDbType = SqlDbType.Int;

                P_MENUCODE.Value = Code;
                db.cmd.Parameters.Add(P_MENUCODE);

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
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Function_To_GETALLMENU
        //List -- fetch all menu records --Prasad
        public List<MenuMaster> GETALLMENU()
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_MENUEMASTERLIST";
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

                List<MenuMaster> OrganizationData = CommonFunction.MapToList<MenuMaster>(dr);

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

        #region Function_To_GETMENUBYID
        //Edit - fetch Menu record by id --Prasad
        public MenuMaster GETMENUBYID(int ID)
        {
            try
            {
                string Con_Connection = configuration.GetConnectionString("MyConnection");
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_Connection);

                string sql = "USP_ADM_GET_MENUEMASTER_BYID";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_MENUCODE = new SqlParameter();
                P_MENUCODE.ParameterName = "P_MENUCODE";
                P_MENUCODE.SqlDbType = SqlDbType.Int;
                P_MENUCODE.Value = ID;
                db.cmd.Parameters.Add(P_MENUCODE);

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
                MenuMaster menuList = new MenuMaster();

                while (dr.Read())
                {

                    menuList.Code = dr["Code"] != DBNull.Value ? dr["Code"].ToString() : "";
                    menuList.MenuName = dr["MenuName"] != DBNull.Value ? dr["MenuName"].ToString() : "";
                    menuList.MenuDesc = dr["MenuDesc"] != DBNull.Value ? dr["MenuDesc"].ToString() : "";
                    menuList.ParentId = dr["ParentId"] != DBNull.Value ? dr["ParentId"].ToString() : null;
                    menuList.MenuUrl = dr["MenuUrl"] != DBNull.Value ? dr["MenuUrl"].ToString() : "";
                    menuList.MenuIcon = dr["MenuIcon"] != DBNull.Value ? dr["MenuIcon"].ToString() : null;
                    menuList.MenuSrNo = dr["MenuSrNo"] != DBNull.Value ? dr["MenuSrNo"].ToString() : null;
                    menuList.Type = dr["Type"] != DBNull.Value ? dr["Type"].ToString() : "";
                    menuList.status = dr["status"] != DBNull.Value ? dr["status"].ToString() : "";




                }
                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        MenuMaster data = new MenuMaster();
                        data.ORG_Menu_MapCode = dr["ORG_Menu_MapCode"] != DBNull.Value ? dr["ORG_Menu_MapCode"].ToString() : "";
                        data.Organization_Name = dr["Organization_Name"] != DBNull.Value ? dr["Organization_Name"].ToString() : "";
                        data.Assigned = dr["Assigned"] != DBNull.Value ? Convert.ToBoolean(dr["Assigned"]) : false;
                        data.Org_Code = dr["Org_Code"] != DBNull.Value ? dr["Org_Code"].ToString() : "";
                        menuList.Orglist.Add(data);
                    }
                }

                db.CloseConnection();
                return menuList;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Function_To_INSERTUPDATE
        //Insert and Update Menu ---Prasad
        public string INSERTUPDATE(MenuMaster obj, List<string> orgCodeList)
        {
            if (obj.Code == null)
            {
                obj.Code = "0";
            }


            try
            {
                string Con_String = configuration.GetConnectionString("MyConnection");

                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);
                string sql = "USP_ADM_INSERTUPDATE_MENUMASTER";
                db.cmd.CommandType = CommandType.StoredProcedure;
                db.cmd.Parameters.Clear();

                SqlParameter P_CODE = new SqlParameter();
                P_CODE.ParameterName = "P_CODE";
                P_CODE.SqlDbType = SqlDbType.Int;
                P_CODE.Value = obj.Code ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_CODE);

                SqlParameter P_ORGCODE = new SqlParameter();
                P_ORGCODE.ParameterName = "P_ORGCODE";
                P_ORGCODE.TypeName = "ADM_UDT_ORGANIZATIONS";
                P_ORGCODE.SqlDbType = SqlDbType.Structured;
                P_ORGCODE.Value = ToDataTable(orgCodeList); // Assuming OrgCode is a list
                db.cmd.Parameters.Add(P_ORGCODE);

                SqlParameter P_MENUNAME = new SqlParameter();
                P_MENUNAME.ParameterName = "P_MENUNAME";
                P_MENUNAME.SqlDbType = SqlDbType.VarChar;
                P_MENUNAME.Size = 50;
                P_MENUNAME.Value = obj.MenuName;
                db.cmd.Parameters.Add(P_MENUNAME);

                SqlParameter P_MENUDESC = new SqlParameter();
                P_MENUDESC.ParameterName = "P_MENUDESC";
                P_MENUDESC.SqlDbType = SqlDbType.VarChar;
                P_MENUDESC.Size = 100;
                P_MENUDESC.Value = obj.MenuDesc ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_MENUDESC);

                SqlParameter P_PARENTID = new SqlParameter();
                P_PARENTID.ParameterName = "P_PARENTID";
                P_PARENTID.SqlDbType = SqlDbType.Int;
                P_PARENTID.Value = string.IsNullOrEmpty(obj.ParentId) ? (object)DBNull.Value : int.Parse(obj.ParentId);
                db.cmd.Parameters.Add(P_PARENTID);

                SqlParameter P_MENUURL = new SqlParameter();
                P_MENUURL.ParameterName = "P_MENUURL";
                P_MENUURL.SqlDbType = SqlDbType.VarChar;
                P_MENUURL.Size = 100;
                P_MENUURL.Value = obj.MenuUrl;
                db.cmd.Parameters.Add(P_MENUURL);

                SqlParameter P_MENUICON = new SqlParameter();
                P_MENUICON.ParameterName = "P_MENUICON";
                P_MENUICON.SqlDbType = SqlDbType.VarChar;
                P_MENUICON.Size = 50;
                P_MENUICON.Value = obj.MenuIcon ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_MENUICON);

                SqlParameter P_MENUSRNO = new SqlParameter();
                P_MENUSRNO.ParameterName = "P_MENUSRNO";
                P_MENUSRNO.SqlDbType = SqlDbType.Int;
                P_MENUSRNO.Value = string.IsNullOrEmpty(obj.MenuSrNo) ? (object)DBNull.Value : int.Parse(obj.MenuSrNo);
                db.cmd.Parameters.Add(P_MENUSRNO);

                SqlParameter P_TYPE = new SqlParameter();
                P_TYPE.ParameterName = "P_TYPE";
                P_TYPE.SqlDbType = SqlDbType.VarChar;
                P_TYPE.Size = 10;
                P_TYPE.Value = obj.Type ?? (object)DBNull.Value;
                db.cmd.Parameters.Add(P_TYPE);

                SqlParameter P_STATUS = new SqlParameter();
                P_STATUS.ParameterName = "P_STATUS";
                P_STATUS.SqlDbType = SqlDbType.Char;
                P_STATUS.Value = obj.status;
                db.cmd.Parameters.Add(P_STATUS);

                SqlParameter P_CREATEDBY = new SqlParameter();
                P_CREATEDBY.ParameterName = "P_CREATEDBY";
                P_CREATEDBY.SqlDbType = SqlDbType.Int;
                P_CREATEDBY.Value = obj.CreatedBy;
                db.cmd.Parameters.Add(P_CREATEDBY);

                SqlParameter P_OUTCODE = new SqlParameter();
                P_OUTCODE.ParameterName = "P_OUTCODE";
                P_OUTCODE.SqlDbType = SqlDbType.Int;
                P_OUTCODE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_OUTCODE);

                SqlParameter P_MESSAGE = new SqlParameter();
                P_MESSAGE.ParameterName = "P_MESSAGE";
                P_MESSAGE.SqlDbType = SqlDbType.VarChar;
                P_MESSAGE.Size = 200;
                P_MESSAGE.Direction = ParameterDirection.Output;
                db.cmd.Parameters.Add(P_MESSAGE);

                db.FillData(sql);

                string ResponseMsg = P_MESSAGE.Value.ToString();
                db.CloseConnection();
                return ResponseMsg;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in INSERTUPDATE method", ex);
            }
        }

        #endregion

        #region Function_To_GETALLORG
        public List<MenuMaster> GETALLORG()
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_GEN_GETALL_MENUORGANIZATIONForm";
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

                List<MenuMaster> OrganizationData = CommonFunction.MapToList<MenuMaster>(dr);

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

        #region Function_To_Get_ToDataTable
        public static DataTable ToDataTable(List<string> items)
        {
            DataTable dataTable = new DataTable("IntTable");

            // Add a single column for the integers
            dataTable.Columns.Add("ORGCODE", typeof(string));

            // Loop through all the items and add rows to the DataTable
            foreach (var item in items)
            {
                DataRow row = dataTable.NewRow();
                row["ORGCODE"] = item;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        #endregion
    }
}
