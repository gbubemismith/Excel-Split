using CsvUpload.Models;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CsvUpload.Controllers.Api
{
    public class UploadController : ApiController
    {
        [HttpPost]
        public IHttpActionResult UploadFile()
        {
            string[] extensions = { ".xls", ".xlsx" };

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');

                var buffer = file.ReadAsByteArrayAsync();




            }  

            

            return Ok();
        }
    }
}
