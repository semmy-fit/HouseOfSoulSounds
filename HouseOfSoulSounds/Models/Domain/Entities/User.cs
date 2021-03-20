using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HouseOfSoulSounds.Helpers;
using Microsoft.AspNetCore.Identity;

namespace HouseOfSoulSounds.Models.Domain.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Аватар")]
        public string ImagePath { get; set; } = Config.DefaultAvatar;
        public bool? Blocked { get; set; } = false;
       




     

        
    }
}
