using MedinetWeb.Helpers;
using MedinetWeb.Models;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;

namespace MedinetWeb.Controllers
{
    public class AccesoController : Controller
    {
        public MedinetWebMembershipProvider MembershipService { get; set; }
        public MedinetWebRoleProvider AuthorizationService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null)
                MembershipService = new MedinetWebMembershipProvider();
            if (AuthorizationService == null)
                AuthorizationService = new MedinetWebRoleProvider();

            base.Initialize(requestContext);
        }

        //
        // GET: /Acceso/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Acceso/Login
        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {

                        string[] roles = AuthorizationService.GetRolesForUser(model.UserName);
                        if (roles.Contains("Visitador Web"))
                        {
                            return RedirectToAction("Index", "Coberturas");
                        }
                        else {
                            return RedirectToAction("Index", "Home");
                        }
                       
                    }
                }
                else
                {
                    ModelState.AddModelError("", "El nombre de usuario o contraseña es incorrecta.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Acceso/LogOff
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Acceso");
        }

        //
        // GET: /Acceso/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Acceso/ChangePassword
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                    return RedirectToAction("ChangePasswordSuccess");
                else
                    ModelState.AddModelError("", "La actual contraseña es incorrecta o la nueva contraseña es invalida.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Acceso/ChangePasswordSuccess
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

    }
}
