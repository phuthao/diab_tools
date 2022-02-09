using System;
using System.ComponentModel;
using CpTech.Core.Middle.Dtos;
using Microsoft.AspNetCore.Http;

namespace DiaB.Middle.Dtos.FileUploadDtos
{
    public partial class FileUploadDtos
    {
        public partial class AppImportStaff : BaseItemDto
        {

            [DisplayName("File Import")]
            public IFormFile file { get; set; }
        }
    }
}
