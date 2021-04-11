using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HouseOfSoulSounds.Models.Domain.Entities;
using HouseOfSoulSounds.Models.Domain.Repositories.Abstract;

namespace HouseOfSoulSounds.Models.Domain.Repositories.EntityFramework
{
    public class EFInstrumentsItemsRepository : IInstrumentsItemsRepository
    {
        private readonly EFAppDbContext context;
        public EFInstrumentsItemsRepository(EFAppDbContext context) => this.context = context;
        public IQueryable<InstrumentItem> Items => context.InstrumentItems;

        public InstrumentItem GetItemById(Guid id) => context.InstrumentItems.FirstOrDefault(x => x.Id == id);

        public void SaveItem(InstrumentItem entity)
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteItem(Guid id)
        {
            context.InstrumentItems.Remove(new InstrumentItem() { Id = id });
            context.SaveChanges();
        }
        public IQueryable<InstrumentItem> GetMessages(Guid id)
        {
            return (IQueryable<InstrumentItem>)context.Messages.Select(x => x.InstrumentItemId == id);
        }
    }
}
