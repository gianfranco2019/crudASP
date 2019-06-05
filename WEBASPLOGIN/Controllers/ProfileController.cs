﻿using System;
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
            
        // POST: Profile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,LastName,Email,Username,Password,Image,ConfirmPassword")] UserAccount userAccount, List<IFormFile> Image)
        {


            if (id != userAccount.UserID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
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
                try
                    {
                    
                        _context.Update(userAccount);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserAccountExists(userAccount.UserID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }


                
            

            
                return RedirectToAction("Details", "Profile", new { id = userAccount.UserID });

            }
            return View(userAccount);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditImage(int id,UserAccount userAccount, List<IFormFile> Image)
        {
            foreach (var item in _context.userAccount)
            {
                string[] hola = { item.Email, item.Username, item.Password, item.ConfirmPassword,item.Image.ToString() };


                if (id != userAccount.UserID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
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
                    try
                    {

                        _context.Update(hola);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserAccountExists(userAccount.UserID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }



                }

           






                return RedirectToAction("Details", "Profile", new { id = userAccount.UserID });

            }
            return View(userAccount);
        }



        private bool UserAccountExists(int id)
        {
            return _context.userAccount.Any(e => e.UserID == id);
        }
    }
}
