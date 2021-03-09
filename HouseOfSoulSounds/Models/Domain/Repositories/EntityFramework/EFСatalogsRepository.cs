using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HouseOfSoulSounds.Models.Domain.Entities;
using HouseOfSoulSounds.Models.Domain.Repositories.Abstract;


namespace HouseOfSoulSounds.Models.Domain.Repositories.EntityFramework
{
    public class EFCategoryInstrumentsRepository : ICatalogsRepository
    {
        private readonly EFAppDbContext context;
        public EFCategoryInstrumentsRepository(EFAppDbContext context) => this.context = context;
        public IQueryable<Catalog> Items => context.Catalogs;
        
        public void DeleteItem(Guid id)
        {
            context.InstrumentsItems.Remove(new InstrumentItem() { CatalogId = id }); 
            context.Catalogs.Remove(new Catalog() { Id = id });
            context.SaveChanges();
        }

        public IQueryable<InstrumentItem> GetInstruments(Guid id)
        {
            return (IQueryable<InstrumentItem>)context.InstrumentsItems.Select(x => x.CatalogId == id);
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
