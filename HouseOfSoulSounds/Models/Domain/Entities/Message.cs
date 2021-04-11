using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseOfSoulSounds.Models.Domain.Entities
{
    public  class Message 
    {
        public Message() => DateAdded = DateTime.UtcNow.Date;
        [Required]
        public Guid Id { get;set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [Required]
        public Guid InstrumentItemId { get; set; }
        public InstrumentItem InstrumentItem { get; set; }

        [Display(Name = "Текст"), MaxLength(256)]
        public string Text { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Дата и время")]
        public DateTime DateAdded { get; set; }

        
    }
}
