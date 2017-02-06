using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Security.Claims;
using Microsoft.Owin.Security;
using CrossDoubleN.ViewModels; // пространство имен LoginViewModel
using CrossDoubleN.Models; // пространство имен моделей
using System.Threading.Tasks;
using System.Data.Entity;

namespace CrossDoubleN.Controllers
{
    public class AccountController : Controller
    {
        UserContext db = new UserContext();
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Name == model.Name && u.Password == model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    ClaimsIdentity claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                    claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String));
                    claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name, ClaimValueTypes.String));
                    claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                        "OWIN Provider", ClaimValueTypes.String));
                    if (user.Role != null)
                        claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name, ClaimValueTypes.String));

                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            try
            {
                AuthenticationManager.SignOut();
            }
            catch
            {

            }
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(Reg model)
        {
            
            if (ModelState.IsValid)
            {
                User user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Name == model.Name);
                if (user == null)
                {
                    UserContext ad = new UserContext();
                    ad.Roles.Load();
                    ad.Users.Load();
                    User abc = new User();
                    abc.Name = model.Name;
                    abc.Password = model.Password;
                    abc.RoleId = 1;
                    ad.Users.Add(abc);
                    ad.SaveChanges();
                    user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Name == model.Name && u.Password == model.Password);

                    if (user == null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ClaimsIdentity claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                        claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String));
                        claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name, ClaimValueTypes.String));
                        claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                            "OWIN Provider", ClaimValueTypes.String));
                        if (user.Role != null)
                            claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name, ClaimValueTypes.String));

                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ViewBag.MyMessageToUsers = "Такой пользователь уже существует!";
            }
            return View(model);
        }

        public ActionResult Manage()
        {
            Log abc = new Log();
            abc.name = User.Identity.Name;
            abc.Role = User.Identity.GetUserRole();
            return View(abc);
        }
	}
}