using System.Collections.Generic;
using HouseOfSoulSounds.Models.Domain.Repositories.Abstract;

namespace HouseOfSoulSounds.Models.Domain
{
    public class DataManager
    {
        public ITextFieldsRepository TextFields { get; set; }
        public ICatalogsRepository Catalogs { get; set; }
        public IMessageRepository Message { get; set; }
        public IInstrumentsItemsRepository Instruments { get; set; }

        public DataManager(
            ITextFieldsRepository textFieldsRepository,
           ICatalogsRepository catalogsRepository,
           IMessageRepository messageRepository,
           IInstrumentsItemsRepository instrumentsItemsRepository
           )

        {
            TextFields = textFieldsRepository;
            Catalogs = catalogsRepository;
            Message = messageRepository;
            Instruments = instrumentsItemsRepository;
        }
    }
}
