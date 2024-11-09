
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using  AdminPanel.Models;
using AdminPanel.CommonRepo;


namespace AdminPanel.Services
{

    public interface IExceptionLog
    {

      List<ExceptionLog> GETALLMENU();
    }
    public class ExceptionLog_Service : IExceptionLog
    {
        IConfiguration configuration;
        public ExceptionLog_Service(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #region Function_To_GETALLMENU
        public List<ExceptionLog> GETALLMENU()
        {
            try
            {
                string ConString = configuration.GetConnectionString("Myconnection");
                string sql = "USP_ADM_GET_EXCEPTIONSLIST";
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

                List<ExceptionLog> OrganizationData = CommonFunction.MapToList<ExceptionLog>(dr);

                int RespCode = Convert.ToInt32(P_CODE.Value);
                // string RespMsg = P_Message.Value.ToString();


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
    }
}
