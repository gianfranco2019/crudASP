﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WEBASPLOGIN.Models
{
    public class UserAccount
    {


        [Key]
        public int UserID { get; set; }
   
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Ejemplo: FirstName + SecondNamee ")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[a-zA-Z]+\s+[a-zA-Z]+$", ErrorMessage = "Ejemplo: First Last Name + Second Surname")]
    
        public string LastName { get; set; }
      
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Ejemplo:User2019")]
     
        public string Username { get; set; }


  
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!.%*?&]{8,16}", ErrorMessage = "Ejemplo:Pa$$w0rd")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "Contraseña no coincide.")]
        [DataType(DataType.Password)]
     
        public string ConfirmPassword { get; set; }



        //----------------------------------------------------

       
            public byte[] Image { get; set; }
        



      




    }

}



