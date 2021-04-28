namespace YoYo.ABPCommunity.Docs.Documents.Services
{
    public interface IDocumentStoreFactory
    {
        IDocumentStore Create(string documentStoreType);

        TDocumentStore Create<TDocumentStore>() where TDocumentStore : IDocumentStore;
    }
}
