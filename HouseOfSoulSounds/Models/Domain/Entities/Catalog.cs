using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HouseOfSoulSounds.Models.Domain.Entities
{
    public class Catalog : ValueObject
    {
        [Required]
        public Guid Id { get; set; }
        [Display(Name = "Название каталога")]
        public string Title { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
