using DiaB.Import.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiaB.Import.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public ActionResult File()
        {
            FileUploadViewModel model = new FileUploadViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult File(FileUploadViewModel model)
        {
            string rootFolder = _hostingEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + model.XlsFile.FileName;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));
            using (var stream = new MemoryStream())
            {
                model.XlsFile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    package.SaveAs(file);
                    //save excel file in your wwwroot folder and get this excel file from wwwroot
                }
            }
            //After save excel file in wwwroot and then
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                {
                    //return or alert message here
                }
                else
                {
                    //read excel file data and add data in  model.StaffInfoViewModel.StaffList
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        model.StaffInfoViewModel.StaffList.Add(new StaffInfoViewModel
                        {
                            user_name = (worksheet.Cells[row, 1].Value ?? string.Empty).ToString().Trim(),
                            user_code = (worksheet.Cells[row, 2].Value ?? string.Empty).ToString().Trim(),
                            survey_type = (worksheet.Cells[row, 3].Value ?? string.Empty).ToString().Trim(),
                            survey_code = (worksheet.Cells[row, 4].Value ?? string.Empty).ToString().Trim(),
                            survey_name = (worksheet.Cells[row, 5].Value ?? string.Empty).ToString().Trim(),

                            user_age = (worksheet.Cells[row, 6].Value ?? string.Empty).ToString().Trim(),
                            user_gender = (worksheet.Cells[row, 7].Value ?? string.Empty).ToString().Trim(),
                            user_phone = (worksheet.Cells[row, 8].Value ?? string.Empty).ToString().Trim(),
                            survey_day = (worksheet.Cells[row, 9].Value ?? string.Empty).ToString().Trim(),
                            user_province = (worksheet.Cells[row, 10].Value ?? string.Empty).ToString().Trim(),
                            Count = (worksheet.Cells[row, 11].Value ?? string.Empty).ToString().Trim(),


                        });
                      
            /*  < th > Bệnh nhân </ th >
   
           < th > Mã số </ th >
      
              < th > Loại khảo sát</ th >
         
                 < th > Tên khảo sát</ th >
            
                    < th > Mã khảo sát</ th >
               
                       < th > Tuổi </ th >
               
                       < th > Giới tính </ th >
                  
                          < th > Số điện thoại</ th >
                     
                             < th > Ngày thực hiện khảo sát</ th >
                        
                                < th > Tỉnh thành </ th >
*/
                    }



                }
            }
            //return same view and  pass view model 
            return View(model);
        }
    }
}
