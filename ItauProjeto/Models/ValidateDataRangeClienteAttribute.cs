using System;
using System.ComponentModel.DataAnnotations;

namespace ItauProjeto.Models
{
    public class ValidateDataRangeClienteAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // your validation logic
            if (Convert.ToDateTime(value) >= Convert.ToDateTime("01/01/1900") && Convert.ToDateTime(value) <= Convert.ToDateTime("01/12/2100"))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Período inválido.");
            }
        }
    }
}