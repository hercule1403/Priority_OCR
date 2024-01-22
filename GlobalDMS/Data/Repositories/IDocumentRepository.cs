using GlobalDMS.Data.Entities;
using System.Collections.Generic;

namespace GlobalDMS.Data.Repositories
{
    interface IDocumentRepository
    {

        string AddDocument(Document product);
        List<Document> GetDocuments();
        Document GetDocumentsByFilename(string filename);
        void UpdateDocument(Document product);
        List<Documents> GetDocumentName();
        //List<SubDocuments> GetSubDocumentDropdown();
        List<SubDocuments> GetSubDocumentDropdown(long documentTypeId);
        //Document GetSubDocumentDropdown(string documentTypeId);
        string InsertSubDocumentDropdown(long documentTypeId, string subDocumentName);
        //void DeleteDocument(long documentID);
        string DeleteDocument(string fileType);

    }
}
