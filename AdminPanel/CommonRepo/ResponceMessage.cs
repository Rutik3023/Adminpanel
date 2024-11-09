using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdminPanel.CommonRepo
{
    public class ResponceMessage
    {

        public string  Sys_Code { get; set; }

        public int code { get; set; }

        public dynamic Message { get; set; }


     public  List<SelectListItem> Data { get; set; }

       public ResponceMessage ()
        {

            Data = new List<SelectListItem> ();
        }

    }
}
