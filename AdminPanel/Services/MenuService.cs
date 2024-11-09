
using System.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using  AdminPanel.Models;

namespace AdminPanel.Services
{


    public interface IMenuService
    {
        public List<MenuItem> GetUserMenus();


        public List<MenuItem> BuildMenuHierarchy(List<MenuItem> flatMenuItems);
    }
    public class MenuService : IMenuService
    {

        IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MenuService(IConfiguration configuration, IHttpContextAccessor htp)
        {
            this.configuration = configuration;
            _httpContextAccessor = htp;
        }




        #region Function_To_GetUserMenus
        public List<MenuItem> GetUserMenus()
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

                string sql = "USP_ADM_GETDYNAMICSIDEBARMENUS";
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



                SqlParameter P_UserId = new SqlParameter();
                P_UserId.ParameterName = "P_UserId";
                P_UserId.SqlDbType = SqlDbType.VarChar;
                P_UserId.Value = userId ?? (object)DBNull.Value;
                P_UserId.Size = 250;
                db.cmd.Parameters.Add(P_UserId);





                var reader = db.DataReader(sql);

                int RespCode = Convert.ToInt32(P_CODE.Value);

                var menuItems = new List<MenuItem>();

                while (reader.Read())
                {
                    var menuItem = new MenuItem
                    {
                        Code = reader.GetInt32(reader.GetOrdinal("Code")),
                        MenuName = reader.IsDBNull(reader.GetOrdinal("MenuName")) ? null : reader.GetString(reader.GetOrdinal("MenuName")),
                        Type = reader.IsDBNull(reader.GetOrdinal("Type")) ? null : reader.GetString(reader.GetOrdinal("Type")),
                        Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                        ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ParentId")),
                        MenuUrl = reader.IsDBNull(reader.GetOrdinal("MenuUrl")) ? null : reader.GetString(reader.GetOrdinal("MenuUrl")),
                        MenuSrNo = (int)(reader.IsDBNull(reader.GetOrdinal("MenuSrNo")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("MenuSrNo"))),
                        MenuDesc = reader.IsDBNull(reader.GetOrdinal("MenuDesc")) ? null : reader.GetString(reader.GetOrdinal("MenuDesc")),
                        MenuIcon = reader.IsDBNull(reader.GetOrdinal("MenuIcon")) ? null : reader.GetString(reader.GetOrdinal("MenuIcon"))
                    };
                    menuItems.Add(menuItem);
                }






                return menuItems;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Function_To_BuildMenuHierarchy
        public List<MenuItem> BuildMenuHierarchy(List<MenuItem> flatMenuItems)
        {
            var lookup = new Dictionary<int, MenuItem>();
            var rootItems = new List<MenuItem>();

            foreach (var item in flatMenuItems)
            {
                lookup[item.Code] = item;
            }

            foreach (var item in flatMenuItems)
            {
                if (item.ParentId.HasValue && lookup.ContainsKey(item.ParentId.Value))
                {
                    lookup[item.ParentId.Value].Children.Add(item);
                }
                else
                {
                    rootItems.Add(item);
                }
            }

            return rootItems;
        } 
        #endregion

    }




}
