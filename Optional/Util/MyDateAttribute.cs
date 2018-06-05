using System;
using System.ComponentModel.DataAnnotations;

namespace Optional.Util
{
    public class MyDateAttribute:ValidationAttribute
    {
        public MyDateAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            if (((DateTime)value).CompareTo(DateTime.Today)>=0)
            {
                return true;
            }
            return false;
        }
    }
}