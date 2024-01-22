using GlobalDMS.Data.Repositories;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using GlobalDMS.Data.Entities;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using System.Web.Helpers;

namespace GlobalDMS.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentController()
        {
            _documentRepository = new DocumentRepository();
        }


        // POST: Document/AddDocument
        [HttpPost]
        [Route("AddDocument")]
        public JsonResult AddDocument()
        {
            var result = new
            {
                Status = "Error",
                StatusCode = 400,
                Message = "An error occurred while processing the request."
            };
            try
            {
                Stream req = Request.InputStream;
                req.Seek(0, System.IO.SeekOrigin.Begin);
                string json = new StreamReader(req).ReadToEnd();
                Document model = JsonConvert.DeserializeObject<Document>(json);

                var responce = _documentRepository.AddDocument(model);
                // Return a success response with status code and message
                result = new
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = responce
                };

            }
            catch (Exception ex)
            {
                // Return a failure response with status code and error message
                result = new
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                };
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult UpdateDocument()
        {
            var result = new
            {
                Status = "Error",
                StatusCode = 400,
                Message = "An error occurred while processing the request."
            };
            try
            {
                Stream req = Request.InputStream;
                req.Seek(0, System.IO.SeekOrigin.Begin);
                string json = new StreamReader(req).ReadToEnd();
                Document model = JsonConvert.DeserializeObject<Document>(json);

                _documentRepository.UpdateDocument(model);
                // Return a success response with status code and message
                result = new
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "Document updated successfully."
                };

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the document.");

                // Return a failure response with status code and error message
                result = new
                {
                    Status = "Error",
                    StatusCode = 500,
                    Message = ex.Message
                };
            }
            return Json(result);
        }

        //[HttpDelete]
        //public ActionResult DeleteDocument(long documentID)
        //{
        //    var result = new
        //    {
        //        Status = "Error",
        //        StatusCode = 400,
        //        Message = "An error occurred while processing the request."
        //    };
        //    try
        //    {

        //        _documentRepository.DeleteDocument(documentID);
        //        // Return a success response with status code and message
        //        result = new
        //        {
        //            Status = "Success",
        //            StatusCode = 200,
        //            Message = "Document Deleted successfully."
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, "An error occurred while creating the document.");

        //        // Return a failure response with status code and error message
        //        result = new
        //        {
        //            Status = "Error",
        //            StatusCode = 500,
        //            Message = ex.Message
        //        };
        //    }
        //    return Json(result);
        //}

        [HttpPost]
        [Route("DeleteDocumentByFileType")]
        public JsonResult DeleteDocument(string fileType)
        {
            var result = new
            {
                Status = "Error",
                StatusCode = 400,
                Message = "An error occurred while processing the request."
            };
            try
            {
                var responce = _documentRepository.DeleteDocument(fileType);
                // Return a success response with status code and message
                result = new
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = responce
                };

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the document.");

                // Return a failure response with status code and error message
                result = new
                {
                    Status = "Error",
                    StatusCode = 500,
                    Message = ex.Message
                };
            }
            return Json(result);
        }

        // GET: Document/GetDocuments
        [HttpGet]
        public JsonResult GetDocuments()
        {
            var someBigObject = _documentRepository.GetDocuments();

            JsonResult result = Json(someBigObject);
            result.MaxJsonLength = Int32.MaxValue;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        /*// GET: Document/GetDocuments
        [HttpGet]
        public ActionResult GetDocuments()
        {
            List<Document> documents = _documentRepository.GetDocuments();

            return Json(new { Success = true, Documents = documents }, JsonRequestBehavior.AllowGet);
        }*/

        [HttpGet]
        public ActionResult GetDocumentsByFilename(string PdfFile)
        {
            try
            {
                Document document = _documentRepository.GetDocumentsByFilename(PdfFile);

                if (document.DocumentID != 0)
                {
                    JsonResult result = Json(document);
                    result.MaxJsonLength = Int32.MaxValue;
                    result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    return result;
                }
                else
                {
                    return Json(new { success = false, message = "no documents found for the specified filename." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errormessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetDocumentName()
        {
            List<Documents> documents = _documentRepository.GetDocumentName();
            return Json(new { Success = true, Documents = documents }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSubDocumentDropdown(int DocumentTypeId)
        {
            try
            {
                List<SubDocuments> subDocument = _documentRepository.GetSubDocumentDropdown(DocumentTypeId);

                return Json(new { Success = true, Documents = subDocument }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errormessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult InsertSubDocumentDropdown(int documentTypeId, string subDocumentName)
        {
            var result = new
            {
                Status = "Error",
                StatusCode = 400,
                Message = "An error occurred while processing the request."
            };
            try
            {

                var responce = _documentRepository.InsertSubDocumentDropdown(documentTypeId, subDocumentName);
                // Return a success response with status code and message
                result = new
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = responce
                };

            }
            catch (Exception ex)
            {
                // Return a failure response with status code and error message
                result = new
                {
                    Status = "Error",
                    StatusCode = 400,
                    Message = ex.Message
                };
            }
            return Json(result);
        }

    }
}
