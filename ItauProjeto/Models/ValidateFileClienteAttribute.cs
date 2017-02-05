using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ItauProjeto.Models
{
    public class ValidateFileClienteAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file == null)
            {   
                return true;
            }

            if (file.ContentLength > 10 * 1024 * 1024)
            {
                return false;
            }

            return true;
        }

    }
}