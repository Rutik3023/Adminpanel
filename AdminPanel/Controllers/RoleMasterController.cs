
using AdminPanel.CommonRepo;
using  AdminPanel.Models;
using AdminPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{


    [Authorize]
    [AuthorizeUser]
    public class RoleMasterController : Controller
    {
        IRoleMaster Role;
        IOrganization general;
        IGeneral g;
        IUserMaster userMaster;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleMasterController(IRoleMaster _Role, IHttpContextAccessor httpContextAccessor, IOrganization _general, IGeneral _g, IUserMaster _userMaster)
        {
            Role = _Role;
            general = _general;
            g = _g;
            _httpContextAccessor = httpContextAccessor;
            userMaster = _userMaster;
        }
        [HttpGet]
        public IActionResult RoleForm(int Id = 0)
        {
            try
            {

                //ViewBag.Organization = general.GetOrganizationDropDown();
                ViewBag.Organization = userMaster.GetOrganizationDropDownbyorgid();
                ViewBag.Status = CommonDropdown.GetStatusDropdown();

                if (Id > 0)
                {
                    VMRoleMaser Data = Role.GetAllRoleMasterById(Id);

                    return View(Data);
                }

                var user = _httpContextAccessor.HttpContext.User;
                string orgCode = user.FindFirst("OrgCode")?.Value;

                if (string.IsNullOrEmpty(orgCode))
                {
                    return View();
                }

                VMRoleMaser obj = new VMRoleMaser
                {
                    OrgCode = orgCode
                };
                return View(obj);
            }
            catch (Exception ex)
            {
                // Log the exception
                g.Write_Log_Into_Txt(ex.Message, "RoleMasterController");
                g.InsertExcepstionlog(ex.Message, "RoleMasterController");
                return View("Error");
            }
        }


        [HttpPost]
        public IActionResult RoleForm(VMRoleMaser obj)
        {
            try
            {
                if (obj != null)
                {
                    string result = Role.INSERTUPDATE(obj);

                    // Set appropriate TempData based on result
                    if (result.Contains("INSERTED"))
                    {
                        TempData["Output"] = "Record submitted successfully.";
                        return RedirectToAction("RoleList");
                    }
                    else if (result.Contains("Upadated"))
                    {
                        TempData["Output"] = "Record updated successfully";
                        return RedirectToAction("RoleList");
                    }


                }
                return View();
            }
            catch (Exception ex)
            {

                g.Write_Log_Into_Txt(ex.Message, "RoleMasterController");
                g.InsertExcepstionlog(ex.Message, "RoleMasterController");
                return View("Error");
            }
        }

        public IActionResult RoleList()
        {
            try
            {
                ViewBag.RoleList = Role.GetAllRoleMasterByOrgCode();

                return View();
            }
            catch (Exception ex)
            {

                g.Write_Log_Into_Txt(ex.Message, "RoleMasterController");
                g.InsertExcepstionlog(ex.Message, "RoleMasterController");
                return View("Error");
            }
        }

        public IActionResult DeleteRole(int Id)
        {
            try
            {

                bool Resonse = Role.DeleteRoleMasterById(Id);

                // Set appropriate TempData based on result
                if (Resonse)
                {
                    TempData["Output"] = "Deleted successfully";
                }
                else
                {
                    TempData["Output"] = "Error occurred while processing the role.";
                }

                return Json(Resonse);

            }
            catch (Exception ex)
            {

                g.Write_Log_Into_Txt(ex.Message, "RoleMasterController");
                g.InsertExcepstionlog(ex.Message, "RoleMasterController");
                return View("Error");
            }
        }


        public IActionResult GetAllRoleFor_Ajax()
        {
            try
            {
                List<VMRoleMaser> Resonse = Role.GetAllRoleMasterList();

                return Json(Resonse);

            }
            catch (Exception ex)
            {

                g.Write_Log_Into_Txt(ex.Message, "RoleMasterController");
                g.InsertExcepstionlog(ex.Message, "RoleMasterController");
                return View("Error");
            }
        }

    }
}
