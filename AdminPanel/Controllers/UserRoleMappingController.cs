
using AdminPanel.CommonRepo;
using  AdminPanel.Models;
using AdminPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{



    [Authorize]
    [AuthorizeUser]
    public class UserRoleMappingController : Controller
    {
        IRoleMaster RM;
        IUserMaster UM;
        IUserRoleMapping URM;
        private readonly IHttpContextAccessor _httpContextAccessor;
        IGeneral g;
        public UserRoleMappingController(IRoleMaster RM, IUserMaster uM, IUserRoleMapping _urm, IHttpContextAccessor HTT, IGeneral _g)
        {
            this.RM = RM;
            UM = uM;
            URM = _urm;
            _httpContextAccessor = HTT;
            g = _g;
        }
        public IActionResult URM_Form(string userid = null)
        {

            try
            {
                var user = _httpContextAccessor.HttpContext.User;
                var orgCode = user.FindFirst("OrgCode")?.Value;


                if (orgCode == "" || orgCode == null)
                {
                    orgCode = null;
                }



                ViewBag.GetAllUserByid = URM.Get_User_By_OrgCode(orgCode);




                if (userid != null)
                {
                    VMUrm_By_Id d = new VMUrm_By_Id();
                    d.UserID = userid;

                    ViewBag.GetAllRole = URM.GetURM_ById(userid);

                    return View(d);
                }


                return View();
            }
            catch (Exception ex)
            {

                g.Write_Log_Into_Txt(ex.Message, "UserRoleMappingController");
                g.InsertExcepstionlog(ex.Message, "UserRoleMappingController");
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult URM_Form([FromBody] VMUserRoleMapping obj)
        {

            try
            {
                var resp = URM.Insert_URM(obj);
                return Ok("Inserted Successfully");
            }
            catch (Exception ex)
            {

                g.Write_Log_Into_Txt(ex.Message, "UserRoleMappingController");
                g.InsertExcepstionlog(ex.Message, "UserRoleMappingController");
                return View("Error");
            }
        }



    }
}
