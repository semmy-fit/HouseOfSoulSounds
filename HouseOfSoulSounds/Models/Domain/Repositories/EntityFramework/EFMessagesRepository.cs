using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HouseOfSoulSounds.Models.Domain.Entities;
using HouseOfSoulSounds.Models.Domain.Repositories.Abstract;
using HouseOfSoulSounds.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace HouseOfSoulSounds.Models.Domain.Repositories.EntityFramework
{
    public class EFMessagesRepository : IMessageRepository
    {
        private readonly EFAppDbContext context;
        public EFMessagesRepository(EFAppDbContext context) => this.context = context;
        public IQueryable<Message> Items => context.Messages;
        public IQueryable<User> users => context.Users;

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
            return context.Messages.Include(x=>x.User).FirstOrDefault(x => x.Id == id);
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
