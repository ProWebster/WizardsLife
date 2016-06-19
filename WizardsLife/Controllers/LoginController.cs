using Lib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WizardsLife.Models.Modals;

namespace WizardsLife.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login(SignUpOrLogIn_VM viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.LoginUsername) || string.IsNullOrWhiteSpace(viewModel.LoginPassword))
                return Json(new { Success = false, Content = "All fields are required" });

            User user = Lib.DatabaseManager.UserManager.Current.GetFromUsername(viewModel.LoginUsername);
            if (user != null)
            {
                string hashedPassword = sha256_hash(viewModel.LoginPassword);
                if (user.Password.Equals(hashedPassword))
                {
                    // Logged in successfully
                    FormsAuthentication.SetAuthCookie("" + user.Id, true);

                    if (user.Status == Lib.Entity.User.UserStatus.NeedsCharacter)
                        return Json(new { Success = true, Redirect = Url.Action("Index", "CreateCharacter") });
                    else if (user.Status == Lib.Entity.User.UserStatus.NeedsShopping)
                        return Json(new { Success = true, Redirect = Url.Action("DiagonAlley", "Shopping") });
                    else if (user.Status == Lib.Entity.User.UserStatus.NeedsSorting)
                        return Json(new { Success = true, Redirect = Url.Action("Index", "Sorting") });
                    else
                        return Json(new { Success = true, Redirect = Url.Action("Index", "News") });
                }
                else
                    return Json(new { Success = false, Content = "The password don't match" });
            }
            else
                return Json(new { Success = false, Content = "User does not exist" });
        }

        public ActionResult SignUp(SignUpOrLogIn_VM viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.SignUpEmail) || string.IsNullOrWhiteSpace(viewModel.SignUpUsername) || string.IsNullOrWhiteSpace(viewModel.SignUpPassword) || string.IsNullOrWhiteSpace(viewModel.SignUpRepeatPassword))
                return Json(new { Success = false, Content = "All fields are required" });

            if (viewModel.SignUpPassword.Length < 6)
                return Json(new { Success = false, Content = "Your password must be at least 6 characters" });

            if (!viewModel.SignUpPassword.Equals(viewModel.SignUpRepeatPassword))
                return Json(new { Success = false, Content = "The 2 password fields does not match" });

            // Check for existing
            User existing = Lib.DatabaseManager.UserManager.Current.GetFromUsername(viewModel.SignUpUsername);
            if (existing != null)
                return Json(new { Success = false, Content = "A user with that exact username already exists" });
            
            // Create user
            User u = new User();
            u.Username = viewModel.SignUpUsername;
            u.Password = sha256_hash(viewModel.SignUpPassword);
            u.Email = viewModel.SignUpEmail;
            int id = Lib.DatabaseManager.UserManager.Current.Create(u);

            if (id > 0)
            {
                FormsAuthentication.SetAuthCookie("" + id, true);
                return Json(new { Success = true, Redirect = Url.Action("Index", "CreateCharacter") });
            }
            else
                return Json(new { Success = false, Content = "An unknown error occurred! Please try again." });
        }


        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}