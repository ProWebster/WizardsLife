using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WizardsLife.Models.CreateCharacter;

namespace WizardsLife.Controllers.Game
{
    [Authorize]
    public class CreateCharacterController : Controller
    {
        // GET: CreateCharacter
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create(CreateCharacter viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Name) || string.IsNullOrWhiteSpace(viewModel.FamilyName))
                return Json(new { Success = false, Content = "All fields are required" });

            if (viewModel.Name.Length < 4 || viewModel.Name.Length > 40)
                return Json(new { Success = false, Content = "The characters name must be between 4 and 40 characters long." });

            if (viewModel.FamilyName.Length < 4 || viewModel.FamilyName.Length > 40)
                return Json(new { Success = false, Content = "The characters family name must be between 4 and 40 characters long." });

            Lib.Entity.User user = Lib.DatabaseManager.UserManager.Current.Get(Int32.Parse(User.Identity.Name));
            user.CharName = viewModel.Name;
            user.CharFamilyName = viewModel.FamilyName;
            user.CharGender = viewModel.Gender;
            user.CharBloodStatus = viewModel.BloodStatus;
            user.Status = Lib.Entity.User.UserStatus.NeedsShopping;

            Lib.DatabaseManager.UserManager.Current.Update(user);
            
            return Json(new { Success = true, Redirect = Url.Action("DiagonAlley", "Shopping") });
        }
    }
}