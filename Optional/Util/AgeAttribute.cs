using System;
using System.ComponentModel.DataAnnotations;

namespace Optional.Util
{
    public class AgeAttribute : ValidationAttribute
    {
        public AgeAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            if (((DateTime)value)<=DateTime.Now.AddYears(-16)
                && (DateTime)value>=DateTime.Now.AddYears(-85))
            {
                return true;
            }

            return false;
        }
    }
}