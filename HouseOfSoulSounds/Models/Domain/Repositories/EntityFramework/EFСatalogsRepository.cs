using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HouseOfSoulSounds.Models.Domain.Entities;
using HouseOfSoulSounds.Models.Domain.Repositories.Abstract;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using HouseOfSoulSounds.Helpers;

namespace HouseOfSoulSounds.Models.Domain.Repositories.EntityFramework
{
    public class EFCatalogsRepository : ICatalogsRepository
    {
        private readonly EFAppDbContext context;
     

        public EFCatalogsRepository(EFAppDbContext context) => this.context = context;
        public IQueryable<Catalog> Items => context.Catalogs;
        
        public void DeleteItem(Guid id)
        {
            foreach (var item in context.InstrumentItems.Where(x => x.CatalogId == id))
            {
                context.InstrumentItems.Remove(item);
            }

            context.Catalogs.Remove(new Catalog() { Id = id });
            context.SaveChanges();
        }

        public  IQueryable<InstrumentItem> GetInstruments(Guid id)
        {

            //   return (IQueryable<InstrumentItem>)context.InstrumentsItems?.Select(x => x.CatalogId == id).AsEnumerable();
            var items = context.InstrumentItems;
            if (items is not null && items.Any())
            {
               
                return ((IQueryable<InstrumentItem>)items.Where(x => x.CatalogId == id)).AsQueryable();
            }
            return null;
        }

        public Catalog GetItemById(Guid id) => context.Catalogs.FirstOrDefault(x => x.Id == id);

        public void SaveItem(Catalog entity)
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

     

    }
}
