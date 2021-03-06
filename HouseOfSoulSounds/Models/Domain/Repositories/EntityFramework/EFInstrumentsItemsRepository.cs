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

        public InstrumentItem GetItemById(Guid id)
        {
            var messages = context.Messages.Where(x => x.InstrumentItemId == id).ToList();
            var inst = context.InstrumentItems.ToList();
            //var res = context.InstrumentItems.FirstOrDefault(x => x.Id == id);  
            var res = inst.FirstOrDefault(x => x.Id == id);  
            if (res is null) return null;
            if (messages.Any()) res.Messages = messages.ToList();
            return res;
           // context.InstrumentItems.Include(i => i.Messages).FirstOrDefault(x => x.Id == id);
        }
        public IQueryable<InstrumentItem> GetInstrumentInCatalog(Guid id)
        {
            var item = context.InstrumentItems;
            if(item is not null && item.Any())
            {
                item.Select(z => z.Title);
                return ((IQueryable<InstrumentItem>)item.Select(x =>x.Title));
            }
            return null; 
        }
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
            foreach (var item in context.InstrumentItems.Where(x => x.CatalogId == id))
            {
                context.InstrumentItems.Remove(item);
            }
            context.InstrumentItems.Remove(new InstrumentItem() { Id = id });
            context.SaveChanges();
        }
        public IQueryable<InstrumentItem> GetMessages(Guid id)
        {
            return (IQueryable<InstrumentItem>)context.Messages.Select(x => x.InstrumentItemId == id);
        }
    }
}
