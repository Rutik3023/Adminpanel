
using  AdminPanel.Models;
using AdminPanel.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace AdminPanel.Controllers
{
 
    public class DynamicSidebarController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IOrganization _O;

        private readonly IHttpContextAccessor _httpContextAccessor;
        IWebHostEnvironment _env;
        IGeneral _g;
        

        public DynamicSidebarController(IMenuService menuService, IWebHostEnvironment e, IOrganization O, IHttpContextAccessor httpContextAccessor, IGeneral g, IGeneral _general)
        {
            _menuService = menuService;
            _O = O;
            _httpContextAccessor = httpContextAccessor;
            _env = e;
            
            _g = g;
        }

        [HttpGet]
        //[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)] // Cache for 5 minutes
        public IActionResult Index()
        {
            try
            {
                var flatMenuItems = _menuService.GetUserMenus();
                var hierarchicalMenu = _menuService.BuildMenuHierarchy(flatMenuItems);

                return Json(hierarchicalMenu);

            }
            catch (Exception ex)
            {


                _g.Write_Log_Into_Txt(ex.Message, "DynamicSidebarController");
                _g.InsertExcepstionlog(ex.Message, "DynamicSidebarController");



                return View("Error");

            }
        }


        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)] // Cache for 5 minutes
        public IActionResult ORGLOGO()
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User;

                var orgCode = user.FindFirst("OrgCode")?.Value;
                if (orgCode != null && orgCode != "")
                {
                    var organization = _O.GetOrganizationById(orgCode);
                    if (organization != null && organization.Logo != null)
                    {
                        var base64Logo = Convert.ToBase64String(organization.Logo);
                        return Json(new { logo = base64Logo });
                    }
                }
                if (orgCode == null || orgCode == "")
                {
                    // Path to the image in wwwroot folder
                    var imagePath = Path.Combine(_env.WebRootPath, "img", "Moksha.png");

                    if (System.IO.File.Exists(imagePath))
                    {
                        var imageBytes = System.IO.File.ReadAllBytes(imagePath);
                        var base64Logo = Convert.ToBase64String(imageBytes);
                        return Json(new { logo = base64Logo });
                    }
                }

                return Json(new { logo = (string)null });
            }
            catch (Exception ex)
            {


                _g.Write_Log_Into_Txt(ex.Message, "DynamicSidebarController");
                _g.InsertExcepstionlog(ex.Message, "DynamicSidebarController");
                return View("Error");
            }
        }
        public IActionResult DemoSidebar(string sysCode = "")
        {

            try
            {
                ViewBag.AllOrg = _O.GetOrganizationListFor_License();

                ViewBag.GetSaasOrgAll = _g.Get_All_Org_Saas();


                if (sysCode != "")
                {

                    var resp = _g.Get_Org_Saas_by_syscode(sysCode);


                    return View(resp);
                }
                return View();
            }
            catch (Exception ex)
            {

                _g.Write_Log_Into_Txt(ex.Message, "DynamicSidebarController");
                _g.InsertExcepstionlog(ex.Message, "DynamicSidebarController");
                return View("Error");
            }



        }

        [HttpPost]
        public IActionResult DemoSidebar(Org_Saas_License obj)
        {
            try
            {
                if (obj.AuthKey == null || obj.AuthKey == "")
                {

                    ViewBag.AllOrg = _O.GetOrganizationListFor_License();

                    ViewBag.GetSaasOrgAll = _g.Get_All_Org_Saas();


                    ViewBag.AuthKeyisEmpty = "Please generate Authkey first and then submit";
                    return View();
                }


                var resp = _g.InsertUpdate_Organization_License(obj);
                if (resp.Contains("Inserted"))
                {
                    TempData["Output"] = "Record submitted successfully.";
                }

                ViewBag.AllOrg = _O.GetOrganizationListFor_License();

                ViewBag.GetSaasOrgAll = _g.Get_All_Org_Saas();

            }
            catch (Exception ex)
            {
                _g.Write_Log_Into_Txt(ex.Message, "DynamicSidebarController");
                _g.InsertExcepstionlog(ex.Message, "DynamicSidebarController");
                return View("Error");

            }



            return View();
        }


        public IActionResult DeleteOrganizationLicense(string sysCode)
        {
            try
            {
                var resp = _g.Delete_Organization_License(sysCode);
                return Json(resp);
            }
            catch (Exception ex)
            {

                _g.Write_Log_Into_Txt(ex.Message, "DynamicSidebarController");
                _g.InsertExcepstionlog(ex.Message, "DynamicSidebarController");
                return View("Error");
            }
        }








        [HttpPost]
        public IActionResult generateKey([FromBody] Org_Saas_License obj)
        {
            try
            {
                // Example dates (replace with your actual dates from database)
                DateTime validFrom = Convert.ToDateTime(obj.Valid_From); // Replace with actual Valid_From date
                DateTime validTill = Convert.ToDateTime(obj.Valid_Till); // Replace with actual Valid_Till date

                // Format dates as needed
                string formattedValidFrom = validFrom.ToString("dd-MM-yyyy");
                string formattedValidTill = validTill.ToString("dd-MM-yyyy");

                // Construct the string to encode (example format: "ValidFrom|ValidTill")
                string stringToEncode = $"{formattedValidFrom}|{formattedValidTill}";

                // Encode the string to Base64
                string base64EncodedString = Base64Encode(stringToEncode);


                return Json(base64EncodedString);
            }
            catch (Exception ex)
            {
                _g.Write_Log_Into_Txt(ex.Message, "DynamicSidebarController");
                _g.InsertExcepstionlog(ex.Message, "DynamicSidebarController");
                return View("Error");

            }
        }


        public static string Base64Encode(string plainText)
        {
            try
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception ex)
            {
                throw;

            }
        }
    }
}
