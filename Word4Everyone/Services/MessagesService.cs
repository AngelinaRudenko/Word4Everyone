using Word4Everyone.Services.Interfaces;

namespace Word4Everyone.Services
{
    public class MessagesService : IMessagesService
    {
        public string NoDocumentFound => "Документ не найден.";
        public string BadRequest => "Неверный запрос.";
        public string Saved => "Сохранено.";
        public string Deleted => "Удалено.";
    }
}
