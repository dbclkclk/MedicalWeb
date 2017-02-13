using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalWebService.Validators
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple =false)]
    public class MinimumDays : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            DateTime now = DateTime.Now;
            DateTime param = (DateTime)value;
            double days = (param-now).TotalDays;
            if (days < -7)
                return new ValidationResult("");
            return ValidationResult.Success;
        }
    }
}