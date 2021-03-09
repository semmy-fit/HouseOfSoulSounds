using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HouseOfSoulSounds.Models.Domain.Entities;
using HouseOfSoulSounds.Models.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace HouseOfSoulSounds.Models.Domain.Repositories.EntityFramework
{
    public class EFMessagesRepository : IMessageRepository
    {
        private readonly EFAppDbContext context;
        public EFMessagesRepository(EFAppDbContext context) => this.context = context;
        public IQueryable<Message> Items => context.Messages;

        public void DeleteItem(Guid id)
        {
            context.Messages.Remove(new Message() { Id = id });
            context.SaveChanges();
        }

        public void DeleteMessagesByUserId(string id)
        {
            context.Messages.Remove(new Message() {UserId = id });
            context.SaveChanges();
        }

        public Message GetItemById(Guid id)
        {
            return context.Messages.FirstOrDefault(x => x.Id == id);
        }

       

        public void SaveItem(Message entity)
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
