using System;

namespace GlobalDMS.Data.Entities
{
    public class Document
    {
        public long DocumentID { get; set; }
        public string JsonString { get; set; }
        public string DocumentType { get; set; }
        public string TransactionType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string PdfFile { get; set; }
        public string TargetField { get; set; }
        public string DocumentName { get; set; }
        public string SearchKey { get; set; }
        public string OcrType { get; set; }
    }
    public class Documents
    {
        public long DocumentTypeId { get; set; }
        public string DocumentName { get; set; }
    }
    public class SubDocuments
    {
        public long SubDocumentTypeId { get; set; }
        public long DocumentTypeId { get; set; }
        public string SubDocumentName { get; set; }
    }

}