using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebScrach2.Models;

namespace WebScrach2.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {

        amugh786_aziz_dbEntities db = new amugh786_aziz_dbEntities();

        [Authorize]
        [HttpGet]
        public ActionResult ChangePassword(string returnUrl, string UserName)
        {
            var model = new RenamePassword
            {
                UserName = UserName,
                ReturnUrl = returnUrl
            };
            return View(model);
        }


        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(RenamePassword model)
        {
            if (!ModelState.IsValid)
            { return View(model); }

            var user = db.Users.Where(x => x.UserNameOrEmail == model.UserName).FirstOrDefault();
            if (user== null )
            { ModelState.AddModelError(string.Empty, "User Not exist."); }
            else
            {
                user.Password = model.Password;
                db.SaveChanges();

                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }
            return View(model);
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult RegisterAdmin(string returnUrl)
        {
            var model = new RegisterModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult RegisterAdmin(RegisterModel model)
        {
            if (!ModelState.IsValid)
            { return View(model); }

            var userPrevious = db.Users.Where(x => x.UserNameOrEmail == model.Email);
            if (userPrevious.Count() > 0)
            { ModelState.AddModelError(string.Empty, "Username already exist."); }
            else
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserNameOrEmail = model.Email,
                    Password = model.Password,
                    IsActive = true,
                    IsAdmin = true
                };

                db.Users.Add(user);
                db.SaveChanges();

                string role = "customer";
                if (user.IsAdmin.Equals(true))
                    role = "admin";

                var identity = new ClaimsIdentity(new[] { 
                    new Claim(ClaimTypes.Actor, role),
                    new Claim(ClaimTypes.Name, user.UserNameOrEmail),
                    new Claim(ClaimTypes.Email, user.UserNameOrEmail)
                    //, new Claim(ClaimTypes.Country, "Philippines")
                }, "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(identity);
                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }
            return View(model);
        }



        [AllowAnonymous]
        [HttpGet]
        public ActionResult RegisterCustomer(string returnUrl)
        {
            var model = new RegisterModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegisterCustomer(RegisterModel model)
        {
            if (!ModelState.IsValid) 
            { return View(model); }

            var userPrevious = db.Users.Where(x => x.UserNameOrEmail == model.Email);
            if (userPrevious.Count() > 0 )
            { ModelState.AddModelError(string.Empty, "Username already exist."); }
            else 
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserNameOrEmail = model.Email,
                    Password = model.Password,
                    IsActive = true,
                    IsAdmin = false
                };

                db.Users.Add(user);
                db.SaveChanges();

                string role = "customer";
                if (user.IsAdmin.Equals(true))
                    role = "admin";

                var identity = new ClaimsIdentity(new[] { 
                    new Claim(ClaimTypes.Actor, role),
                    new Claim(ClaimTypes.Name, user.UserNameOrEmail),
                    new Claim(ClaimTypes.Email, user.UserNameOrEmail)
                    //, new Claim(ClaimTypes.Country, "Philippines")
                }, "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(identity);
                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }                         
            return View(model);
        }



        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            { return View(model); }

            var user = db.Users.Where( x => x.UserNameOrEmail == model.Email && x.Password == model.Password && x.IsActive == true ).FirstOrDefault();

            if (user != null)
            {
                string role =  "customer";
                if (user.IsAdmin.Equals(true))
                    role = "admin";

                var identity = new ClaimsIdentity(new[] { 
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Name, user.UserNameOrEmail),
                    new Claim(ClaimTypes.Email, user.UserNameOrEmail)
                    //, new Claim(ClaimTypes.Country, "Philippines")
                }, "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(identity);

                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
            }

            //if (model.Email == "admin@admin.com" && model.Password == "123")
            //{ 
            //    var identity = new ClaimsIdentity( new[] { 
            //        new Claim(ClaimTypes.Role, "admin"),
            //        new Claim(ClaimTypes.Name, "Xtian"),
            //        new Claim(ClaimTypes.Email, "xtian@email.com"),
            //        new Claim(ClaimTypes.Country, "Philippines")
            //    }, "ApplicationCookie");
 
            //    var ctx = Request.GetOwinContext();
            //    var authManager = ctx.Authentication;
            //    authManager.SignIn(identity);
 
            //    return Redirect(GetRedirectUrl(model.ReturnUrl));
            //}

            return View(model);
        }


        //[Authorize(Roles = "admin")]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login", "Auth");
        }


        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }
            return returnUrl;
        }

        //// GET: Auth
        //public ActionResult Login()
        //{
        //    return View();
        //}



    }
}