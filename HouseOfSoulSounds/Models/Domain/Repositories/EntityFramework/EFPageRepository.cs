using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseOfSoulSounds.Areas.Admin.Models;
using HouseOfSoulSounds.Models.Domain.Entities;
using HouseOfSoulSounds.Models.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace HouseOfSoulSounds.Models.Domain.Repositories.EntityFramework
{
    public class EFPageRepository:IPageRepository
    {
        private readonly EFAppDbContext context;

        public EFPageRepository(EFAppDbContext context) => this.context = context;
        public IQueryable<NewPage> Items => context.Pages;

        public void DeleteItem(Guid id)
        {
            foreach (var item in context.Pages.Where(x => x.Id == id))
            {
                context.Pages.Remove(item);
            }

            context.Pages.Remove(new NewPage() { Id = id });
            context.SaveChanges();
        }
        public  NewPage GetItemById(Guid id) => context.Pages.Include(x=>x.PageTitle).FirstOrDefault(x => x.Id == id);
        public IQueryable<NewPage> GetPages(Guid id)
        {

            //   return (IQueryable<InstrumentItem>)context.InstrumentsItems?.Select(x => x.CatalogId == id).AsEnumerable();
            var items = context.Pages;
            if (items is not null && items.Any())
            {

                return ((IQueryable<NewPage>)items.Where(x => x.Id == id)).AsQueryable();
            }
            return null;
        }
        public void SaveItem(NewPage entity)
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }



    }
}
