//=========================================================================
//  Controller:    OrganizationController
//  Description:   Manages the Organization operations.
//  Created by:    [Moin Mulla]
//  Created on:    [22-05-2024]
//=========================================================================




using AdminPanel.CommonRepo;
using  AdminPanel.Models;
using AdminPanel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace AdminPanel.Controllers
{
    [Area("Organization")]
    [Authorize]
    [AuthorizeUser]
    public class OrganizationController : Controller
    {

        //private IWebHostEnvironment hostingEn;
        private readonly IHttpContextAccessor _httpContextAccessor;

        IOrganization general;

        //ICountry country;

        //ICItyMaster city;

        //IstateMaster state;

        IGeneral g;
        IUserMaster userMaster;

       


        public OrganizationController(IOrganization _general, IWebHostEnvironment env,IGeneral _g,
            IUserMaster _userMaster, IHttpContextAccessor httpContextAccessor)
        {
            general = _general;
            //this.hostingEnv = env;
            //country = c;
            //city = ci;
            //state = s;
            g = _g;
            userMaster = _userMaster;
            _httpContextAccessor = httpContextAccessor;
             
        }
        public IActionResult Index(string Code)
        {
            try
            {

                //ViewBag.country = country.Get_CountryList(2);
                //ViewBag.city = city.GetCityDropDown(2);
                //ViewBag.State = city.GETstateDropdown(2);
                ViewBag.Status = CommonDropdown.GetStatusDropdown();
               // ViewBag.Currency = booking.GETALL_CURRENCY();

                if (Code != null && Code != "")
                {
                    Organization_model Data = general.GetOrganizationById(Code);
                   
                    return View(Data);
                }
              

                return View();
            }
            catch (Exception er)
            {
                g.Write_Log_Into_Txt(er.Message, "OrganizationController");
                g.InsertExcepstionlog(er.Message, "OrganizationController");
                return Ok("Error");

            }
            
        }

        [HttpPost]
        public IActionResult Index(Organization_model obj)
        {
            try
            {
                ResponceMessage RespMsg = general.InsertUpadte_Oraganiz(obj);

                if (RespMsg.code==0)
                {
                    TempData["Output"] = "Record submitted successfully.";
                }
                else if (RespMsg.code == 1)
                {
                    TempData["Output"] = "Record updated successfully";
                }
               
                if (RespMsg.code == -1)
                {
                    return View();
                }

                return RedirectToAction("OrganizationList"); // Pass the model back to the view if necessary


                //if (Logo != null && Logo.Length > 0)
                //{
                //    // Read the file into a byte array
                //    byte[] logoBytes;
                //    using (var memoryStream = new MemoryStream())
                //    {
                //        Logo.CopyTo(memoryStream);
                //        logoBytes = memoryStream.ToArray();
                //    }

                //    // Assuming Organization has a property to hold the byte array of the logo
                //    obj.Logo = logoBytes;
                //}

                //var d = CommonRepo.CommonFunction.GetCheckBoxValues(obj);

            }
            catch (Exception er)
            {

                g.Write_Log_Into_Txt(er.Message, "OrganizationController");
                g.InsertExcepstionlog(er.Message, "OrganizationController");
                return Ok("Error");
            }
            
        }


        
        public IActionResult OrganizationList()
        {
            try
            {
                ViewBag.OrgData = general.GetOrganizationList(1);
                return View();
            }
            catch (Exception er)
            {

                g.Write_Log_Into_Txt(er.Message, "OrganizationController");
                g.InsertExcepstionlog(er.Message, "OrganizationController");
                return Ok("Error");
            }
            
        }

        public IActionResult GetOrganizationList()
        {
            try
            {
                List<Organization_model> obj = general.GetOrganizationList(1);
                if (obj != null)
                {
                    return Json(obj);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception er)
            {

                g.Write_Log_Into_Txt(er.Message, "OrganizationController");
                g.InsertExcepstionlog(er.Message, "OrganizationController");
                return Ok("Error");
            }
            
        }


        public IActionResult Delete(string Code)
        {
            try
            {
                bool RespMsg = general.Delete_Organization(Code);
                if (RespMsg)
                {
                    TempData["Output"] = "Deleted successfully";
                }
                return Json(RespMsg);
            }
            catch (Exception er)
            {

                g.Write_Log_Into_Txt(er.Message, "OrganizationController");
                g.InsertExcepstionlog(er.Message, "OrganizationController");
                return Ok("Error");
            }
            
        }


        //logo 
        [HttpGet]
        public IActionResult logo(string Id, Organization_model O)
         {
            ViewBag.ORG = userMaster.GetOrganizationDropDownbyorgid();
            //ViewBag.ORG = general.GetOrganizationList(2);
            //ViewBag.ORG = userMaster.GetOrganizationDropDownbyorgid();
            var user = _httpContextAccessor.HttpContext.User;

            string orgCode = user.FindFirst("OrgCode")?.Value;
           // Organization_model d = new Organization_model();
            if (orgCode != "" || orgCode != null)
            {
             Organization_model data = general.GetOrgLogoById(Id,O);
                
                ViewBag.LogoInfo = data;
                 return View(data);
            }
            O.Organization_Code = Id;
      
            return View(O);
            

        }

        [HttpPost]
        //public IActionResult logo(IFormFile Logo,string OrgCode)
        //{
        //    try
        //    {

        //        //ViewBag.ORG = userMaster.GetOrganizationDropDownbyorgid();
        //        ViewBag.ORG = general.GetOrganizationList(2);
        //        if (Logo != null && Logo.Length > 0)
        //        {
        //            // Read the file into a byte array

        //            byte[] logoBytes;
        //            using (var memoryStream = new MemoryStream())
        //            {
        //                Logo.CopyTo(memoryStream);

        //                logoBytes = memoryStream.ToArray();


        //            }
        //            Organization_model OBJ = new Organization_model();
        //            // Assuming Organization has a property to hold the byte array of the logo
        //             OBJ.Logo = logoBytes;


        //            ResponceMessage RespMsg = general.InsertUpadte_OrgLogo(OBJ, OrgCode);

        //            if (RespMsg.code!=-1)
        //            {
        //                TempData["showPopup"] = true;
        //            }

        //        }

        //        return RedirectToAction("LogoList");
        //    }
        //    catch (Exception er)
        //    {
        //        g.Write_Log_Into_Txt(er.Message, "OrganizationController");
        //        g.InsertExcepstionlog(er.Message, "OrganizationController");
        //        return Ok("Error");

        //    }


        //}
        public IActionResult logo(IFormFile Logo, string OrgCode, string existingLogoBase64, string existingMimeType, Organization_model obj )
        {
            try
            {
                // Get organization list for dropdown
                //ViewBag.ORG = general.GetOrganizationList(2);
                ViewBag.ORG = userMaster.GetOrganizationDropDownbyorgid();

                //Organization_model OBJ = new Organization_model();

                // Check if a new logo file has been uploaded
                if (Logo != null && Logo.Length > 0)
                {
                    // Read the new file into a byte array
                    using (var memoryStream = new MemoryStream())
                    {
                        Logo.CopyTo(memoryStream);
                        byte[] logoBytes = memoryStream.ToArray();
                        obj.Logo = logoBytes; // Set the new logo
                    }
                }
                else
                {
                    // If no new logo is uploaded, retain the existing logo
                    if (!string.IsNullOrEmpty(existingLogoBase64))
                    {
                        // Convert the existing logo base64 string back to byte array
                        obj.Logo = Convert.FromBase64String(existingLogoBase64);
                    }
                }

                // Insert or update the organization logo
                ResponceMessage RespMsg = general.InsertUpadte_OrgLogo(obj, OrgCode);

                // Show popup message if the update is successful
                if (RespMsg.code != -1)
                {
                    TempData["showPopup"] = true;
                }
               // return View();
                return RedirectToAction("LogoList");
            }
            catch (Exception er)
            {
                g.Write_Log_Into_Txt(er.Message, "OrganizationController");
                g.InsertExcepstionlog(er.Message, "OrganizationController");
                return Ok("Error");
            }
        }

        public IActionResult LogoList()
        {
            try
            {
                ViewBag.OrgData = general.GetOrgLogoList();
                    return View();
            }
            catch (Exception er)
            {
                g.Write_Log_Into_Txt(er.Message, "OrganizationController");
                g.InsertExcepstionlog(er.Message, "OrganizationController");
                return Ok("Error");
            }
        }


        public IActionResult Orglicence()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Orglicence(Org_Saas_License obj)
        {

            try
            {
                if (obj.AuthKey == null || obj.AuthKey == "")
                {

                    ViewBag.AllOrg = general.GetOrganizationListFor_License();

                    ViewBag.GetSaasOrgAll = g.Get_All_Org_Saas();


                    ViewBag.AuthKeyisEmpty = "Please generate Authkey first and then submit";
                    return View();
                }


                var resp = g.InsertUpdate_Organization_License(obj);

                ViewBag.AllOrg = general.GetOrganizationListFor_License();

                ViewBag.GetSaasOrgAll = g.Get_All_Org_Saas();

            }
            catch (Exception ex)
            {
                g.Write_Log_Into_Txt(ex.Message, "DynamicSidebarController");
                g.InsertExcepstionlog(ex.Message, "DynamicSidebarController");
                return View("Error");

            }



            return View();
        }

        [HttpGet]
        public IActionResult DeleteDocument(string SysCode)
        {
            try
            {

                bool RespMsg = general.Delete_Doc_Logo(SysCode);
                if (RespMsg)
                {
                    TempData["Output"] = "Delete Successfully.";
                }
                return Json(RespMsg);
            }
            catch (Exception ex)
            {
                g.Write_Log_Into_Txt(ex.Message, "OrganizationController");
                g.InsertExcepstionlog(ex.Message, "OrganizationController");
                return View("Error");

            }
        }
    }
}
