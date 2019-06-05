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
using WEBASPLOGIN.Helper;

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

       

    
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAccount user )
        {
           
            var accountEm = _context.userAccount.Where(u => u.Email.Trim() == user.Email && u.Password.Trim() == EncodeDecodeSecurity.EncriptPassword(user.Password)).FirstOrDefault();
            ///user.Password = EncodeDecodeSecurity.DecryptPassword(user.Password);

            if (accountEm != null)
            {
               
                HttpContext.Session.SetString("UserID", accountEm.UserID.ToString());
                HttpContext.Session.SetString("Username", accountEm.Username);
                return RedirectToAction("Details", "Profile", new { id = accountEm.UserID });

            }
            else
            {
                ModelState.AddModelError("", "EMAIL OCONTRASEÑA INCORRECTA.");
            }

            //var perfil=_context.userAccount


            return View();

        }
      
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "UserAccounts");
        }
    }
}