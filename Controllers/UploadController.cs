using CsvUpload.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CsvUpload.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
             
            return View(new List<BidModel>());
        }

        //[HttpPost]
        //public ActionResult Index(HttpPostedFileBase postedFile)
        //{

        //    var csvData = new List<BidModel>();
        //    string filePath = string.Empty;

        //    if (postedFile != null)
        //    {
        //        string path = Server.MapPath("~Uploads/");
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        filePath = path + Path.GetFileName(postedFile.FileName);

        //        string extension = Path.GetExtension(postedFile.FileName);
        //        postedFile.SaveAs(filePath);

        //        //Read csvData
        //        csvData = ProcessFile(filePath);
        //    }

        //    return View(csvData);

        //}

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            GeneratePdf();

            var bidsList = new List<BidModel>();
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));


                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentsheet = package.Workbook.Worksheets;
                        var worksheet = currentsheet.First();
                        var noOfCol = worksheet.Dimension.End.Column;
                        var noOfRow = worksheet.Dimension.End.Row;

                        var tempZone = string.Empty;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var bids = new BidModel();

                            bids.Zone = worksheet.Cells[rowIterator, 1].Value.ToString();

                            if (tempZone == bids.Zone || string.IsNullOrEmpty(tempZone))
                            {
                                tempZone = bids.Zone;

                                bids.Branch = worksheet.Cells[rowIterator, 2].Value.ToString();

                                bids.Lc_number = worksheet.Cells[rowIterator, 3].Value.ToString();
                                bids.Acct_no = worksheet.Cells[rowIterator, 4].Value.ToString();
                                bids.Customers_name = worksheet.Cells[rowIterator, 5].Value.ToString();
                                bids.Amount_won = decimal.Parse(worksheet.Cells[rowIterator, 6].Value.ToString());

                                bidsList.Add(bids);
                            }
                            else
                            {
                                //build it

                                var list = bidsList;
                                CreateExcel(list);
                                bidsList = new List<BidModel>();
                                tempZone = string.Empty;
                                --rowIterator;
                                
                            }


                        }
                    }
                }
            }
            TempData["bidsList"] = bidsList;

            return View(bidsList);
        }



        private static List<BidModel> ProcessFile(string path)
        {
            return System.IO.File.ReadAllLines(path)
                .Where(line => line.Length > 1)
                .Select(BidModel.ParseFromCSV)
                .ToList();
        }

        private static bool CreateExcel(List<BidModel> inputBids)
        {
            var created = false;

            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("Worksheet1");

                var headerRow = new List<string[]>()
                {
                    new string[] {"Zone", "Branch", "LC Number", "Acct No", "Customers Name", "Amount Won"}
                };

                //Determine header range
                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                var worksheet = excel.Workbook.Worksheets["Worksheet1"];



                var count = inputBids.Count();

                worksheet.Cells[headerRange].LoadFromArrays(headerRow);




                for (int i = 2; i <= count + 1; )
                {
                    foreach (var item in inputBids)
                    {
                        worksheet.Cells["A" + i].Value = item.Zone;
                        worksheet.Cells["B" + i].Value = item.Branch;
                        worksheet.Cells["C" + i].Value = item.Lc_number;
                        worksheet.Cells["D" + i].Value = item.Acct_no;
                        worksheet.Cells["E" + i].Value = item.Customers_name;
                        worksheet.Cells["F" + i].Value = item.Amount_won;

                        i++;
                    }
                }

              

                FileInfo excelFile = new FileInfo(@"C:\Users\walter smith\Desktop\test.xlsx");
                excel.SaveAs(excelFile);

                created = true;
            }

            return created;
        }

        private static void GeneratePdf()
        {
            var doc1 = new Document();
            PdfWriter.GetInstance(doc1, new FileStream(@"C:\Users\walter smith\Desktop\" + "Doc1.pdf", FileMode.Create));
            doc1.Open();
            doc1.Add(new Paragraph("FX BIDS PDF"));
            doc1.Close();
        }

       
    }
}