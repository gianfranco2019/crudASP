using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEBASPLOGIN.Models;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace WEBASPLOGIN.Controllers
{
    public class LoginController : Controller
    {

        private OurDbContext _context;
        public LoginController (OurDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {



            return View(_context.userAccount.ToList());
        }

        public IActionResult Error()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAccount user)
        {

           

            string reason = String.Empty;

            

                var verifyEmail = _context.userAccount.Where(u => u.Email == user.Email).FirstOrDefault();

            if (verifyEmail != null)
            {

                ModelState.AddModelError("", "Email in use");
            }
            else
            {

                if (ModelState.IsValid)
                {
                    _context.userAccount.Add(user);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Message = user.FirstName + " " + user.LastName + " Usuario Registrado";
                }


                else
                {
                    reason = "No se pudo registrar";
                   
                }

            }
            return View();
        }

        public ActionResult Perfil(UserAccount user)
        {

            return View();
          
        }
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAccount user)
        {


            var accountUn = _context.userAccount.Where(u => u.Username.Trim() == user.Username && u.Password.Trim() == user.Password).FirstOrDefault();
            var accountEm = _context.userAccount.Where(u => u.Email.Trim() == user.Email && u.Password.Trim() == user.Password).FirstOrDefault();


            if (accountUn != null)
            {

               
                HttpContext.Session.SetString("UserID", accountUn.UserID.ToString());
                HttpContext.Session.SetString("Username", accountUn.Username);
                return RedirectToAction("Welcome");

            }
            else
            {
                ModelState.AddModelError("", "USUARIO OCONTRASEÑA INCORRECTA.");

            }


            if (accountEm != null)
            {
               
                HttpContext.Session.SetString("UserID", accountEm.UserID.ToString());
                HttpContext.Session.SetString("Username", accountEm.Username);
                return RedirectToAction("Welcome");

            }
            else
            {
                ModelState.AddModelError("", "EMAIL OCONTRASEÑA INCORRECTA.");
            }

            //var perfil=_context.userAccount


            return View();

        }


    

        public ActionResult Welcome()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}