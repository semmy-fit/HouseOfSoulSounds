using System.Collections.Generic;
using HouseOfSoulSounds.Models.Domain.Entities;
using HouseOfSoulSounds.Models.Domain.Repositories.Abstract;

namespace HouseOfSoulSounds.Models.Domain
{
    public class DataManager
    {
        public ITextFieldsRepository TextFields { get; set; }
        public ICatalogsRepository Catalogs { get; set; }
        public IMessageRepository Messages { get; set; }
        public IInstrumentsItemsRepository Instruments { get; set; }
        public IPageRepository Pages { get; set; }

        public DataManager(
            ITextFieldsRepository textFieldsRepository,
           ICatalogsRepository catalogsRepository,
           IMessageRepository messageRepository,
           IInstrumentsItemsRepository instrumentsItemsRepository,
           IPageRepository pageItemsBaseRepository
           )

        {
            TextFields = textFieldsRepository;
            Catalogs = catalogsRepository;
            Messages = messageRepository;
            Instruments = instrumentsItemsRepository;
            Pages = pageItemsBaseRepository;

        }
    }
}
