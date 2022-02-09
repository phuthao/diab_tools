using System;
using System.Threading.Tasks;
using CpTech.Core.Common.Dtos;
using CpTech.Core.Middle.Services;
using DiaB.Middle.Dtos.FileUploadDtos;

namespace DiaB.Middle.Services.Interfaces
{
    public interface IImportService : IService
    {
        FileUploadDtos.StaffInfo ImportStaff(FileUploadDtos.AppImportStaff input, ActionContext context);
    }
}
