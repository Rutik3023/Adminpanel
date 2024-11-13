using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult form()
        {
            return View();
        }


        public IActionResult form( Patientmaster data)
        {
            return View();
        }


        public IActionResult List()
        {
            return View();
        }
    }
}
