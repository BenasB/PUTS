using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace PUTSWeb.Helpers
{
    public class SourceFileValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            IFormFile formFile = (IFormFile)value;

            if (formFile != null && !Path.GetExtension(formFile.FileName).Equals(".cpp")) // TODO: Don't hardcode .cpp
            {
                return false;
            }
            else
                return true;
        }
    }

    public class SourceFileValidationAdapter : AttributeAdapterBase<SourceFileValidation>
    {
        public SourceFileValidationAdapter(SourceFileValidation attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer) { }

        public override void AddValidation(ClientModelValidationContext context) { }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
