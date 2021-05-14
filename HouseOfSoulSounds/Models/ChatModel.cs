using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HouseOfSoulSounds.Models.Domain.Entities;

namespace HouseOfSoulSounds.Models
{
    public class ChatModel
    {
        [Display(Name = "Логин")]
        public string UserName { get; set; }
    
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public DateTime UpTo { get; set; }
        public bool IsBlocked { get; set; }

        public bool IsModerator { get; set; }

        public bool IsWriter { get; set; }

        public IQueryable<Message> messages { get; set; }
    }
}
