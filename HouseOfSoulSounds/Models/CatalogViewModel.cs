using HouseOfSoulSounds.Areas.Admin.Models;
using HouseOfSoulSounds.Models.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace HouseOfSoulSounds.Models
{
    public class CatalogViewModel
    {
        public IQueryable<EditCatalogsModel> editCatalogs { get; set; }
        public IQueryable<InstrumentItem> Instruments { get; set; }

    }
}
