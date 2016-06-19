using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WizardsLife.Controllers.Game
{
    [Authorize]
    public class NewsController : Controller
    {
        public ActionResult Index()
        {
            string id = User.Identity.Name;
            return View();
        }
    }
}