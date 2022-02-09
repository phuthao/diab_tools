using System;
using System.Collections.Generic;
using System.ComponentModel;
using CpTech.Core.Middle.Dtos;
using Microsoft.AspNetCore.Http;

namespace DiaB.Middle.Dtos.FileUploadDtos
{
    public partial class FileUploadDtos
    {
        public partial class StaffInfo : BaseItemDto
        {
            /* public string FirstName { get; set; }
                  public string LastName { get; set; }
                  public string Email { get; set; }
                  public string Sex { get; set; }
                  public string Count { get; set; }*/

            public string Count { get; set; }
            public string user_code { get; set; }
            public string user_name { get; set; }
            public string user_gender { get; set; }
            public string user_age { get; set; }

            public string user_yearofbirth { get; set; }
            public string survey_type { get; set; }
            public string survey_name { get; set; }
            public string survey_code { get; set; }
            public string code { get; set; }
            public string user_phone { get; set; }
            public string survey_day { get; set; }
            public string user_province { get; set; }


            /*public string user_carrer { get; set; }
            public int user_phone { get; set; }
            public string user_hoobit { get; set; }
            public int user_yearofbirth { get; set; }
            public int user_yearofbirth { get; set; }*/












            public List<StaffInfo> StaffList { get; set; }
            public StaffInfo()
            {
                StaffList = new List<StaffInfo>();
            }
        }
    }
}
