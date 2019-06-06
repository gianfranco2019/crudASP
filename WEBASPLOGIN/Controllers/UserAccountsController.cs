using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nancy.Authentication.Forms;
using WEBASPLOGIN.Models;
using WEBASPLOGIN.Helper;
using System.Text.RegularExpressions;

namespace WEBASPLOGIN.Controllers
{
    public class UserAccountsController : Controller
    {
        private readonly OurDbContext _context;

        public UserAccountsController(OurDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            return View(await _context.userAccount.ToListAsync());
        }

      
      

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,FirstName,LastName,Email,Username,Password,Image,ConfirmPassword")] UserAccount userAccount,List<IFormFile>Image)
        {

         
            string reason = String.Empty;
            if (userAccount.Password !=null && userAccount.ConfirmPassword !=null)
            {
                userAccount.Password = EncodeDecodeSecurity.EncriptPassword(userAccount.Password);
                userAccount.ConfirmPassword = EncodeDecodeSecurity.EncriptPassword(userAccount.ConfirmPassword);
            }


        
            

            var verifyEmail = _context.userAccount.Where(u => u.Email == userAccount.Email).FirstOrDefault();
    
            if (verifyEmail != null)
            {

                ModelState.AddModelError("", "Email in use");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (Image != null)
                    {
                        foreach (var item in Image)
                        {
                       
                            if (item.Length > 0)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    await item.CopyToAsync(stream);
                                    userAccount.Image = stream.ToArray();
                                }
                            }
                       
                        }
                       
                        }

                    _context.Add(userAccount);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
            }
           
            return View(userAccount);
        }

          
    }
}
