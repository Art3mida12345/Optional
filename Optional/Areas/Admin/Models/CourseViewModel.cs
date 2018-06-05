using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Optional.Util;

namespace Optional.Areas.Admin.Models
{
    public class CourseViewModel:IValidatableObject
    {
        public int CourseId { get; set; }

        [Required]
        public string Theme { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [DataType(DataType.Date)]
        [MyDate(ErrorMessage = "Start date can not be in the past")]
        public DateTime StartDate { get; set; }

        public string LecturerName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate < StartDate)
            {
                yield return
                    new ValidationResult(errorMessage: "EndDate must be greater than StartDate",
                        memberNames: new[] { "EndDate" });
            }
        }
    }
}