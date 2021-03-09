using System;
using System.Linq;
using HouseOfSoulSounds.Models.Domain.Entities;

namespace HouseOfSoulSounds.Models.Domain.Repositories.Abstract
{
    public interface IInstrumentsItemsRepository : IPageItemsBaseRepository<InstrumentItem> 
    {
        IQueryable<InstrumentItem> GetMessages(Guid id);



    }
}
