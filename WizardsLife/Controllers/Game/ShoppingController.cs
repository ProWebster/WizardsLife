using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WizardsLife.Controllers.Game
{
    [Authorize]
    public class ShoppingController : Controller
    {
        public ActionResult DiagonAlley()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoneShopping()
        {
            Lib.Entity.User u = Lib.DatabaseManager.UserManager.Current.Get(Int32.Parse(User.Identity.Name));
            u.Status = Lib.Entity.User.UserStatus.NeedsSorting;
            Lib.DatabaseManager.UserManager.Current.Update(u);

            return Json(new { Success = true, Redirect = Url.Action("Index", "Sorting") });
        }
    }
}