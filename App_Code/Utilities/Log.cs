using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for Log
/// </summary>
public class Log
{
    public static bool StartLog(string ProcessName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Environment.NewLine+ string.Format("========================================Start of {0}============================================" + Environment.NewLine, ProcessName));
            sb.Append(string.Format("IP: {0}",Common.GetClientIP)+Environment.NewLine);
            sb.Append(string.Format("DateTime: {0}",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")) + Environment.NewLine);
            // flush every 20 seconds as you do it
            File.AppendAllText(HttpContext.Current.Server.MapPath("~\\Logs")+"\\Log_"+DateTime.Now.ToString("dd-MM-yyyy")+".txt", sb.ToString());
            sb.Clear();
            return true ;
        }
        catch
        {
            return false ;
 
        }
    }

    public static bool LogInfo(string Message)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("I:" + Message + Environment.NewLine);
            // flush every 20 seconds as you do it
            File.AppendAllText(HttpContext.Current.Server.MapPath("~\\Logs") + "\\Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt", sb.ToString());
            sb.Clear();
            return true;
        }
        catch
        {
            return false;

        }
    }
    public static bool LogWarning(string Message)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("W:" + Message + Environment.NewLine);
            // flush every 20 seconds as you do it
            File.AppendAllText(HttpContext.Current.Server.MapPath("~\\Logs") + "\\Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt", sb.ToString());
            sb.Clear();
            return true;
        }
        catch
        {
            return false;

        }
    }
    public static bool LogError(string Message)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("E:" + Message + Environment.NewLine);
            // flush every 20 seconds as you do it
            File.AppendAllText(HttpContext.Current.Server.MapPath("~\\Logs") + "\\Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt", sb.ToString());
            sb.Clear();
            return true;
        }
        catch
        {
            return false;

        }
    }

    public static bool EndLog(string ProcessName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("=======================================End Of {0}============================================" + Environment.NewLine, ProcessName));
            // flush every 20 seconds as you do it
            File.AppendAllText(HttpContext.Current.Server.MapPath("~\\Logs") + "\\Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt", sb.ToString());
            sb.Clear();
            return true;
        }
        catch
        {
            return false;

        }
    }

    public static bool AddAPILog(string ApiCall, string ServiceMethod, string SessionID, CallResult result, string Error)
    {
        try
        {
           // return SqlHelper.ExecuteNonQuery(System.Configuration.ConfigurationManager.ConnectionStrings["APILogManager"].ConnectionString , "InsertUpdateApiLog", ApiCall, SessionID, result, Error, ServiceMethod) > 0;

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["APILogManager"].ConnectionString;
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter() { DbType = System.Data.DbType.String, Value = ApiCall, ParameterName = "ApiCall" });
                param.Add(new SqlParameter() { DbType = System.Data.DbType.String, Value = SessionID, ParameterName = "Session" });
                param.Add(new SqlParameter() { DbType = System.Data.DbType.Int32, Value = ((int)result), ParameterName = "Result" });
                param.Add(new SqlParameter() { DbType = System.Data.DbType.String, Value = Error, ParameterName = "ErrorMessage" });
                param.Add(new SqlParameter() { DbType = System.Data.DbType.String, Value = ServiceMethod, ParameterName = "ServiceMethod" });
                cmd.Parameters.AddRange(param.ToArray());
                cmd.CommandText = string.Format("InsertUpdateApiLog ");
                bool flag = cmd.ExecuteNonQuery() > 0;
                return flag;
            }
            
            

        }
        catch(Exception ex)
        {
            try
            {
                StartLog(ServiceMethod);
                LogError(ex.ToString());
                LogInfo(string.Format("This Api CAll is logged on [{0}] calling method [{1}] against SessionId [{2}] with Result [{3}] and Error [{4}] \n APICALL {5}", DateTime.Now.ToString("dd/MM/yyyy"), ServiceMethod, SessionID, result, Error,ApiCall));
                EndLog(ServiceMethod);

            }
            catch
            { //do nothing 
            }

            return false;
        }

        // inserting log in odata
        //try
        //{
        //    var context = ExigoApiContext.CreateApiLogContext();
        //    Exigo.OData.ApiLog.ApiLogs obj = new Exigo.OData.ApiLog.ApiLogs();
        //    obj.ApiCall = ApiCall.ToString();
        //    obj.Session = SessionID.ToString();
        //    obj.Result = ((int)result);
        //    obj.ErrorMessage = Error.ToString();
        //    obj.ServiceMethod = ServiceMethod.ToString();
        //    obj.CreatedDate = DateTime.Now;
        //    context.AddToApiLogs(obj);
        //    context.SaveChanges();
        //    return true;
        //}
        //catch
        //{
        //    return false;
        //}

    }
}