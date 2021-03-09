using System;
using System.Linq;
using HouseOfSoulSounds.Models.Domain.Entities;

namespace HouseOfSoulSounds.Models.Domain.Repositories.Abstract
{
    public interface IMessageRepository : IPageItemsBaseRepository<Message>
    {
        public void DeleteMessagesByUserId(string id);

    }
}

