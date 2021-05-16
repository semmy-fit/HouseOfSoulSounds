
using HouseOfSoulSounds.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HouseOfSoulSounds.Models.Domain.Entities
{
    public sealed class InstrumentItem : EntityBase
    {
        [Required(ErrorMessage = "Заполните название инструмента")]
        [Display(Name = "Название инструмента")]
        public override string Title { get; set; }
        [Display(Name = "Краткое описание инструмента")]
        public override string Subtitle { get; set; }

        [Display(Name = "Характеристики инструмента")]
        public override string Text { get; set; }
        [Required]
        public Guid  CatalogId { get; set; }
        [ForeignKey("CatalogId")]
        public Catalog Catalog { get; set; }

        public  IList<Message> Messages { get; set; } = new List<Message>();
        public IQueryable<EditCatalogsModel> editCatalogs { get; set; }
        [NotMapped]
        public string BlockedName { get; set; }
        [NotMapped]
        public ChatModel Chat { get; set; }
    }

}
