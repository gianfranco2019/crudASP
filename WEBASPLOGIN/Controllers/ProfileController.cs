using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEBASPLOGIN.Helper;
using WEBASPLOGIN.Models;

namespace WEBASPLOGIN.Controllers
{
    public class ProfileController : Controller
    {
        private readonly OurDbContext _context;

        public ProfileController(OurDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int? id,UserAccount userAccount)
        {
            if (HttpContext.Session.GetString("UserID") == id.ToString())
            {
                if (id == null)
                {
                    return NotFound();
                }
                byte[] byteArray = _context.userAccount.Find(id).Image;

                if (byteArray != null)
                {
                    new FileContentResult(byteArray, "image/jpeg");
                }
                else
                {
                }
                 userAccount = await _context.userAccount
                    .FirstOrDefaultAsync(m => m.UserID == id);

                if (userAccount == null)
                {
                    return NotFound();
                }

            }
            else
            {
                return RedirectToAction("Index", "UserAccounts");

            }

                     
            

            return View(userAccount);
        }



        public async Task<IActionResult> Edit(int? id,UserAccount userAccount)
        {
            if (HttpContext.Session.GetString("UserID") != null )
            {
             

                if (id == null)
                {
                    return NotFound();
                }

                if (HttpContext.Session.GetString("UserID")==id.ToString())
                {
                    userAccount = await _context.userAccount.FindAsync(id);
                    if (userAccount == null)
                    {
                        return NotFound();
                    }
                }
                else
                {
                  return RedirectToAction("Index", "UserAccounts");

                }

                return View(userAccount);

            }



            else
            {
                return RedirectToAction("Index", "UserAccounts");

            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,FirstName,LastName,Email,Username,Password,Image,ConfirmPassword")] UserAccount userAccount, List<IFormFile> Image)
        {

            if (userAccount.Password != null && userAccount.ConfirmPassword != null)
            {
                userAccount.Password = EncodeDecodeSecurity.EncriptPassword(userAccount.Password);
                userAccount.ConfirmPassword = EncodeDecodeSecurity.EncriptPassword(userAccount.ConfirmPassword);
            }



            if (id != userAccount.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    foreach (var item2 in Image)
                {
                    if (item2.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await item2.CopyToAsync(stream);
                            userAccount.Image = stream.ToArray();
                        }
                    }
                }

                    _context.Update(userAccount);
                    await _context.SaveChangesAsync();


                    return RedirectToAction("Details", "Profile", new { id = userAccount.UserID });

                }

                 
            }
           return View(userAccount);
        }

        private bool UserAccountExists(int id)
        {
            return _context.userAccount.Any(e => e.UserID == id);
        }
    }
}
