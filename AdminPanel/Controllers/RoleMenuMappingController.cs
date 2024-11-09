
using AdminPanel.CommonRepo;
using  AdminPanel.Models;
using AdminPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{


    [Authorize]
    [AuthorizeUser]
    public class RoleMenuMappingController : Controller
    {
        IRoleMenuMapping rmm;
        private readonly IHttpContextAccessor _httpContextAccessor;
        IGeneral g;
        public RoleMenuMappingController(IRoleMenuMapping _mm, IHttpContextAccessor httpContextAccessor, IGeneral _g)
        {
            rmm = _mm;
            _httpContextAccessor = httpContextAccessor;
            g = _g;
        }


        public IActionResult RMM_Form(string roleid = null)
        {

            try
            {
                var user = _httpContextAccessor.HttpContext.User;
                var orgCode = user.FindFirst("OrgCode")?.Value;


                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }

                ViewBag.Roles = rmm.Get_RoleMaster_By_OrgCode(orgCode);

                //USP_ADM_GET_RMMAPPING_BYROLEID

                if (roleid != null)
                {
                    VMGet_Menu_ByRoleID d = new VMGet_Menu_ByRoleID();
                    d.Code = roleid;


                    //USP_ADM_GET_ROLEMASTER_BYORGID


                    ViewBag.GetAllMenu = rmm.Get_Menu_ByRoleID(roleid);

                    return View(d);
                }




                return View();
            }
            catch (Exception ex)
            {
                g.Write_Log_Into_Txt(ex.Message, "RoleMenuMappingController");
                g.InsertExcepstionlog(ex.Message, "RoleMenuMappingController");
                return View("Error");
            }
        }






        [HttpPost]
        public IActionResult RMM_Form([FromBody] VMRoleMenuMapping obj)
        {


            try
            {

                var resp = rmm.Insert_RMM(obj);
                TempData["Output"] = "Inserted Successfully";
                return Ok("Inserted");
            }
            catch (Exception ex)
            {

                g.Write_Log_Into_Txt(ex.Message, "RoleMenuMappingController");
                g.InsertExcepstionlog(ex.Message, "RoleMenuMappingController");
                return View("Error");
            }
        }



    }
}
