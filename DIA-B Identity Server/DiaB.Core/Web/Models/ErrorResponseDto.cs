// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     ErrorResponseDto.cs
// Created Date:  2018/10/12 2:37 PM
// ------------------------------------------------------------------------

namespace DiaB.Core.Web.Models
{
    public class ErrorResponseDto<T>
    {
        public string Code { get; set; }

        public T Message { get; set; }

        public string Trace { get; set; }
    }
}
