using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ValidationsHelper
    {
        public static void ModelVaidation(object model)
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject
                (model, validationContext, validationResults, true);
            if(!isValid)
            {
                throw new ArgumentException(validationResults?.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
