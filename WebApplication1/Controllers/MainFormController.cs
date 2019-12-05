using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class MainFormController : Controller
    {
        // GET: MainForm
        public ActionResult MainIndex()
        {
            if (Request.QueryString["id"] != null)
            {
                string value = Request.QueryString["id"].ToString();

                ViewBag.UserTicket = value;

                return View();
            }
            else
            {
                return RedirectToAction("myindex", "Home");
            }
            
        }
    }
}