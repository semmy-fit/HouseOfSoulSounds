using HouseOfSoulSounds.Models.Domain;
using HouseOfSoulSounds.Models.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HouseOfSoulSounds.Areas.Admin.Models
{
    public class ViewModel
    {
      
      public IQueryable<EditCatalogsModel> editCatalogs { get; set; }
    public IQueryable<InstrumentItem> editInstruments { get; set; }
      
       
}
}
