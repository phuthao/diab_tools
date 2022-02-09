using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CpTech.Core.Common.Dtos;
using DiaB.Middle.Dtos.FileUploadDtos;
using DiaB.Middle.Services.Interfaces;
using OfficeOpenXml;

namespace DiaB.Middle.Services
{
    public class ImportService :  IImportService
    {
        public ImportService()
            : base()
        {
        }


        public FileUploadDtos.StaffInfo ImportStaff(FileUploadDtos.AppImportStaff input, ActionContext context)
        {
            FileUploadDtos.StaffInfo result = new FileUploadDtos.StaffInfo();
            string rootFolder = "../";
            string fileName = Guid.NewGuid().ToString() + input.file.FileName;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));
            using (var stream = new MemoryStream())
            {
                input.file.CopyToAsync(stream);
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
                    
                }
                else
                {
                    //read excel file data and add data in  model.StaffInfoViewModel.StaffList
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        result.StaffList.Add(new FileUploadDtos.StaffInfo
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

                    }



                }
            }
            return result;
        }
    }
}
