
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using  AdminPanel.Models;
using AdminPanel.Services;
using AdminPanel.CommonRepo;
using Newtonsoft.Json.Linq; 

namespace AdminPanel.Controllers
{
  
    public class LoginController : Controller
    {
        IGeneral General;
        IUserValidationService uv;
        IDeviceDetailse _devicedetailse;
        private readonly IHttpContextAccessor _httpContextAccessor;
        IUserMaster userMaster;
        IEmployee Employee;
        public LoginController(IGeneral _general, IUserValidationService _uv, IDeviceDetailse devicedetailse, IUserMaster userMaster, IHttpContextAccessor httpContextAccessor, IEmployee employee)
        {
            General = _general;
            uv = _uv;
            _devicedetailse = devicedetailse;
            this.userMaster = userMaster;
            _httpContextAccessor = httpContextAccessor;
            Employee = employee;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Index(Login obj)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    LoginCrediential data = General.LoginCheck(obj);
                    if (data.Code == 2)
                    {
                        ViewBag.RespMsg = data.Message;
                        return View();
                    }
                    #region 1.Check Org No. Validation
                    if (obj != null)
                    {
                        var resp = uv.Get_Org_License_By_OrgCode(obj.saas_Org_Code);




                        if (resp != null && resp.Sys_Code == "SA")
                        {

                        }
                        else if (resp != null)
                        {
                            var resp1 = uv.Check_OrgNo_UserName_Validation(resp.OrgCode, obj.User_Name);


                            if (resp1 == 0)
                            {




                                ResponceMessage res = uv.ValidateOrgLicencebyorgcode(resp.OrgCode);


                                if (res.code != 0)
                                {
                                    ViewBag.InvalidOrgNo = "Organization license is expire. Please contact the administrator.";
                                    return View();

                                }



                                //string decodedString = CommonFunction.Base64Decode(resp.AuthKey);

                                //// Split decoded string by the delimiter
                                //string[] parts = decodedString.Split('|');

                                //if (parts.Length == 2)
                                //{
                                //    // Parse Valid_from and Valid_till dates
                                //    DateTime valid_from;
                                //    DateTime valid_till;

                                //    if (DateTime.TryParse(parts[0], out valid_from) && DateTime.TryParse(parts[1], out valid_till))
                                //    {


                                //        // Compare current date with Valid_from and Valid_till
                                //        DateTime currentDate = DateTime.Now; /*new DateTime(2012, 6, 25);*/

                                //        if (!(currentDate >= valid_from && currentDate <= valid_till))
                                //        {

                                //            // If system date not comes between Valid_from and valid_till date then showing below msg
                                //            ViewBag.InvalidOrgNo = "Kindly entered correct Saas/Org No,else contact Administration.";
                                //            return View();
                                //        }


                                //    }

                                //}
                            }
                            else if (resp1 == 1)
                            {
                                ViewBag.InvalidOrgNo = "Kindly entered correct Saas/Org No,else contact Administration.";
                                return View();
                            }
                            else if (resp1 == 2)
                            {

                                //ViewBag.RespMsg =data.Message ;
                                ViewBag.RespMsg = "Invalid email or password.Please check your credentials and try again.";

                                return View();
                            }
                            else if (resp1 == 3)
                            {
                                ViewBag.RespMsg = "Your have reached max attempts to login for the day.your login account is locked . Please Contact Admin to re-Active Your Account";
                                return View();
                            }
                            else
                            {
                                ViewBag.InvalidOrgNo = "Kindly entered correct Saas/Org No,else contact Administration.";
                                return View();
                            }

                        }
                        else
                        {
                            ViewBag.InvalidOrgNo = "Kindly entered correct Saas/Org No,else contact Administration.";
                            return View();
                        }


                    }
                    #endregion

                    #region 2.Check UserID and Password Validation
                    if (obj != null)
                    {


                        if (data.Code == -1)
                        {

                            ViewBag.RespMsg = "Invalid email or password.Please check your credentials and try again.";
                            //ViewBag.RespMsg = data.Message;
                            return View();
                        }

                    }
                    #endregion

                    #region Multiple Login Check

                    Login ActiveUserData = General.ActiveLoginCheck(data);
                    if (ActiveUserData.UserId != null)
                    {

                        ViewBag.ActiveUserData = ActiveUserData;
                        return View();
                    }
                    else
                    {



                        var resultData = _devicedetailse.GetUserAgentInfo(Request);
                        #region Funtion to find location
                        string publicIPAD = await GetPuclicIP();

                        JObject Jobj = await GetLocationDetails(publicIPAD);
                        var city = Jobj["city"]?.ToString();
                        var region = Jobj["region"]?.ToString();
                        var country = Jobj["country"]?.ToString();
                        var loc = Jobj["loc"]?.ToString();
                        var org = Jobj["org"]?.ToString();
                        var postal = Jobj["postal"]?.ToString();
                        var timezone = Jobj["timezone"]?.ToString();
                        #endregion

                        DeviceDetails MachineDT = new DeviceDetails();
                        MachineDT.UserCode = data.Id;
                        MachineDT.OS = resultData.OS;
                        MachineDT.Browser = resultData.Browser;
                        string uniqueCode = Guid.NewGuid().ToString();
                        MachineDT.DuniqueIdentifier = uniqueCode;
                        MachineDT.Location = country + '/' + region + '/' + city;
                        data.SessionId = uniqueCode;

                        ResponceMessage RespMsg = General.InsertUserLoginSession(MachineDT);
                    }




                   



                    #endregion

                    #region 3.Check Machine Details Validation
                    if (obj != null)
                    {

                        //string biosSerial = CommonFunction.GetWmiIdentifier("Win32_BIOS", "SerialNumber");
                        //string motherboardSerial = CommonFunction.GetWmiIdentifier("Win32_BaseBoard", "SerialNumber");



                        //check Logedin device is register or not

                        var result = _devicedetailse.GetUserAgentInfo(Request);

                        DeviceDetails checkMD = new DeviceDetails();

                        checkMD.UserCode = data.Id;

                        checkMD.OS = result.OS;
                        checkMD.Browser = result.Browser;
                        checkMD.DuniqueIdentifier = obj.DuniqueIdentifier;

                        //DeviceDetails checkMD = new DeviceDetails();

                        //checkMD.UserCode = data.Id;
                        //checkMD.MotherboardSerialNo = motherboardSerial;
                        //checkMD.BiosSerialNo = biosSerial;
                        //checkMD.Deviceuniquecode = obj.DuniqueIdentifier;


                        var machineCheck = uv.Check_Machine_Details_Validation(checkMD);


                        if (machineCheck == 1)
                        {

                            ViewBag.UserIdForMAchineDetails = data.Id;
                            ViewBag.Loginsessionstatus = data.Id;
                            return View();

                        }
                        else if (machineCheck == 2)
                        {
                            ViewBag.LoginLimitOver = "Your limit for login on multiple devices is over.If you want to login on new device please de-active previous device";
                            ViewBag.UserIdForActiveDeactivePage = data.Id;
                            ViewBag.Loginsessionstatus = data.Id;
                            return View();
                        }
                        else if (machineCheck == 3)
                        {
                            ViewBag.LoginLimitOver = "Acitve device please";
                            ViewBag.UserIdForActiveDeactivePage = data.Id;
                            ViewBag.Loginsessionstatus = data.Id;
                            return View();
                        }

                    }
                    #endregion

                    #region After all validation we are generating claims
                    if (data != null)
                    {


                        var UserName = data.Name;
                        var Id = data.Id;

                        // Split roles string into an array
                        string[] roles = data.Roles.Split(',');

                        // Create claims array for UserName and Id
                        var claims = new List<Claim>
                         {
                         new Claim(ClaimTypes.Name, UserName),
                         new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                         new Claim("OrgCode", data.OrgCode ?? string.Empty),
                         new Claim("Orgname", data.OrgName ?? string.Empty),
                         new Claim("Email", data.Email ?? string.Empty),
                         new Claim("SessionId", data.SessionId ?? string.Empty),
                         new Claim("isChecker", data.isChecker.HasValue ? data.isChecker.Value.ToString() : string.Empty)
                         };

                        // Add role claims to the claims array
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.Trim())); // Trim to remove leading/trailing spaces
                        }

                        // Create ClaimsIdentity with all claims
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // Sign in with the created identity
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                        // Check roles and redirect accordingly
                        foreach (var role in roles)
                        {
                            //if (role.Trim().Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase)) // Case-insensitive comparison
                            //{
                            //    return RedirectToAction("Index", "MasterDashboard");
                            //}

                            if (role.ToLower() == "Super Admin".ToLower())
                            {
                                return RedirectToAction("Index", "MasterDashboard");
                            }
                        }



                        uv.UpdateDeviceDetailsFlg(data.Id, obj.DuniqueIdentifier);

                        // Default redirect
                        return RedirectToAction("Index", "MasterDashboard");

                    }
                    #endregion


                }

                return View();
            }
            catch (Exception er)
            {


                General.Write_Log_Into_Txt(er.Message, "LoginController");
                General.InsertExcepstionlog(er.Message, "LoginController");
                return View("Error");
            }

        }


        // Logout
        public async Task<IActionResult> Logout(string id = "", string flag = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(flag))
                {
                    General.UpdateUserLoginSession(id, 1);
                    return RedirectToAction("Index", "Login");
                }
                else if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(flag))
                {
                    General.UpdateUserLoginSession(id, 0);
                    return Json(null);
                }
                else if (flag == "3")
                {
                    return RedirectToAction("Index", "Login");
                }

                var user = _httpContextAccessor.HttpContext.User;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                ResponceMessage ResMsg = General.UpdateUserLoginSession(userId, 0);
                await HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Login");
            }
            catch (Exception er)
            {

                General.Write_Log_Into_Txt(er.Message, "LoginController");
                General.InsertExcepstionlog(er.Message, "LoginController");
                return View("Error");
            }
        }


        [HttpGet]
        public IActionResult Forget_User_validation(string Mail)
        {
            try
            {
                ResponceMessage Msg = uv.UserMailvalidationCheck(Mail);
                if (Msg.code == 0)
                {

                    bool Resp = uv.mailalert_ForgetUsername(Msg.Message.UserName, Mail, Msg.Message.FullName);
                    if (Resp)
                    {
                        return Json(Msg);
                    }
                }
                return Json(Msg.code);
            }
            catch (Exception er)
            {

                General.Write_Log_Into_Txt(er.Message, "LoginController");
                General.InsertExcepstionlog(er.Message, "LoginController");
                return View("Error");
            }
        }




        [HttpGet]
        public IActionResult Forget_Password_Validation(string Mail, string UserName)
        {
            try
            {
                // Validate the password reset request
                ResponceMessage Msg = uv.PassswordMailvalidationCheck(Mail, UserName);

                if (Msg.code == 0)
                {

                    bool Resp = uv.mailalert_ForgetPassword(Msg.Message.Email, Msg.Message.Username, Msg.Message.FullName);
                    if (Resp)
                    {
                        // Return success message with the response details
                        return Json(Msg);
                    }

                }

                // Return an error message if validation fails
                return Json(new { success = false, message = "Invalid email or username. Please try again." });
            }
            catch (Exception er)
            {
                // Log the error and return a generic error message
                General.Write_Log_Into_Txt(er.Message, "LoginController");
                General.InsertExcepstionlog(er.Message, "LoginController");

                return Json(new { success = false, message = "An error occurred while processing your request. Please try again later." });
            }
        }


        [HttpGet]
        public IActionResult Reset_Password(string email, string Username, string Fullname)
        {
            try
            {
                byte[] byteArray = Convert.FromBase64String(email);

                // Convert the byte array to a string
                string decodedString = System.Text.Encoding.UTF8.GetString(byteArray);


                byte[] Fullnamebtarray = Convert.FromBase64String(Fullname);

                // Convert the byte array to a string
                string decodedFullname = System.Text.Encoding.UTF8.GetString(Fullnamebtarray);

                byte[] Usernamebtarray = Convert.FromBase64String(Username);

                // Convert the byte array to a string
                string decodedUsername = System.Text.Encoding.UTF8.GetString(Usernamebtarray);

                bool Status_Check = uv.Check_USER_PASSWORDCHANGE_LINK_Status(decodedString);
                if (!Status_Check)
                {
                    return View("Expire_Link");
                }

                ViewBag.Email = email;


                ViewBag.Username = decodedUsername;
                ViewBag.Fullname = decodedFullname;

                return View();
            }
            catch (Exception er)
            {

                General.Write_Log_Into_Txt(er.Message, "LoginController");
                General.InsertExcepstionlog(er.Message, "LoginController");
                return View("Error");
            }
        }


        [HttpPost]
        public IActionResult Update_New_Password([FromBody] ForgetUserPassword obj)
        {

            try
            {
                byte[] byteArray = Convert.FromBase64String(obj.Email);

                // Convert the byte array to a string
                string decodedEmail = System.Text.Encoding.UTF8.GetString(byteArray);
                obj.Email = decodedEmail;
                bool Status_Check = uv.Check_USER_PASSWORDCHANGE_LINK_Status(decodedEmail);
                if (!Status_Check)
                {
                    return View("Expire_Link");
                }

                bool RespMsg = uv.Update_USER_PASSWORD(obj);
                if (RespMsg)
                {
                    uv.Update_USER_PASSWORDCHANGE_LINK(decodedEmail);
                    return Json(RespMsg);
                }



                return Json(RespMsg);
            }
            catch (Exception er)
            {

                General.Write_Log_Into_Txt(er.Message, "LoginController");
                General.InsertExcepstionlog(er.Message, "LoginController");
                return View("Error");
            }
        }


        //Register Machine Details

        public async Task<IActionResult> RegisterMachineDetails(string useRcode, string DeviceMotionEvent)
        {
            try
            {

                string publicIP = await GetPuclicIP();
                ViewBag.publicIP = publicIP;

                JObject jobj = await GetLocationDetails(publicIP);
                var city = jobj["city"]?.ToString();
                var region = jobj["region"]?.ToString();
                var country = jobj["country"]?.ToString();
                var loc = jobj["loc"]?.ToString();
                var org = jobj["org"]?.ToString();
                var postal = jobj["postal"]?.ToString();
                var timezone = jobj["timezone"]?.ToString();





                //-------------------------------------




                var result = _devicedetailse.GetUserAgentInfo(Request);






                //string biosSerial = CommonFunction.GetWmiIdentifier("Win32_BIOS", "SerialNumber");


                //string motherboardSerial = CommonFunction.GetWmiIdentifier("Win32_BaseBoard", "SerialNumber");


                //string username = CommonFunction.GetWmiIdentifier("Win32_ComputerSystem", "UserName");

                DeviceDetails dd = new DeviceDetails();

                dd.UserCode = useRcode;
                dd.Device_Flag = "1";
                dd.OS = result.OS;
                dd.Browser = result.Browser;
                dd.DuniqueIdentifier = DeviceMotionEvent;
                dd.Location = country + '/' + region + '/' + city;
                dd.Sys_Created_By = useRcode;


                var resp = uv.Insert_Device_Details(dd);

                return Json("Your device details has been successfully registered.You can login now");

            }
            catch (Exception er)
            {

                General.Write_Log_Into_Txt(er.Message, "LoginController");
                General.InsertExcepstionlog(er.Message, "LoginController");
                return View("Error");
            }

        }


        public IActionResult GnerateAuthTokenRegisterDevice(string useRcode)
        {

            var resp = CommonFunction.GenerateAuthTokenRegisterDevice();




            var UserData = userMaster.USERMASTERBYID(useRcode.ToString());

            var Empdata = Employee.GetEmployeeById(UserData.EmpCode.ToString());

            var sendEmail = uv.mailalert_For_Auth_Code(Empdata.Co_Email_ID, UserData.UserName, resp);

            if (sendEmail)
            {
                return Json(resp);
            }

            return Json(null);
        }



        public async Task<string> GetPuclicIP()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Get the public IP address from the ipify service
                    string publicIP = await client.GetStringAsync("https://api.ipify.org");
                    return publicIP;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }


        public async Task<JObject> GetLocationDetails(string publicIP)
        {
            JObject jobj = null;
            string url = $"https://ipinfo.io/{publicIP}/geo";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode(); // Throw if not a success code.

                    string responseBody = await response.Content.ReadAsStringAsync();
                    jobj = JObject.Parse(responseBody);
                    //ViewBag.responseBody = responseBody;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message: {0} ", e.Message);
                }
            }
            return jobj;
        }

    }


}

