using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HouseOfSoulSounds.Models.Domain.Entities;

namespace HouseOfSoulSounds.Areas.Admin.Models
{
    public class EditCatalogsModel
    {
        [Required]
        public Guid Id { get; set; }

        [MinLength(2, ErrorMessage ="Минимум 2 буквы")]
        [MaxLength(450)]
        [RegularExpression("[a-zA-ZА-Яа-яЁё]", ErrorMessage = "Должно начинаться с буквы")]
        [Display(Name = "Название каталога")]
        public string Title { get; set; }

        [Display(Name = "Инструменты")]
        public IEnumerable<InstrumentItem>? InstrumentItems;
        


    }
}
