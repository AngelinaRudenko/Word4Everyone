namespace Word4Everyone.Services.Interfaces
{
    public interface IMessagesService
    {
        string NoDocumentFound { get; }
        string BadRequest { get; }
        string Saved { get; }
        string Deleted { get; }
    }
}
