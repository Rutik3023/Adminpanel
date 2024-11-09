
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;
using System.Net.Http.Headers;
using AdminPanel.Services;
using AdminPanel.CommonRepo;
using  AdminPanel.Models;
using Microsoft.AspNetCore.Authorization;


namespace AdminPanel.Controllers
{



    [Authorize]
    [AuthorizeUser]
    public class MenuMasterController : Controller
    {
        IMenuMaster M;
        private readonly IWebHostEnvironment _hostingEnv;
        IGeneral general;

        public MenuMasterController(IMenuMaster _M, IWebHostEnvironment env, IGeneral general)
        {
            M = _M;
            _hostingEnv = env;
            this.general = general;
        }

        [HttpGet]
        public IActionResult MenuForm(int Code = 0)
        {
            try
            {
                ViewBag.ParentMenu = M.GETALLMENU();
                ViewBag.o = M.GETALLORG();
                ViewBag.Status = CommonDropdown.GetStatusDropdown();
                if (Code > 0)
                {
                    MenuMaster res = M.GETMENUBYID(Code);
                    //ViewBag.o = M.GETALLORG();
                    ViewBag.o = res.Orglist;
                    return View(res);
                }
                return View();

            }
            catch (Exception ex)
            {

                general.Write_Log_Into_Txt(ex.Message, "MenuMasterController");
                general.InsertExcepstionlog(ex.Message, "MenuMasterController");
                return View("Error");
            }


        }
        [HttpPost]

        public IActionResult MenuForm(MenuMaster obj, IFormFile MenuIcon)
        {


            try
            {
                if (MenuIcon != null && MenuIcon.Length > 0)
                {
                    var filename = ContentDispositionHeaderValue.Parse(MenuIcon.ContentDisposition).FileName.Trim('"');
                    var uploadsFolder = Path.Combine(_hostingEnv.WebRootPath, "MenuIcon");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var filePath = Path.Combine(uploadsFolder, filename);

                    // Save the file to the specified directory
                    if (!System.IO.File.Exists(filePath))
                    {
                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            MenuIcon.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    else
                    {
                        Response.Clear();
                        Response.StatusCode = 409; // Conflict
                        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File already exists.";
                        return new EmptyResult();
                    }

                    //// Convert the file to a base64 string
                    //string base64Logo;
                    //using (var memoryStream = new MemoryStream())
                    //{
                    //    Logo.CopyTo(memoryStream);
                    //    byte[] fileBytes = memoryStream.ToArray();
                    //    base64Logo = Convert.ToBase64String(fileBytes);
                    //}

                    // Assuming Organization has a property to hold the base64 string of the logo
                    obj.MenuIcon = filePath;
                }


                if (obj != null)
                {
                    List<string> orgCodeList = new List<string>();

                    if (obj.Org_Code != null)
                    {
                        obj.OrgCode = obj.Org_Code.Split(",");
                        orgCodeList = CommonFunction.ConvertArrayToList(obj.OrgCode);
                    }


                    string result = M.INSERTUPDATE(obj, orgCodeList);

                    if (result.Contains("INSERTED"))
                    {
                        TempData["Output"] = "Record submitted successfully.";

                    }
                    else if (result.Contains("UPDATED"))
                    {
                        TempData["Output"] = "Record updated successfully";

                    }

                    ModelState.Clear();


                }
                return View();
            }
            catch (Exception ex)
            {
                general.Write_Log_Into_Txt(ex.Message, "MenuMasterController");
                general.InsertExcepstionlog(ex.Message, "MenuMasterController");
                return View("Error");

            }


        }
        [HttpGet]
        public IActionResult MenuList()
        {
            try
            {
                ViewBag.MenuList = M.GETALLMENU();

                return View();
            }
            catch (Exception ex)
            {

                general.Write_Log_Into_Txt(ex.Message, "MenuMasterController");
                general.InsertExcepstionlog(ex.Message, "MenuMasterController");
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult MenuDelete(int Code = 0)
        {
            try
            {
                if (Code > 0)
                {

                    bool o = M.DELETEMENU(Code);
                    if (o)
                    {
                        TempData["Output"] = "Deleted successfully";
                    }

                }
                return RedirectToAction("MenuList");
            }
            catch (Exception ex)
            {

                general.Write_Log_Into_Txt(ex.Message, "MenuMasterController");
                general.InsertExcepstionlog(ex.Message, "MenuMasterController");
                return View("Error");
            }
        }
        public class YourViewModel
        {
            public string IconClass { get; set; }
        }
        [HttpGet]
        public IActionResult SaveIcon()
        {
            return View();


        }
        [HttpPost]
        public IActionResult SaveIcon(YourViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Save the icon class to the database
                    //string selectedIconClass = model.IconClass;
                    // Your code to save to the database

                    return RedirectToAction("Index");
                }

                return View(model);

            }
            catch (Exception ex)
            {
                general.Write_Log_Into_Txt(ex.Message, "MenuMasterController");
                general.InsertExcepstionlog(ex.Message, "MenuMasterController");
                return View("Error");

            }
        }

    }
}
