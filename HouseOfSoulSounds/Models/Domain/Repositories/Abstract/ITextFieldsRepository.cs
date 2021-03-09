using HouseOfSoulSounds.Models.Domain.Entities;

namespace HouseOfSoulSounds.Models.Domain.Repositories.Abstract
{
    public interface ITextFieldsRepository : IPageItemsBaseRepository<TextField> {
        TextField GetItemByCodeWord(string codeWord);
    }
}
