
using AdminPanel.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{

    public class ExceptionLogController : Controller
    {

        IExceptionLog E;

        public ExceptionLogController(IExceptionLog _E)
        {
            E= _E;

        }
        public IActionResult ExceptionLogList()
        {

          ViewBag.ExceptionLogList = E.GETALLMENU();
            return View();
        }

      
    }
}
