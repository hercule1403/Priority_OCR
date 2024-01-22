using GlobalDMS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace GlobalDMS.Data.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        public string AddDocument(Document document)
        {
            string resultMessage = ""; // Initialize the result message
            SqlConnection connection = null;

            try
            {
                connection = DatabaseHelper.GetOpenConnection();

                using (SqlCommand cmd = new SqlCommand("AddDocument", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@JsonString", document.JsonString);
                    cmd.Parameters.AddWithValue("@DocumentType", document.DocumentType);
                    cmd.Parameters.AddWithValue("@TransactionType", document.TransactionType);
                    cmd.Parameters.AddWithValue("@PdfFile", document.PdfFile);
                    cmd.Parameters.AddWithValue("@SearchKey", document.SearchKey);
                    cmd.Parameters.AddWithValue("@OcrType", document.OcrType);
                    // Add an output parameter to capture the result message from the stored procedure
                    SqlParameter resultParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 2000);
                    resultParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(resultParam);

                    cmd.ExecuteNonQuery();

                    // Get the result message from the output parameter
                    if (resultParam.Value != DBNull.Value)
                    {
                        resultMessage = resultParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here, log them, etc.
                resultMessage = "An error occurred while processing the request: " + ex.Message;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return resultMessage;
        }

        public List<Document> GetDocuments()
        {
            List<Document> documents = new List<Document>();

            using (SqlConnection connection = DatabaseHelper.GetOpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand("GetDocuments", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Document document = new Document
                            {
                                JsonString = reader["JsonString"].ToString(),
                                DocumentType = reader["DocumentType"].ToString(),
                                TransactionType = reader["TransactionType"].ToString(),
                                CreatedDate = reader["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreatedDate"]),
                                UpdatedDate = reader["UpdatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedDate"]),
                                PdfFile = reader["PdfFile"].ToString(),
                            };
                            documents.Add(document);
                        }
                    }
                }
            }
            return documents;
        }

        public Document GetDocumentsByFilename(string filename)
        {
            Document document = new Document();
            using (SqlConnection connection = DatabaseHelper.GetOpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand("GetDocumentsbyPdf", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add the @filename parameter to the SqlCommand
                    cmd.Parameters.Add(new SqlParameter("@PdfFile", SqlDbType.NVarChar, 255));
                    cmd.Parameters["@PdfFile"].Value = filename;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            document = new Document
                            {
                                DocumentID = (int)reader["DocumentID"],
                                JsonString = reader["JsonString"].ToString(),
                                DocumentType = reader["DocumentType"].ToString(),
                                TransactionType = reader["TransactionType"].ToString(),
                                CreatedDate = reader["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreatedDate"]),
                                UpdatedDate = reader["UpdatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedDate"]),
                                PdfFile = reader["PdfFile"].ToString(),
                                TargetField = reader["TargetField"].ToString(),
                                SearchKey = reader["SearchKey"].ToString(),
                                OcrType = reader["OcrType"].ToString(),
                            };
                        }
                    }
                }
            }
            return document;
        }

        public void UpdateDocument(Document document)
        {
            using (SqlConnection connection = DatabaseHelper.GetOpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand("UpdateDocument", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TargetField", document.TargetField);
                    cmd.Parameters.AddWithValue("@PdfFile", document.PdfFile);
                    cmd.Parameters.AddWithValue("@DocumentType", document.DocumentType);
                    cmd.Parameters.AddWithValue("@TransactionType", document.TransactionType);
                    cmd.Parameters.AddWithValue("@SearchKey", document.SearchKey);
                    cmd.Parameters.AddWithValue("@OcrType", document.OcrType);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Documents> GetDocumentName()
        {
            List<Documents> documents = new List<Documents>();

            using (SqlConnection connection = DatabaseHelper.GetOpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand("GetDocumentName", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Documents document = new Documents
                            {
                                DocumentTypeId = (int)reader["DocumentTypeId"],
                                DocumentName = reader["DocumentName"].ToString(),

                            };
                            documents.Add(document);
                        }
                    }
                }
            }
            return documents;
        }

        public List<SubDocuments> GetSubDocumentDropdown(long documentTypeId)
        {
            List<SubDocuments> subDocuments = new List<SubDocuments>();
            using (SqlConnection connection = DatabaseHelper.GetOpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand("GetSubDocumentDropdown", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add the @filename parameter to the SqlCommand
                    cmd.Parameters.Add(new SqlParameter("@DocumentTypeId", SqlDbType.Int));
                    cmd.Parameters["@DocumentTypeId"].Value = documentTypeId;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SubDocuments subDocument = new SubDocuments
                            {
                                DocumentTypeId = (int)reader["DocumentTypeId"],
                                SubDocumentName = reader["SubDocumentName"].ToString(),
                            };
                            subDocuments.Add(subDocument);
                        }
                    }
                }
            }
            return subDocuments;
        }

        public string InsertSubDocumentDropdown(long documentTypeId, string subDocumentName)
        {
            string resultMessage = ""; // Initialize the result message
            SqlConnection connection = null;

            try
            {
                connection = DatabaseHelper.GetOpenConnection();

                using (SqlCommand cmd = new SqlCommand("InsertSubDocumentDropdown", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DocumentTypeId", documentTypeId);
                    cmd.Parameters.AddWithValue("@SubDocumentName", subDocumentName);

                    // Add an output parameter to capture the result message from the stored procedure
                    SqlParameter resultParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 2000);
                    resultParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(resultParam);

                    cmd.ExecuteNonQuery();

                    // Get the result message from the output parameter
                    if (resultParam.Value != DBNull.Value)
                    {
                        resultMessage = resultParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here, log them, etc.
                resultMessage = "An error occurred while processing the request: " + ex.Message;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return resultMessage;
        }

        //public void DeleteDocument(long documentID)
        //{
        //    using (SqlConnection connection = DatabaseHelper.GetOpenConnection())
        //    {
        //        using (SqlCommand cmd = new SqlCommand("DeleteDocument", connection))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@DocumentID", documentID);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}

        public string DeleteDocument(string fileType)
        {
            string resultMessage = ""; // Initialize the result message
            SqlConnection connection = null;

            try
            {
                connection = DatabaseHelper.GetOpenConnection();

                using (SqlCommand cmd = new SqlCommand("DeleteDocumentsByPdfFile", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PdfFile", fileType);

                    // Add an output parameter to capture the result message from the stored procedure
                    SqlParameter resultParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 150);
                    resultParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(resultParam);

                    cmd.ExecuteNonQuery();

                    // Get the result message from the output parameter
                    if (resultParam.Value != DBNull.Value)
                    {
                        resultMessage = resultParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here, log them, etc.
                resultMessage = "An error occurred while processing the request: " + ex.Message;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return resultMessage;
        }
    }
}