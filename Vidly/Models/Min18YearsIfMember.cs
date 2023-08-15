using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class Min18YearsIfMember : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;
            if(customer.MembershipTypeId == 0 || customer.MembershipTypeId == 1)
            {
                return ValidationResult.Success;
            }
            if(customer.BirthDate is null)
            {
                return new ValidationResult("Birth Date is Required");
            }
            var age = DateTime.Now.Year - customer.BirthDate.Value.Year;
            return age >= 18 ? ValidationResult.Success : new ValidationResult("Customer should be atleast 18 years to be a member");
        }
    }
}