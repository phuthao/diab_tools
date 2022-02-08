// ------------------------------------------------------------------------
// Cactus Software Solutions © 2015-2019 Vietnam, Inc. All rights reserved.
// Project Name:  DiaB.Core
// File Name:     NotEmptyAttribute.cs
// Created Date:  2018/10/15 6:18 PM
// ------------------------------------------------------------------------

using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace DiaB.Core.Common.Attributes
{
    public class NotEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is ICollection collection)
            {
                return collection.Count > 0;
            }

            return false;
        }
    }
}
