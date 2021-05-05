using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseOfSoulSounds.Areas.Admin.Models;
using HouseOfSoulSounds.Models.Domain.Entities;
using HouseOfSoulSounds.Models.Domain.Repositories.EntityFramework;

namespace HouseOfSoulSounds.Models.Domain.Repositories.Abstract
{
    public interface IPageRepository:IPageItemsBaseRepository<NewPage>
    {
        IQueryable<NewPage> GetPages(Guid id);
    }
}
