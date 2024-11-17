
using  AdminPanel.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Management;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AdminPanel.CommonRepo
{
    public static class CommonFunction
    {


        private static string _connectionString;

        // Method to initialize the connection string
        public static void Initialize(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MyConnection");
        }
        public static UserLoginSession ActiveLoginCheck(LoginCrediential o)
        {
            try
            {
                string Con_String = _connectionString;//"Data Source=.\\MSSQLSERVER20;Initial Catalog=M_Freight;User ID=Sa;Password=Sql123;Trust Server Certificate=True";
                DBHelper db = new DBHelper();
                db.OpenConnection(Con_String);

                db.cmd.Parameters.Clear();
                string Sql = "USP_ADM_CHECK_LOGINSESSION_STATUS_FILTER";
                db.cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter P_USERID = new SqlParameter();
                P_USERID.ParameterName = "P_USERID";
                P_USERID.SqlDbType = SqlDbType.VarChar;
                P_USERID.Size = 10;
                P_USERID.Value = o.Id;
                db.cmd.Parameters.Add(P_USERID);


                SqlParameter P_SESSIONID = new SqlParameter();
                P_SESSIONID.ParameterName = "P_SESSIONID";
                P_SESSIONID.SqlDbType = SqlDbType.VarChar;
                P_SESSIONID.Size = 50;
                P_SESSIONID.Value = o.SessionId;
                db.cmd.Parameters.Add(P_SESSIONID);



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
                }

                UserLoginSession obj = new UserLoginSession();
                while (dr.Read())
                {
                    obj.UserId = dr["UserId"].ToString();
                    obj.Device_OS = dr["Device_OS"].ToString();
                    obj.Browser_Detail = dr["Browser_Detail"].ToString();
                    obj.Device_Location = dr["Device_Location"].ToString();
                    obj.LoginTime = dr["LoginTime"].ToString();
                    obj.Remarks = dr["Remarks"].ToString();


                }
                db.CloseConnection();
                return obj;


                return OBJ;


            }
            catch (Exception ex)
            
            +{

                return null;
                throw;
            }


        }

        private static readonly Dictionary<string, string> MimeTypeMappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            { ".pdf", "application/pdf" },
          
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".html", "text/html" },
            { ".txt", "text/plain" },
            { ".doc", "application/msword" },
            { ".xls", "application/vnd.ms-excel" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".zip", "application/zip" },
        // Add more mappings as needed
        };

        //public static List<UdtData> GetCheckBoxValues(Organization_model obj)
        //{
        //    try
        //    {

        //        List<int> CheckboxValues = new List<int>();

        //        //if (obj.isOcean)
        //        //{
        //        //    CheckboxValues.Add(1);
        //        //}

        //        //if (obj.isLand)
        //        //{
        //        //    CheckboxValues.Add(2);
        //        //}

        //        //if (obj.isAir)
        //        //{
        //        //    CheckboxValues.Add(3);
        //        //}


        //        // Concatenate roles with commas
        //        string concatenatedRoles = string.Join(",", CheckboxValues);

        //        // Split concatenatedRoles into an array
        //        string[] CheckboxAsIntArray = concatenatedRoles.Split(',');


        //        List<UdtData> d = new List<UdtData>();


        //        foreach (var item in CheckboxAsIntArray)
        //        {
        //            UdtData u = new UdtData();

        //            u.ModelID = item;

        //            d.Add(u);
        //        }

        //        return d;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public static string GenerateAuthTokenRegisterDevice(int length = 8) // Shorter token, e.g., 8 bytes
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[length];
                rng.GetBytes(tokenData);

                // Convert the byte array to a Base64 string
                string token = Convert.ToBase64String(tokenData);

                // Remove any padding characters ('=') and make it URL-friendly
                return token.TrimEnd('=').Replace('+', '-').Replace('/', '_');
            }
        }


        public static List<T> ConvertArrayToList<T>(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            return array.ToList();
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static List<T> MapToList<T>(SqlDataReader reader) where T : new()
        {
            List<T> list = new List<T>();
            Type objType = typeof(T);
            var properties = objType.GetProperties();

            while (reader.Read())
            {
                T obj = new T();
                foreach (var property in properties)
                {
                    try
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                        {
                            PropertyInfo propInfo = objType.GetProperty(property.Name);
                            // Convert.ChangeType may not handle all conversions properly, especially for complex types
                            // You may need to implement custom conversion logic if necessary
                            propInfo.SetValue(obj, Convert.ChangeType(reader[property.Name], propInfo.PropertyType), null);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception or log if necessary
                        Console.WriteLine($"Error mapping property {property.Name}: {ex.Message}");
                    }
                }
                list.Add(obj);
            }

            return list;
        }



        public static string GetMimeType(string extension)
        {
            try
            {
                // Remove leading and trailing quotes if present
                extension = extension.Trim();

                if (MimeTypeMappings.TryGetValue(extension, out string mimeType))
                {
                    return mimeType;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

          

            return "application/octet-stream"; // Default to binary stream if not found
        }

        //// Define the scopes for Gmail API
        //static string[] Scopes = { GmailService.Scope.GmailSend };
        //static string ApplicationName = "LACL";


//        public static void mailalert(MailAlert o)
//        {

//            try
//            {



//                // Your Gmail address
//                string SenderEmail = o.SenderMail;
//                string SenderName = o.SenderName;

//                // Recipient email address
//                string recipientEmail = o.ReceiverEmail;
//                string recipientName = o.ReceiverName;

//                // Get the current date
//                string currentDate = DateTime.Now.ToString("dd-MM-yyyy");

//                // Mail subject and body
//                string subject = "Key Fact Statement Mailout Summary";
//                string body = $@"
//<head>
//    <style>
//        table {{
//            border-collapse: collapse;
//            width: 100%;
//        }}
//        th, td {{
//            border: 1px solid black;
//            padding: 8px;
//            text-align: center;
//        }}
//        th {{
//            background-color: #f2f2f2;
//        }}
//    </style>
//</head>
//<body>
//    <p>Hi {o.ReceiverName},</p>
//    <p>We are pleased to inform you that the user has been successfully registered. Below are the details:</p>
//    <table>
//        <tr>
//            <th style=""background-color: #6082B6; color: white;""><b>Email</b></th>
//            <th style=""background-color: #6082B6; color: white;""><b>Password</b></th>
//        </tr>
//        <tr>
//            <td>{o.UserName}</td>
//            <td>{o.Password}</td>
//        </tr>
//    </table>
//    <p><strong>Note:</strong> If you encounter any issues, please contact the support team.</p>
//    <p><i><strong>Disclaimer:</strong> This is a system-generated email. For any concerns, please coordinate with the support team.</i></p>
//</body>
//</html>";


//                // Create Gmail service using OAuth 2.0 authentication
//                GmailService service = GetGmailService();

//                // Create the MimeMessage
//                var message = new MimeMessage();
//                message.From.Add(new MailboxAddress(SenderName, SenderEmail));
//                message.To.Add(new MailboxAddress(recipientName, recipientEmail));
//                message.Subject = subject;
//                message.Body = new TextPart("html")
//                {
//                    Text = body
//                };

//                // Convert MimeMessage to Google API message
//                var rawMessage = Base64UrlEncode(message.ToString());
//                var gmailMessage = new Message
//                {
//                    Raw = rawMessage
//                };

//                // Send the email
//                service.Users.Messages.Send(gmailMessage, "me").Execute();





//                // Pause the program to keep the console window open


//            }
//            catch (Exception ex)
//            {



//            }
//        }

        // Encode the email message to base64 URL-safe string
        private static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }

        // Create Gmail service with OAuth 2.0 authentication
        //private static GmailService GetGmailService()
        //{
        //    try
        //    {
        //        UserCredential credential;
        //        string CredentialJson = "D:\\M_Freight Git\\ AdminPanel\\Credential.json";
        //        using (var stream =
        //            new FileStream(CredentialJson, FileMode.Open, FileAccess.Read))
        //        {
        //            // The file token.json stores the user's access and refresh tokens, and is created
        //            // automatically when the authorization flow completes for the first time.
        //            string credPath = "token.json";
        //            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //                GoogleClientSecrets.Load(stream).Secrets,
        //                Scopes,
        //                "user",
        //                CancellationToken.None,
        //                new FileDataStore(credPath, true)).Result;
        //            Console.WriteLine("Credential file saved to: " + credPath);
        //        }

        //        // Create Gmail API service.
        //        return new GmailService(new BaseClientService.Initializer()
        //        {
        //            HttpClientInitializer = credential,
        //            ApplicationName = ApplicationName,
        //        });
        //    }
        //    catch (Exception ex)
        //    {


        //        return null;
        //    }

        //}


        // Common functions for login 

        public static string Base64Decode(string base64EncodedData)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string GetWmiIdentifier(string wmiClass, string wmiProperty)
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT {wmiProperty} FROM {wmiClass}");
                foreach (ManagementObject obj in searcher.Get())
                {
                    return obj[wmiProperty]?.ToString() ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while querying {wmiClass}: {ex.Message}");
            }
            return string.Empty;
        }
    }
}



