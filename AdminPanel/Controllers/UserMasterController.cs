
using AdminPanel.CommonRepo;
using  AdminPanel.Models;
using AdminPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace AdminPanel.Controllers
{
    [Authorize]
    [AuthorizeUser]
    public class UserMasterController : Controller
    {
        #region inject The Dependancy

        IEmployee EmpMaster;
        IUserMaster userMaster;
        IGeneral g;
        private readonly IHttpContextAccessor _httpContextAccessor;

       // ICommon common;


        public UserMasterController(IUserMaster _userMaster, IHttpContextAccessor httpContextAccessor, IEmployee _EmpMaster, IGeneral _g) //ICommon _common)
        {
            userMaster = _userMaster;
            EmpMaster = _EmpMaster;
            g = _g;
            _httpContextAccessor = httpContextAccessor;
           // common = _common;

        }
        #endregion


        public IActionResult Index(string id = null)
        {
            #region cerate view and bind dropdown
            try
            {


                VMUserMaser obj = new VMUserMaser();


                //ViewBag.Emp = EmpMaster.GetEmployeeList();

                ViewBag.sq = userMaster.DropDownSQ();
                ViewBag.Emp = userMaster.GetEmpCodeEmpName();
                ViewBag.Status = CommonDropdown.GetStatusDropdown();

                ViewBag.ORG = userMaster.GetOrganizationDropDownbyorgid();
                if (id != null)
                {

                    VMUserMaser Data = userMaster.USERMASTERBYID(id);

                    return View(Data);
                }
                var user = _httpContextAccessor.HttpContext.User;

                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (orgCode == "" || orgCode == null)
                {
                    obj.MultipleDevice = 3;
                    obj.LoginAttempts = 3;
                    return View(obj);
                }
                obj.OrgCode = orgCode;
                obj.MultipleDevice = 1;
                obj.LoginAttempts = 3;
                return View(obj);


            }
            catch (Exception ex)
            {

                g.Write_Log_Into_Txt(ex.Message, "UserMasterController");
                g.InsertExcepstionlog(ex.Message, "UserMasterController");
                return View("Error");
            }
            #endregion
        }

        [HttpPost]
        public IActionResult Index(VMUserMaser Data, IFormFile Logo)
        {
            #region Insert and update the user master in the database
            try
            {

                ViewBag.sq = userMaster.DropDownSQ();
                ViewBag.Emp = userMaster.GetEmpCodeEmpName();
                ViewBag.Status = CommonDropdown.GetStatusDropdown();

                ViewBag.ORG = userMaster.GetOrganizationDropDownbyorgid();
                //    // Validate the uploaded file (if any)
                if (Logo != null)
                {
                    // Ensure the file is an image and is less than 3MB
                    var allowedFileTypes = new[] { "image/jpeg", "image/png" };
                    var maxFileSizeInMB = 3;
                    if (!allowedFileTypes.Contains(Logo.ContentType) || Logo.Length > maxFileSizeInMB * 1024 * 1024)
                    {
                        TempData["ShowPopup"] = true;
                        //ModelState.AddModelError("Logo", "Please upload a valid image file (JPG/PNG) less than 3MB.");
                        return View(Data); // Return the view with the current data and validation errors
                    }
                }

                // Insert or update the user master in the database
                ResponceMessage obj = userMaster.InserUpdate(Data, Logo);

                if (obj.code == 0)
                {

                    //DataTable UserData = userMaster.GetUsermasterDataTomail(obj.Sys_Code);
                    //List<TempletConfigration> Config = userMaster.GETEmailCONFIGRATION();
                    //List<Cangeneration> UserDdata = CommonRepo.CommonFunction.GetDataCANGENRATIONFormat(Config, UserData);


                    ////string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Mailtemplate");
                    ////string emailBodyFilePath = Path.Combine(rootPath, "UserCreate.html");

                    ////string body = System.IO.File.ReadAllText(emailBodyFilePath);


                    ////string Emailbody = common.ReplacePlaceholders(UserDdata, body, "");

                    //string SenderEmailid = UserData.Rows[0]["Co_Email_ID"].ToString();
                    //if (SenderEmailid != null || SenderEmailid != "")
                    //{
                    //    common.mailalertUserCreation(UserDdata, SenderEmailid);
                    //}



                }

                if (obj.code == -1)
                {
                    return View(Data); // Return the view with the current data and validation errors
                }

                // Set appropriate TempData based on result
                if (obj.Message.Contains("INSERTED"))
                {
                    TempData["Output"] = "Record submitted successfully.";
                    return RedirectToAction("UserList");
                }
                else if (obj.Message.Contains("UPDATED"))
                {
                    TempData["Output"] = "Record updated successfully.";
                    return RedirectToAction("UserList");
                }
                else if (obj.Message.Contains("Dublicate"))
                {
                    TempData["Output"] = "Record already Exists.";
                    return RedirectToAction("UserList");
                }

                return View();

            }
            catch (Exception ex)
            {
                // Log the exception
                g.Write_Log_Into_Txt(ex.Message, "UserMasterController");
                g.InsertExcepstionlog(ex.Message, "UserMasterController");

                // Return the error view
                return View("Error");
            }
            #endregion
        }



        public IActionResult UserList()
        {
            #region Get all User Master List And bind in Grid
            try
            {
                ViewBag.userlist = userMaster.GETLISTUSERMASTER();

                return View();

            }
            catch (Exception ex)
            {

                g.Write_Log_Into_Txt(ex.Message, "UserMasterController");
                g.InsertExcepstionlog(ex.Message, "UserMasterController");
                return View("Error");
            }
            #endregion
        }



        [HttpGet]
        public IActionResult Delete(string id)
        {

            #region Delete The User Master in Db and update the status in D in db 
            try
            {
                ResponceMessage data = userMaster.DELETEUSERMASTER(id);
                // Set appropriate TempData based on result
                if (data.Message.Contains("UPDATED"))
                {
                    TempData["Output"] = "Deleted successfully";
                    return Json(data);
                }
                return Json(null);

            }
            catch (Exception ex)
            {

                g.Write_Log_Into_Txt(ex.Message, "UserMasterController");
                g.InsertExcepstionlog(ex.Message, "UserMasterController");
                return View("Error");
            }
            #endregion
        }

    }
}
